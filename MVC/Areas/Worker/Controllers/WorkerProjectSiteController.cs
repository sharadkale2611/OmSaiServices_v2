using GeneralTemplate.Filter;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Worker;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Worker.Implimentation;

namespace GeneralTemplate.Areas.Worker.Controllers
{
    [Area("Worker")]
	[EmpAuthorizeFilter]
	public class WorkerProjectSiteController : Controller
    {
        private readonly WorkerProjectSiteService _workerProjectSiteService;
        private readonly WorkerService _workerService;
        private readonly SiteService _siteService;
        private readonly ProjectService _projectService;
        public WorkerProjectSiteController()
        {
            _workerProjectSiteService = new WorkerProjectSiteService();
            _siteService = new SiteService();
            _projectService = new ProjectService();
            _workerService = new WorkerService();
        }

        public ActionResult Index()
        {
            var allData = _workerProjectSiteService.GetAll();
            ViewBag.AllData = allData;
            ViewBag.Projects=_projectService.GetAll();
            ViewBag.Sites=_siteService.GetAll();
            ViewBag.Workers=_workerService.GetAll();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WorkerProjectSiteModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Record added successfully!";
                    _workerProjectSiteService.Create(model);
                }
                else
                {
                    var errorMessages = ModelState.Values
                                                  .SelectMany(v => v.Errors)
                                                  .Select(e => e.ErrorMessage)
                                                  .ToList();

                    TempData["errors"] = errorMessages;
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong! " + ex.Message;
                return View("Index", model);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkerProjectSiteModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Record updated successfully!";
                    _workerProjectSiteService.Update(model);
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

        public IActionResult Delete(int id)
        {
            _workerProjectSiteService.Delete(id);
            return RedirectToAction("Index");
        }


    }
}
