using System.Diagnostics;
using System.Security.Claims;
using GeneralTemplate.Models;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Implementations;

namespace GeneralTemplate.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
