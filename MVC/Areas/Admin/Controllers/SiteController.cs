using GeneralTemplate.Filter;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Admin.Interfaces;

namespace GeneralTemplate.Areas.Admin.Controllers
{
    [Area("Admin")]
	[EmpAuthorizeFilter]
	public class SiteController : Controller
    {
		private readonly ProjectService _projectService;
		private readonly SiteService _siteService;
        public SiteController()
        {
			_projectService = new ProjectService();
			_siteService = new SiteService();
        }


        public ActionResult Index()
        {
            //var allData = _siteService.GetAll();

            //return View(allData);

            var allData = _siteService.GetAll();
            ViewBag.AllData = allData;
			ViewBag.Projects = _projectService.GetAll();

			return View();

        }

        public IActionResult Create()
        {
            SiteModel s = new SiteModel();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SiteModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Record added successfully!";
                    _siteService.Create(model);
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

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["error"] = "Something went wrong!";
                return View("Index", model);
            }
        }


        public IActionResult Edit(int id)
        {
            SiteModel s = _siteService.GetById(id);
            return View(s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SiteModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Record updated successfully!";
                    _siteService.Update(model);
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


        public IActionResult Delete(int id)
        {
            _siteService.Delete(id);
            return RedirectToAction("Index");
        }
    }

}
