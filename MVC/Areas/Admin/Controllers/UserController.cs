using GeneralTemplate.Areas.Identity.Data;
using GeneralTemplate.Filter;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeneralTemplate.Areas.Admin.Controllers
{
	[Area("Admin")]
	[EmpAuthorizeFilter]
	public class UserController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}


		// List Users
		public async Task<IActionResult> Index()
		{
			var users = await _userManager.Users.ToListAsync(); // Get all users
			var userRoles = new List<UserRolesViewModel>();

			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user); // Get roles for each user
				userRoles.Add(new UserRolesViewModel
				{
					Id = user.Id,
					Email = user.Email,
					Roles = roles.ToList()  // Explicitly convert IList<string> to List<string>
				});
			}

			return View(userRoles); // Pass the userRoles to the view

		}

		// Create User (GET)
		public IActionResult Create()
		{
			// Retrieve all available roles
			var roles = _roleManager.Roles.Select(r => r.Name).ToList();

			// Initialize the ViewModel
			var model = new CreateUserViewModel
			{
				Roles = roles // Populate with all available roles
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateUserViewModel model)
		{
			if (!ModelState.IsValid)
			{
				// Reload roles in case of validation failure
				model.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
				return View(model);
			}

			// Create a new user object
			var user = new AppUser
			{
				UserName = model.Email,
				Email = model.Email
			};

			// Attempt to create the user
			var createResult = await _userManager.CreateAsync(user, model.Password);

			if (!createResult.Succeeded)
			{
				foreach (var error in createResult.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}

				// Reload roles in case of failure
				model.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
				return View(model);
			}

			// Assign the selected roles to the user
			if (model.SelectedRoles != null && model.SelectedRoles.Any())
			{
				var addRolesResult = await _userManager.AddToRolesAsync(user, model.SelectedRoles);

				if (!addRolesResult.Succeeded)
				{
					foreach (var error in addRolesResult.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}

					// Delete the user if role assignment fails
					await _userManager.DeleteAsync(user);

					// Reload roles in case of failure
					model.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
					return View(model);
				}
			}

			return RedirectToAction("Index");
		}


		// GET: User/Edit/{id}
		public async Task<IActionResult> Edit(string id)
		{
			// Retrieve the user
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			// Get all available roles and convert them to List<string>
			var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

			// Get the user's current roles
			var userRoles = await _userManager.GetRolesAsync(user);

			// Ensure the Roles and SelectedRoles are initialized and populated
			var model = new EditUserViewModel
			{
				Id = user.Id,
				Email = user.Email,
				Roles = roles,  // List of all roles
				SelectedRoles = userRoles.ToList()  // Convert to List<string>
			};

			return View(model);
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditUserViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByIdAsync(model.Id);
			if (user == null)
			{
				return NotFound();
			}

			// Update the email address
			user.Email = model.Email;
			var updateResult = await _userManager.UpdateAsync(user);

			if (!updateResult.Succeeded)
			{
				ModelState.AddModelError("", "Failed to update user.");
				return View(model);
			}

			// Remove the user's current roles
			var currentRoles = await _userManager.GetRolesAsync(user);
			var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

			if (!removeResult.Succeeded)
			{
				ModelState.AddModelError("", "Failed to remove roles.");
				return View(model);
			}

			// Add the selected roles
			var addResult = await _userManager.AddToRolesAsync(user, model.SelectedRoles);

			if (!addResult.Succeeded)
			{
				ModelState.AddModelError("", "Failed to assign roles.");
				return View(model);
			}

			return RedirectToAction("Index");
		}





		// Create User (POST)
		[HttpPost]
		public async Task<IActionResult> Create2(string email, string password)
		{
			var user = new AppUser { UserName = email, Email = email };
			var result = await _userManager.CreateAsync(user, password);

			if (result.Succeeded)
			{
				return RedirectToAction("Index");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View();
		}

		// Delete User
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user != null)
			{
				await _userManager.DeleteAsync(user);
			}

			return RedirectToAction("Index");
		}


		// Assign Role (GET)
		public async Task<IActionResult> AssignRole(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			var roles = _roleManager.Roles.ToList();
			var userRoles = await _userManager.GetRolesAsync(user);

			ViewBag.User = user;
			ViewBag.Roles = roles;
			ViewBag.UserRoles = userRoles;

			return View();
		}

		// Assign Role (POST)
		[HttpPost]
		public async Task<IActionResult> AssignRole(string userId, string role)
		{
			var user = await _userManager.FindByIdAsync(userId);

			if (!await _userManager.IsInRoleAsync(user, role))
			{
				await _userManager.AddToRoleAsync(user, role);
			}

			return RedirectToAction("Index");
		}




	}
}
