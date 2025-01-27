using GeneralTemplate.Filter;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Admin.Interfaces;

namespace GeneralTemplate.Areas.Admin.Controllers
{
    [Area("Admin")]
	[EmpAuthorizeFilter]
	public class DocumentController : Controller
    {
        
        private readonly DocumentService _documentService;
        public DocumentController()
        {
            _documentService = new DocumentService();
        }
        public IActionResult Index()
        {
            ViewBag.AllData = _documentService.GetAll();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DocumentModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Record added successfully!";
                    _documentService.Create(model);
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
        public ActionResult Edit(DocumentModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TempData["success"] = "Record updated successfully!";
                    _documentService.Update(model);
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
			//try
			//{
			//    if (id != null)
			//    {
			//        TempData["success"] = "Record deleted successfully!";
			//        _documentService.Delete(id);
			//        return RedirectToAction(nameof(Index));
			//    }
			//    else
			//    {
			//        TempData["error"] = "Invaild Id. Please try again.";

			//        return View();
			//    }

			//}
			//catch
			//{
			//    TempData["error"] = "Something went wrong!";
			//    return View("Index");
			//}
			_documentService.Delete(id);
			return RedirectToAction("Index");
		}
    }
}
