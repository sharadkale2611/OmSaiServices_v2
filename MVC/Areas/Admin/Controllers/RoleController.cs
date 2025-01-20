using GeneralTemplate.Filter;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Implementations;



namespace GeneralTemplate.Areas.Admin.Controllers
{
	[Area("Admin")]
	[EmpAuthorizeFilter]
	public class RoleController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;

		public RoleController(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}

		public IActionResult Index()
		{
			var roles = _roleManager.Roles.ToList();
			ViewBag.AllData = roles;
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(IdentityRole model)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(model.Name))
				{
					ModelState.AddModelError(string.Empty, "Role name cannot be empty.");
				}

				if (ModelState.IsValid)
				{
					TempData["success"] = "Record added successfully!";

					var result = await _roleManager.CreateAsync(new IdentityRole(model.Name));
					if (result.Succeeded)
					{
						return RedirectToAction(nameof(Index));
					}
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

			}
			catch
			{
				TempData["error"] = "Something went wrong!";
				return View("Index", model);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(IdentityRole model)
		{
			try
			{
				var role = await _roleManager.FindByIdAsync(model.Id);
				if (role == null) return NotFound();

				if (string.IsNullOrWhiteSpace(model.Name))
				{
					ModelState.AddModelError(string.Empty, "Role name cannot be empty.");
				}


				if (ModelState.IsValid)
				{
					TempData["success"] = "Record updated successfully!";
					role.Name = model.Name;
					var result = await _roleManager.UpdateAsync(role);

					if (result.Succeeded)
					{
						return RedirectToAction(nameof(Index));
					}

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

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role == null) return NotFound();

			var result = await _roleManager.DeleteAsync(role);
			if (result.Succeeded)
			{
				return RedirectToAction(nameof(Index));
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return View(role);
		}

	}
}
