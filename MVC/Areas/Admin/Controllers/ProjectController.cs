using OmSaiServices.Admin.Implementations;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Interfaces;
using GeneralTemplate.Filter;

namespace GeneralTemplate.Areas.Admin.Controllers
{
	[Area("Admin")]
	[EmpAuthorizeFilter]
	public class ProjectController : Controller
	{
		private readonly ProjectService _projectService;
		public ProjectController()
		{
			_projectService = new ProjectService();
		}
		public IActionResult Index()
		{
			ViewBag.AllData = _projectService.GetAll();
			return View();
		}

		public IActionResult Create()
		{
			ViewBag.AllData = _projectService.GetAll();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(ProjectModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{
					TempData["success"] = "Record added successfully!";
					_projectService.Create(model);
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

		public IActionResult Edit(int id)
		{
			ViewBag.AllData = _projectService.GetAll();
			var p = _projectService.GetById(id);
			return View(p);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(ProjectModel model)
		{

			try
			{
				if (ModelState.IsValid)
				{
					TempData["success"] = "Record updated successfully!";
					_projectService.Update(model);
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
			//_projectService.Update(model);
			//TempData["success"] = "Project Updated successfully!";
			//return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
			_projectService.Delete(id);

			TempData["success"] = "Project deleted successfully!";

			return RedirectToAction("Index");
		}
	}
}
