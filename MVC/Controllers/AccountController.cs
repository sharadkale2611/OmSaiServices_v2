using GeneralTemplate.Areas.Identity.Data;
using GeneralTemplate.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GeneralTemplate.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;

		public AccountController(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			return RedirectToAction("Login");
		}


		[EmpGuestFilter]
		public IActionResult Login()
		{		
			var employeeName = HttpContext.Session.GetString("id");
			if (!string.IsNullOrEmpty(employeeName))
			{
				return RedirectToAction("Index", "Home");
			}

			ViewBag.IsSignedIn = false;

			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Login(string email, string password)
		{
			// Find the user by email
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				ViewBag.Message = "Invalid Email ID or Password.";
				ViewData["error"] = "Invalid Email ID or Password.";
				return View();
			}

			// Verify the password
			var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
			if (!isPasswordValid)
			{
				ViewBag.Message = "Invalid Email ID or Password.";
				ViewData["error"] = "Invalid Email ID or Password.";

				return View();
			}

			// Get the roles assigned to the user
			var roles = await _userManager.GetRolesAsync(user);

			// Optionally, convert roles to an array if you need it
			var rolesArray = roles.ToArray();
			
			string employeeRoles = string.Join(",", rolesArray);
			string employeeRolesArray = JsonConvert.SerializeObject(rolesArray);

			// Save Worker Details in Session
				HttpContext.Session.SetString("id", user.Id);
				HttpContext.Session.SetString("username", user.UserName);
				HttpContext.Session.SetString("employeeRoles", employeeRoles);
				HttpContext.Session.SetString("employeeRolesArray", employeeRolesArray);

			ViewBag.Message = "Login Success";
				ViewData["success"] = "Login Success";
				return RedirectToAction("Index", "Home");

		}


		[HttpGet]
		[EmpAuthorizeFilter]
		public IActionResult Logout()
		{
			// Clear all session data
			HttpContext.Session.Clear();
			return RedirectToAction("Login","Account");
		}

	}
}
