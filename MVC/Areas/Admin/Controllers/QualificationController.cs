using GeneralTemplate.Filter;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Admin.Interfaces;

namespace GeneralTemplate.Areas.Admin.Controllers
{
    [Area("Admin")]
	[EmpAuthorizeFilter]
	public class QualificationController : Controller
    {
       
            private readonly QualificationService _qualificationService;
            public QualificationController()
            {

			_qualificationService = new QualificationService();

            }

            public ActionResult Index()
            {
                var AllData = _qualificationService.GetAll();
                ViewBag.AllData = AllData;
                
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create(QualificationModel model)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        TempData["success"] = "Record added successfully!";
                        _qualificationService.Create(model);
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
            public ActionResult Edit(QualificationModel model)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        TempData["success"] = "Record updated successfully!";
                        _qualificationService.Update(model);
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
                _qualificationService.Delete(id);
                return RedirectToAction("Index");
            }

        }

    }

