using GeneralTemplate.Filter;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Implementations;

namespace GeneralTemplate.Areas.Admin.Controllers
{
	[Area("Admin")]
	[EmpAuthorizeFilter]
	public class LeaveTypeController : Controller
	{
		private readonly LeaveTypeService _leaveTypeService;
		public LeaveTypeController()
		{
			_leaveTypeService = new LeaveTypeService();

		}
		public IActionResult Index()
		{
			ViewBag.AllData = _leaveTypeService.GetAll();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(LeaveTypeModel model)
		{
			//var result =  _leaveTypeService.Create(model);			
			//return RedirectToAction(nameof(Index));
			try
			{
				if (ModelState.IsValid)
				{
					TempData["success"] = "Record added successfully!";
					_leaveTypeService.Create(model);
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

		public IActionResult Edit(LeaveTypeModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					TempData["success"] = "Record updated successfully!";
					_leaveTypeService.Update(model);
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

			_leaveTypeService.Delete(id);
			TempData["success"] = "Record deleted successfully!";

			return RedirectToAction("Index");
		}
	}
}
