using GeneralTemplate.Filter;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Admin.Interfaces;

namespace GeneralTemplate.Areas.Admin.Controllers
{
	[Area("Admin")]
	[EmpAuthorizeFilter]
	public class SiteShiftController : Controller
	{
		private readonly SiteShiftService _siteShiftService;
		private readonly SiteService _siteService;

		public SiteShiftController()
		{
			_siteShiftService = new SiteShiftService();
			_siteService = new SiteService();
		}


		public ActionResult Index()
		{
			var allData = _siteShiftService.GetAll();
			ViewBag.AllData = allData;
			var sites = _siteService.GetAll();
			ViewBag.Sites = sites;

			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(SiteShiftModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					TempData["success"] = "Record added successfully!";
					_siteShiftService.Create(model);
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
			SiteShiftModel s = _siteShiftService.GetById(id);
			return View(s);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(SiteShiftModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					TempData["success"] = "Record updated successfully!";
					_siteShiftService.Update(model);
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
			_siteShiftService.Delete(id);
			return RedirectToAction("Index");
		}



	}
}
