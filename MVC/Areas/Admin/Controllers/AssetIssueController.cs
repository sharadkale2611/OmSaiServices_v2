using GeneralTemplate.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Admin;
using OmSaiModels.Worker;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Admin.Interfaces;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Worker.Implimentation;
using System.Security.Claims;

namespace GeneralTemplate.Areas.Admin.Controllers
{
	[Area("Admin")]
    [EmpAuthorizeFilter]
    public class AssetIssueController : Controller
    {

        private readonly AssetsIssuesService _assetsIssuesService;
        private readonly AssetService _assetService;
        private readonly WorkerService _workerService;
        public AssetIssueController()
        {
            _assetsIssuesService = new AssetsIssuesService();
            _assetService = new AssetService();
            _workerService = new WorkerService();
        }


        public IActionResult Index()
        {
            ViewBag.assets = _assetService.GetAll(); // Get _asset list
            ViewBag.workers = _workerService.GetAll(); // Get workers list
            ViewBag.assetsIssues = _assetsIssuesService.GetAll(); // Get all _assetsIssues
            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AssetsIssuesModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Get the logged-in user's ID
                    //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    string userId = ViewBag.EmployeeId;
                    model.IssueBy = userId;
                    _assetsIssuesService.Create(model);
					TempData["success"] = "Record added successfully!";

					return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = "Validation errors occurred!";
                    return View("Index", model);
                }
            }
            catch(Exception ex)
            {
                //TempData["error"] = "Something went wrong!";
                TempData["error"] = ex.Message;
				//return View("Index", model);
				return RedirectToAction(nameof(Index));

			}
		}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AssetsIssuesModel model)
        {
            try
            {


				if (!ModelState.IsValid)
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
					return View(model);

				}

				TempData["success"] = "Record updated successfully!";
				_assetsIssuesService.Update(model);
				return RedirectToAction(nameof(Index));


			}
			catch
            {
                TempData["error"] = "Something went wrong!";
                return View("Index", model);
            }
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                if (id != 0)
                {
                    TempData["success"] = "Record deleted successfully!";
                    _assetsIssuesService.Delete(id);
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
    //public class AssetIssueController : Controller
    //{
    //	private readonly AssetsIssuesService  _assetsIssuesService;

    //	public AssetIssueController()
    //	{
    //		_assetsIssuesService = new AssetsIssuesService();
    //	}
    //	public IActionResult Index()
    //	{
    //		ViewBag.AllData = _assetsIssuesService.GetAll();
    //		return View();
    //	}
    //	[HttpPost]
    //	[ValidateAntiForgeryToken]
    //	public IActionResult Create(AssetsIssuesModel model)
    //	{
    //		try
    //		{
    //			if (ModelState.IsValid)
    //			{
    //				TempData["success"] = "Record added successfully!";
    //				_assetsIssuesService.Create(model);
    //			}
    //			else
    //			{
    //				var errorMessages = new List<string>();
    //				foreach (var state in ModelState)
    //				{
    //					foreach (var error in state.Value.Errors)
    //					{
    //						errorMessages.Add(error.ErrorMessage);
    //					}
    //				}
    //				TempData["errors"] = errorMessages;
    //			}

    //			return RedirectToAction(nameof(Index));

    //		}
    //		catch
    //		{
    //			TempData["error"] = "Something went wrong!";
    //			return View("Index", model);
    //		}
    //	}
    //	[HttpPost]
    //	[ValidateAntiForgeryToken]
    //	public ActionResult Edit(AssetsIssuesModel model)
    //	{
    //		try
    //		{
    //			if (ModelState.IsValid)
    //			{
    //				TempData["success"] = "Record updated successfully!";
    //				_assetsIssuesService.Update(model);
    //			}
    //			else
    //			{
    //				var errorMessages = new List<string>();
    //				foreach (var state in ModelState)
    //				{
    //					foreach (var error in state.Value.Errors)
    //					{
    //						errorMessages.Add(error.ErrorMessage);
    //					}
    //				}
    //				TempData["errors"] = errorMessages;
    //			}

    //			return RedirectToAction(nameof(Index));
    //		}
    //		catch
    //		{
    //			TempData["error"] = "Something went wrong!";
    //			return View("Index", model);
    //		}
    //	}

    //	[HttpPost]
    //	[ValidateAntiForgeryToken]
    //	public ActionResult Delete(int id)
    //	{
    //		//try
    //		//{
    //		//    if (id != null)
    //		//    {
    //		//        TempData["success"] = "Record deleted successfully!";
    //		//        _documentService.Delete(id);
    //		//        return RedirectToAction(nameof(Index));
    //		//    }
    //		//    else
    //		//    {
    //		//        TempData["error"] = "Invaild Id. Please try again.";

    //		//        return View();
    //		//    }

    //		//}
    //		//catch
    //		//{
    //		//    TempData["error"] = "Something went wrong!";
    //		//    return View("Index");
    //		//}
    //		_assetsIssuesService.Delete(id);
    //		return RedirectToAction("Index");
    //	}
    //}
}
