using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Worker;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Worker.Implimentation;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace GeneralTemplate.Areas.Worker.Controllers
{
    [Area("Worker")]
    public class WorkerAuthenticationController : Controller
    {
        private readonly WorkerService _workerService;
		private readonly WorkerAttendanceService _attendanceService;
		private readonly WorkerDocumentService _workerDocumentService;
		private readonly WorkerAddressService _workerAddressService;



		public WorkerAuthenticationController()
        {
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


			return View();
        }

        public IActionResult Login()
        {
			var workerName = HttpContext.Session.GetString("WorkerName");
			if (!string.IsNullOrEmpty(workerName))
			{
				return RedirectToAction("Profile");
			}
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

			ViewBag.Worker = worker;

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


			// Convert workerId to int?
			//int? workerId = null;
			//if (!string.IsNullOrEmpty(workerIdString) && int.TryParse(workerIdString, out var parsedWorkerId))
			//{
			//	workerId = parsedWorkerId;
			//}

			ViewBag.WorkerName = workerName;
			ViewBag.AllData = _workerService.GetProfileById(workerId, null);
			if (ViewBag.AllData == null)
			{
				return RedirectToAction(nameof(Index));// nameof checks method compiletime to avoid errors
			}
			ViewBag.AttendanceHistory = _attendanceService.GetAll(workerId);
			ViewBag.WorkerDocuments = _workerDocumentService.GetAll(workerId);
			ViewBag.Addresses = _workerAddressService.GetByWorkerId(workerId??0);

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
