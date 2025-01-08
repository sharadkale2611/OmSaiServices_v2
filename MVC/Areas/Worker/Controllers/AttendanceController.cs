using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Worker;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Worker.Implimentation;
using LmsServices.Common;

namespace GeneralTemplate.Areas.Worker.Controllers
{
	[Area("Worker")]
	public class AttendanceController : Controller
	{
		private readonly WorkerService _workerService;
		private readonly WorkerAttendanceService _attendanceService;
		private readonly MultiMediaService _mms;
		public AttendanceController()
		{
			_workerService = new WorkerService();
			_attendanceService = new WorkerAttendanceService();
			_mms = new MultiMediaService();
		}

		public IActionResult Index()
		{

			ViewBag.AttendanceHistory = _attendanceService.GetAll();

			return View();
		}



		public IActionResult Create(string wokrman)
		{

			var attendanceHistory = _attendanceService.GetAll();


			var worker = _workerService.GetProfileById(null, wokrman);

			if (worker == null || wokrman == null)
			{
				return RedirectToAction(nameof(Error));
			}

			ViewBag.Worker = worker;

			return View();
		}


		[HttpPost]
		[Route("api/WorkerAttendance")]
		public async Task<IActionResult> WorkerAttendance([FromForm] WorkerAttendanceModel model, IFormFile SelfieImage)
		{

			try
			{
				// Validate the model
				if (!ModelState.IsValid)
				{
					return Json(new
					{
						success = false,
						message = "Invalid data provided.",
						errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
					});
				}

				// Check if an image is provided
				if (SelfieImage == null || SelfieImage.Length == 0)
				{
					return Json(new
					{
						success = false,
						message = "Selfie image is required. too"
					});
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

					return Json(new
					{
						success = false,
						message = "An error occurred while processing attendance.",
						error = ex.Message
					});

				}


				// Return a success response
				return Json(new
				{
					success = true,
					message = "Attendance processed successfully.",
				});
			}
			catch (Exception ex)
			{
				// Log the exception if necessary

				// Return a failure response
				return Json(new
				{
					success = false,
					message = "An error occurred while processing attendance.",
					error = ex.Message
				});
			}
		}





		public IActionResult Error(string id)
		{
			ViewBag.WorkerId = id;

			return View();
		}


	}
}
