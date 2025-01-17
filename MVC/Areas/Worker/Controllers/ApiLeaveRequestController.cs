using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OmSaiModels.Common;
using OmSaiModels.Worker;
using OmSaiServices.Worker.Implimentation;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Worker.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
using OmSaiServices.Worker.Interfaces;

namespace GeneralTemplate.Areas.Worker.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApiLeaveRequestController : ControllerBase
	{
		private readonly LeaveRequestService _leaveRequestService;
		private readonly LeaveTypeService _leaveTypeservice;
		private readonly WorkerService _workerservice;

		public ApiLeaveRequestController()
		{
			_leaveRequestService = new LeaveRequestService();
			_workerservice = new WorkerService();
			_leaveTypeservice = new LeaveTypeService();
		}

		[HttpGet("get-all-leave-request")]
		//[Authorize(AuthenticationSchemes = "Jwt")]
		public IActionResult GetAllLeaveRequestId()
		{
			var request = _leaveRequestService.GetAll();
			if (request == null)
			{
				var errors = new
				{
					LeaveRequestId = new[] { "LeaveRequestId not found!" }
				};
				return BadRequest(new ApiResponseModel<object>(false, null, errors));
			}
			//var type = _leaveTypeservice.GetById(id);

			return Ok(new ApiResponseModel<object>(true, new
			{
				request,
				//type,
			}, null));

			return Ok(new { message = "This is a protected id" });
		}

		[HttpGet("get-by-worker-id")]
		//[Authorize(AuthenticationSchemes = "Jwt")]
		public IActionResult GetByWorkerId(int id)
		{
			var worker = _leaveRequestService.GetAllByWorkerId(id);
			if (worker == null)
			{
				var errors = new
				{
					WorkerId = new[] { "Worker not found!" }
				};
				return BadRequest(new ApiResponseModel<object>(false, null, errors));
			}
			//var documents = _workerDocumentService.GetAll(id);
			//var address = _workerAddressService.GetByWorkerId(id);

			return Ok(new ApiResponseModel<object>(true, new
			{
				worker
				//address,
				//documents,
			}, null));

			return Ok(new { message = "This is a protected id" });
		}

		[HttpGet("get-by-id")]
		//[Authorize(AuthenticationSchemes = "Jwt")]
		public IActionResult GetById(int id)
		{
			var worker = _leaveRequestService.GetById(id);
			if (worker == null)
			{
				var errors = new
				{
					WorkerId = new[] { "Worker not found!" }
				};
				return BadRequest(new ApiResponseModel<object>(false, null, errors));
			}
			//var documents = _workerDocumentService.GetAll(id);
			//var address = _workerAddressService.GetByWorkerId(id);

			return Ok(new ApiResponseModel<object>(true, new
			{
				worker
				//address,
				//documents,
			}, null));

			return Ok(new { message = "This is a protected id" });
		}
		[HttpPost]
		[Route("create-leave-request")]
		//[Authorize(AuthenticationSchemes = "Jwt")]
		public async Task<IActionResult> CreateLeaveRequest([FromBody] LeaveRequestModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var result = _leaveRequestService.Create(model);
					//return Ok(new
					//{
					//	success = true,
					//	message = "Record added successfully!"
					//});
					return Ok(new ApiResponseModel<object>(true, result, null));

				}
				else
				{									
					var errors = new
					{
						LeaveRequestId = new[] { "Leave Request not found!" }
					};
					return BadRequest(new ApiResponseModel<object>(false, null, errors));
					//return BadRequest(new
					//{
					//	success = false,
					//	errors = errorMessages
					//});
				}
			}
			catch (Exception ex)
			{
				var errors = new
				{
					Message = new[] { ex.Message },
				};
				return BadRequest(new ApiResponseModel<object>(false, null, errors));
			}
		}

		[HttpPost]
		[Route("update-leave-request")]
		//[Authorize(AuthenticationSchemes = "Jwt")]
		public async Task<IActionResult> UpdateLeaveRequest([FromBody] LeaveRequestModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{

					_leaveRequestService.Update(model);
					//return Ok(new
					//{
					//	success = true,
					//	message = "Record updated successfully!"
					//});
					return Ok(new ApiResponseModel<object>(true, new 
					{ 
						message= "Record updated successfully!" 
					}, null));
				}
				else
				{
					var errors = new
					{
						LeaveRequestId = new[] { "Leave Request not found!" }
					};
					return BadRequest(new ApiResponseModel<object>(false, null, errors));
					//return BadRequest(new
					//{
					//	success = false,
					//	errors = errorMessages
					//});
				}
			}
			catch (Exception ex)
			{
				var errors = new
				{
					Message = new[] { ex.Message },
				};
				return BadRequest(new ApiResponseModel<object>(false, null, errors));
			}
		}


	}	
}
