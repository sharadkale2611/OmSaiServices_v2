using Microsoft.AspNetCore.Mvc;

using OmSaiModels.Worker;

using OmSaiServices.Worker.Implementations;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Worker.Interfaces;
using OmSaiServices.Worker.Implimentation;
using GeneralTemplate.Filter;

namespace GeneralTemplate.Areas.Worker.Controllers
{
    [Area("Worker")]
	[EmpAuthorizeFilter]
	public class WorkerQualificationController : Controller
    {

        private readonly WorkerQualificationService _workerQualificationService;
        private readonly QualificationService _qualificationService;
        private readonly WorkerService _workerService;
        public WorkerQualificationController()
        {
            _workerQualificationService = new WorkerQualificationService();
            _qualificationService = new QualificationService();
            _workerService = new WorkerService();
        }

        
        public IActionResult Index()
        {
            ViewBag.qualifications = _qualificationService.GetAll(); // Get qualifications list
            ViewBag.workers = _workerService.GetAll(); // Get workers list
            ViewBag.workerQualifications = _workerQualificationService.GetAll(); // Get all qualifications
            return View();
                
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(WorkerQualificationModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Record added successfully!";
                    _workerQualificationService.Create(model);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["errors"] = "Validation errors occurred!";
                    return View("Index", model);
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong!";
                return View("Index", model);
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkerQualificationModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Record updated successfully!";
                    _workerQualificationService.Update(model);
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

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                if (id != 0)
                {
                    TempData["success"] = "Record deleted successfully!";
                    _workerQualificationService.Delete(id);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Invalid Id. Please try again.";
                    return View();
                }
            }
            catch
            {
                TempData["error"] = "Something went wrong!";
                return View("Index");
            }
        }

        
    }
}
