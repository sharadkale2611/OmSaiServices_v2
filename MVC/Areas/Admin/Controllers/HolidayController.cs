using GeneralTemplate.Filter;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Implementations;


namespace GeneralTemplate.Areas.Admin.Controllers
{
    [Area("Admin")]
	[EmpAuthorizeFilter]
	public class HolidayController : Controller
    {
        private readonly HolidayService _holidayService;
        public HolidayController()
        {
            _holidayService = new HolidayService();

        }

        public ActionResult Index()
        {
            var allData = _holidayService.GetAll();
            ViewBag.AllData = allData;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HolidayModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Record added successfully!";
                    _holidayService.Create(model);
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
        public ActionResult Edit(HolidayModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Record updated successfully!";
                    _holidayService.Update(model);
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



   

        //public IActionResult Delete(int id)
        //{
        //    _holidayService.Delete(id);
        //    return RedirectToAction("Index");
        //}

    }
}
