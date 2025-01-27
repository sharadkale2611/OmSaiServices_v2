using GeneralTemplate.Areas.Identity.Data;
using GeneralTemplate.Filter;
using GeneralTemplate.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Admin.Interfaces;

namespace GeneralTemplate.Areas.Admin.Controllers
{
	[Area("Admin")]
	[EmpAuthorizeFilter]
	public class AssetController : Controller
	{

		private readonly AssetService _assetService;
		public AssetController()
		{ 
			_assetService = new AssetService();
			
		}
		public IActionResult Index()
		{
			ViewBag.AllData = _assetService.GetAll();
			return View();
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(AssetModel model)
		{
			//var result =  _assetService.Create(model);			
			//return RedirectToAction(nameof(Index));
			try
			{
				if (ModelState.IsValid)
				{
					TempData["success"] = "Record added successfully!";
					_assetService.Create(model);
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

		public IActionResult Edit(AssetModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					TempData["success"] = "Record updated successfully!";
					_assetService.Update(model);
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

		[HttpPost]
		[ValidateAntiForgeryToken]

		public IActionResult Delete(int id)
		{
		
			_assetService.Delete(id);
			TempData["success"] = "Record deleted successfully!";

			return RedirectToAction("Index");
		}
	}
}
