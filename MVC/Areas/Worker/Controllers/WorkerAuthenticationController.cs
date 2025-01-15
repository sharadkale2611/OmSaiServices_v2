using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OmSaiModels.Worker;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Worker.Implimentation;
using OmSaiServices.Worker.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace GeneralTemplate.Areas.Worker.Controllers
{
    [Area("Worker")]
    public class WorkerAuthenticationController : Controller
    {

		private readonly LeaveRequestService _leaveRequestService;
		private readonly LeaveTypeService _leaveTypeservice;

		private readonly WorkerService _workerService;
		private readonly WorkerAttendanceService _attendanceService;
		private readonly WorkerDocumentService _workerDocumentService;
		private readonly WorkerAddressService _workerAddressService;

		public WorkerAuthenticationController()
        {
			_leaveRequestService = new LeaveRequestService();
			_leaveTypeservice = new LeaveTypeService();
			_workerService = new WorkerService();
			_attendanceService = new WorkerAttendanceService();
			_workerDocumentService = new WorkerDocumentService();
			_workerAddressService = new WorkerAddressService();

		}
		public IActionResult Index()
        {
			var workerName = HttpContext.Session.GetString("WorkerName");
			if (!string.IsNullOrEmpty(workerName))
			{
				return RedirectToAction("Profile");
			}

			ViewBag.IsSignedIn = false;
			return View();
        }

        public IActionResult Login()
        {
			var workerName = HttpContext.Session.GetString("WorkerName");
			if (!string.IsNullOrEmpty(workerName))
			{
				return RedirectToAction("Profile");
			}
			ViewBag.IsSignedIn = false;
			return View();
        }

        [HttpPost]
        public IActionResult Login(string WorkmanId, string Password)
        {
            // Validate WorkmanId and Password
            var worker = _workerService
                .GetByWorkManId(WorkmanId)
                .FirstOrDefault();

            if (worker != null && worker.Password == Password)
            {
                // Save Worker Details in Session
                HttpContext.Session.SetInt32("WorkerId", worker.WorkerId.Value);
				HttpContext.Session.SetString("WorkerName", $"{worker.FirstName} {worker.LastName}");
				HttpContext.Session.SetString("WorkmanId", worker.WorkmanId);

				return RedirectToAction("Profile");
            }

            ViewBag.Message = "Invalid Workman ID or Password.";
            return View();
        }

		//public IActionResult ChangePassword()
		//{
		//	var workerId = HttpContext.Session.GetInt32("WorkerId");
		//	{
		//		return View();
		//	}

		//}

		//[HttpPost]
		////[ValidateAntiForgeryToken]
		//public IActionResult ChangePassword(WorkerChangePasswordModel model)
		//{
		//	var workerId = HttpContext.Session.GetInt32("WorkerId");

		//	if (!ModelState.IsValid)
		//	{

		//		var errorMessages = new List<string>();
		//		foreach (var state in ModelState)
		//		{
		//			foreach (var error in state.Value.Errors)
		//			{
		//				errorMessages.Add(error.ErrorMessage);
		//			}
		//		}
		//		TempData["errors"] = errorMessages;

		//	}

		//	var result = _workerService.ChangePassword(workerId.Value, model.oldPassword, model.NewPassword);

		//	if (!result)
		//	{
		//		ModelState.AddModelError(string.Empty, "Old password is incorrect.");

		//		var errorMessages = new List<string>();
		//		foreach (var state in ModelState)
		//		{
		//			foreach (var error in state.Value.Errors)
		//			{
		//				errorMessages.Add(error.ErrorMessage);
		//			}
		//		}
		//		TempData["errors"] = errorMessages;
		//		return View(model);
		//	}

		//	//HttpContext.Session.Clear();
		//	TempData["success"] = "Your password has been changed successfully. Please log in with your new password.";
		//	return View();
		//}




		public IActionResult LeaveRequest()
		{
			// Check Session for WorkerName
			var workerName = HttpContext.Session.GetString("WorkerName");
			var workerId = HttpContext.Session.GetInt32("WorkerId");
			if (string.IsNullOrEmpty(workerName))
			{
				return RedirectToAction("Login");
			}

			ViewBag.WorkerId = workerId;
			ViewBag.AllData = _leaveRequestService.GetAllByWorkerId(ViewBag.WorkerId);
			ViewBag.LeaveType = _leaveTypeservice.GetAll();
			ViewBag.Worker = _workerService.GetAll();
			ViewBag.IsSignedIn = true;
			//ViewBag.Role = _roleService.GetAll();

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult LeaveRequest(LeaveRequestModel model)
		{
			//var result =  _leaveRequestService.Create(model);			
			//return RedirectToAction(nameof(Index));
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

				return RedirectToAction(nameof(LeaveRequest));// nameof checks method compiletime to avoid errors

			}
			catch
			{
				TempData["error"] = "Something went wrong!";
				return View("LeaveRequest", model);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public IActionResult LeaveRequestEdit(LeaveRequestModel model)
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

				return RedirectToAction(nameof(LeaveRequest));             // nameof checks method compiletime to avoid errors
			}
			catch
			{
				TempData["error"] = "Something went wrong!";
				return View("LeaveRequest", model);
			}
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult LeaveRequestDelete(int id)
		{
			_leaveRequestService.Delete(id);
			TempData["success"] = "Record deleted successfully!";

			return RedirectToAction("Index");
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(LeaveRequestModel model)
		{
			//var result =  _leaveRequestService.Create(model);			
			//return RedirectToAction(nameof(Index));
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

				return RedirectToAction(nameof(LeaveRequest));// nameof checks method compiletime to avoid errors
													   //return RedirectToAction(nameof(Index), model);    // If we pass data, it will append to url as a query string

			}
			catch
			{
				TempData["error"] = "Something went wrong!";
				return View("Index", model);
			}
		}

		public IActionResult Attendance()
		{
			var WorkmanId = HttpContext.Session.GetString("WorkmanId");
			var workrman = WorkmanId;

			var attendanceHistory = _attendanceService.GetAll();
			var worker = _workerService.GetProfileById(null, workrman);

			if (worker == null || workrman == null)
			{
				return RedirectToAction(nameof(Error));
			}

			var workerName = HttpContext.Session.GetString("WorkerName");
			var workerId = HttpContext.Session.GetInt32("WorkerId");

			ViewBag.WorkerName = workerName;
			ViewBag.WorkerId = workerId;


			ViewBag.Worker = worker;
			ViewBag.IsSignedIn = true;
			return View();
		}


		public IActionResult Error(string id)
		{
			ViewBag.WorkerId = id;
			return View();
		}

		public IActionResult Profile()
        {
            // Check Session for WorkerName
            var workerName = HttpContext.Session.GetString("WorkerName");
			var workerId = HttpContext.Session.GetInt32("WorkerId");

			if (string.IsNullOrEmpty(workerName))
            {
                return RedirectToAction("Login");
            }

			ViewBag.WorkerName = workerName;
			ViewBag.WorkerId = workerId;

			ViewBag.AllData = _workerService.GetProfileById(workerId, null);
			if (ViewBag.AllData == null)
			{
				return RedirectToAction(nameof(Index));// nameof checks method compiletime to avoid errors
			}
			ViewBag.AttendanceHistory = _attendanceService.GetAll(workerId);
			ViewBag.WorkerDocuments = _workerDocumentService.GetAll(workerId);
			ViewBag.Addresses = _workerAddressService.GetByWorkerId(workerId??0);
			ViewBag.IsSignedIn = true;

			return View();
        }

		
	

		public IActionResult Logout()
        {
            // Clear all session data
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
