using GeneralTemplate.Filter;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Worker;
using OmSaiServices.Worker.Implementations;

namespace GeneralTemplate.Areas.Worker.Controllers
{
    [Area("Worker")]
	[EmpAuthorizeFilter]
	public class WorkerMobileNumberController : Controller
    {
        private readonly WorkerMobileNumbersService _workerMobileNumbersService;
        public WorkerMobileNumberController()
        {
            _workerMobileNumbersService = new WorkerMobileNumbersService();
        }

        public ActionResult Index()
        {
            var allData = _workerMobileNumbersService.GetAll();
            ViewBag.allData = allData;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(WorkerMobileNumbersModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Project added successfully!";
                    _workerMobileNumbersService.Create(model);
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

            //_projectService.Create(model);
            //TempData["success"] = "Project Added successfully!";
            //return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(WorkerMobileNumbersModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Record updated successfully!";
                    _workerMobileNumbersService.Update(model);
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
            _workerMobileNumbersService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
