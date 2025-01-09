using LmsServices.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Common;
using OmSaiModels.Worker;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Worker.Implimentation;
using System.Net;

namespace GeneralTemplate.Areas.Worker.Controllers
{
	[Route("api/attendance")]
	//[ApiController]
	public class ApiAttendaceController : ControllerBase
	{
		private readonly WorkerService _workerService;
		private readonly WorkerAttendanceService _attendanceService;
		private readonly MultiMediaService _mms;

		public ApiAttendaceController()
		{
			_workerService = new WorkerService();
			_attendanceService = new WorkerAttendanceService();
			_mms = new MultiMediaService();
		}


		[HttpPost("worker-attendance-report")]
		[Authorize]
		public async Task<IActionResult> WorkerAttendanceReport([FromQuery] int? WorkerId, [FromQuery] int? SiteId, [FromQuery] DateOnly? CurrentDate)
		{
			// Validate the parameters
			if (WorkerId == null && SiteId == null && CurrentDate == null)
			{
				return BadRequest(new
				{
					success = false,
					data = (object)null,
					errors = new
					{
						message = "At least one parameter (WorkerId, SiteId, or CurrentDate) must be provided."
					}
				});
			}

			try
			{
				// Retrieve attendance history from the service
				var attendanceHistory =  _attendanceService.GetAll(WorkerId, SiteId, CurrentDate);

				// Return a successful response
				return Ok(new
				{
					success = true,
					data = attendanceHistory,
					errors = (object)null
				});
			}
			catch (Exception ex)
			{
				// Handle exceptions
				return StatusCode(500, new
				{
					success = false,
					data = (object)null,
					errors = new
					{
						message = "An error occurred while processing the request.",
						details = ex.Message
					}
				});
			}
		}



		[HttpPost("worker-selfie-attendance")]
		[Authorize]
		public async Task<IActionResult> WorkerSelfieAttendance([FromForm] WorkerAttendanceModel model, IFormFile SelfieImage)
		{
			try
			{
				// Validate the model
				if (!ModelState.IsValid)
				{
					var errors = ModelState
						   .Where(x => x.Value.Errors.Count > 0)
						   .ToDictionary(
							   kvp => kvp.Key,
							   kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
						   );

					var response = new
					{
						success = false,
						data = (object)null,
						errors
					};

					return BadRequest(response);

				}

				// Check if an image is provided
				if (SelfieImage == null || SelfieImage.Length == 0)
				{

					var errors = new
					{
						SelfieImage = new[] { "Selfie image is required!" }
					};
					return Unauthorized(new ApiResponseModel<object>(false, null, errors));
				}

				try
				{
					var uploadPath = "selfies"; // Define your folder path
					var maxFileSizeKb = 30; // 30KB size limit
					var quality = 20;
					var filePath = await _mms.SaveAndCompressImageAsync(SelfieImage, uploadPath, maxFileSizeKb, quality);

					// Set the selfie path in the model
					model.SelfieImage = filePath;

					// Call the service to manage attendance
					_attendanceService.ManageAttendance(model);


					//return Ok(new { FilePath = filePath, Message = "Image uploaded successfully." });
				}
				catch (Exception ex)
				{

					var errors = new
					{
						SelfieImage = new[] { ex.Message }
					};
					return Unauthorized(new ApiResponseModel<object>(false, null, errors));
				}

				return Ok(new ApiResponseModel<object>(true, new
				{
					message = "Attendance processed successfully."
				}, null));

			}
			catch (Exception ex)
			{
				var errors = new
				{
					SelfieImage = new[] { ex.Message }
				};
				return Unauthorized(new ApiResponseModel<object>(false, null, errors));
			}
		}



	}
}
