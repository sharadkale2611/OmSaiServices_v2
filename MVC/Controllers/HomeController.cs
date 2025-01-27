//using System.Diagnostics;
//using System.Security.Claims;
//using GeneralTemplate.Models;
//using Microsoft.AspNetCore.Mvc;
//using OmSaiModels.Admin;
//using OmSaiServices.Admin.Implementations;

//namespace GeneralTemplate.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }

//		public IActionResult Index()
//		{
//			// Get the logged-in user's ID
//			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//			// You can now use userId as needed
//			ViewBag.UserId = userId;
//            // Check Session for WorkerName
//            var workerName = HttpContext.Session.GetString("WorkerName");
//            var workerId = HttpContext.Session.GetInt32("WorkerId");
//            if (workerId != null)
//            {
//                ViewBag.IsSignedIn = true;

//            }
//            else
//            {
//				ViewBag.IsSignedIn = false;

//			}
//			ViewBag.WorkerName = workerName;
//            ViewBag.WorkerId = workerId;

//			return View();
//		}

//		public IActionResult SetService()
//        {
///*
//            var qService = new QualificationService();

//            var data = new QualificationModel
//            {
//				QualificationId = 1,
//				QualificationName = "MA",
//				Status = true
//            };

//            //qService.Create(data);
//            //qService.Delete(1);
//            //qService.Restore(1);
//            var a = qService.GetAll();

//			var b = qService.GetById(1);

//*/
//			return View();
//        }

//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}

using System.Diagnostics;
using System.Security.Claims;
using GeneralTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Worker.Implimentation;
using OmSaiServices.Worker.Interfaces;

namespace GeneralTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DashboardService _dashboardService;
        private readonly IWorkerService _workerService;

        // Inject the DashboardService into the controller constructor
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _dashboardService = new DashboardService();
            _workerService = new WorkerService();
        }

        public IActionResult Index()
        {
            // Get the logged-in user's ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // You can now use userId as needed
            ViewBag.UserId = userId;

            // Check Session for WorkerName
            var workerName = HttpContext.Session.GetString("WorkerName");
            var workerId = HttpContext.Session.GetInt32("WorkerId");
            if (workerId != null)
            {
                ViewBag.IsSignedIn = true;
            }
            else
            {
                ViewBag.IsSignedIn = false;
            }
            ViewBag.WorkerName = workerName;
            ViewBag.WorkerId = workerId;

            return View();
        }

        // Create an action for Dashboard
        //public IActionResult Dashboard()
        //{




        //    ViewBag.DashboardData = _dashboardService.GetDashboardData();
        //    return View();

        //}
        public IActionResult Dashboard(DateTime? inputDate)
        {
            
            

            // Fetch role and employee data
            var dashboardData = _dashboardService.GetDashboardData();

            // Fetch project and site data
            var projectSiteData = _dashboardService.GetProjectSiteData();

            // Fetch site and worker data
            var siteWorkerData = _dashboardService.GetSiteWorkerData();

            var workerAttendance = _dashboardService.GetWorkerAttendanceSummary(inputDate);


            // Pass the data to the view using ViewBag
            ViewBag.DashboardData = dashboardData;
            ViewBag.ProjectSiteData = projectSiteData;      
            ViewBag.SiteWorkerData = siteWorkerData;
            ViewBag.WorkerAttendance = workerAttendance;
            ViewBag.InputDate = inputDate.HasValue ? inputDate.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");


            return View();
        }

        public IActionResult SetService()
        {
            /*
                        var qService = new QualificationService();

                        var data = new QualificationModel
                        {
                            QualificationId = 1,
                            QualificationName = "MA",
                            Status = true
                        };

                        //qService.Create(data);
                        //qService.Delete(1);
                        //qService.Restore(1);
                        var a = qService.GetAll();

                        var b = qService.GetById(1);

            */
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
