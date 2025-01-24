using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Admin;
using OmSaiModels.Worker;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Worker.Implimentation;
using System.Security.Claims;

namespace GeneralTemplate.Areas.Worker.Controllers
{
	[Area("Worker")]
	public class LeaveRequestController : Controller
	{
		private readonly LeaveRequestService _leaveRequestService;
		private readonly LeaveTypeService _leaveTypeservice;
		private readonly WorkerService _workerservice;


		public LeaveRequestController()
		{
			_leaveRequestService = new LeaveRequestService();
			_workerservice = new WorkerService();
			_leaveTypeservice = new LeaveTypeService();
		}
		public IActionResult Index()
		{
			// Check Session for WorkerName
			var workerName = HttpContext.Session.GetString("WorkerName");
			var workerId = HttpContext.Session.GetInt32("WorkerId");

			if (string.IsNullOrEmpty(workerName))
			{
				return RedirectToAction("Login", "WorkerAuthentication", new { area = "Worker" });
				//return RedirectToAction("ActionName", "ControllerName", new { area = "AreaName" });
			}

			ViewBag.AllData = _leaveRequestService.GetAll();
			ViewBag.LeaveType = _leaveTypeservice.GetAll();
			ViewBag.Worker = _workerservice.GetAll();
			ViewBag.IsSignedIn = true;

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(LeaveRequestModel model)
		{			
			try
			{
				if (ModelState.IsValid)
				{
	
					TempData["success"] = "Record added successfully!";
					_leaveRequestService.Create(model);
				}
				else
				{
					var errorMessages = new List<string>();
					foreach (var state in ModelState)
					{
						foreach (var error in state.Value.Errors)
						{
							errorMessages.Add(error.ErrorMessage);
						}
					}
					TempData["errors"] = errorMessages;
				}

				return RedirectToAction(nameof(Index));// nameof checks method compiletime to avoid errors
													   //return RedirectToAction(nameof(Index), model);    // If we pass data, it will append to url as a query string

			}
			catch
			{
				TempData["error"] = "Something went wrong!";
				return View("Index", model);
			}
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Approve(int LeaveRequestId, string Remark, string Status)   //LeaveRequestApproveModel model
		{
			// Get the logged-in user's ID
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			LeaveRequestApproveModel model = new LeaveRequestApproveModel
			{
				LeaveRequestId = LeaveRequestId,
				Remark = Remark,
				Status = Status,
				ApproverId = userId
			};

			_leaveRequestService.Approve(model);
			return RedirectToAction("Index");
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(LeaveRequestModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					
					TempData["success"] = "Record updated successfully!";
					_leaveRequestService.Update(model);
				}
				else
				{
					var errorMessages = new List<string>();
					foreach (var state in ModelState)
					{
						foreach (var error in state.Value.Errors)
						{
							errorMessages.Add(error.ErrorMessage);
						}
					}
					TempData["errors"] = errorMessages;
				}

				return RedirectToAction(nameof(Index));             // nameof checks method compiletime to avoid errors
			}
			catch
			{
				TempData["error"] = "Something went wrong!";
				return View("Index", model);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
			_leaveRequestService.Delete(id);
			TempData["success"] = "Record deleted successfully!";

			return RedirectToAction("Index");
		}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public IActionResult Approve(LeaveRequestModel model)
		//{
		//	ViewBag.AllData = _leaveRequestService.GetAll();
		//	ViewBag.LeaveType = _leaveTypeservice.GetAll();
		//	ViewBag.Worker = _workerservice.GetAll();

		//	_leaveRequestService.Create(model);


		//	return View();
		//}
	}
}
