using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GeneralTemplate.Filter
{
	public class EmpAuthorizeFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			// Exclude specific actions or controllers
	
			// Check session
			var userSession = context.HttpContext.Session.GetString("id");
			if (string.IsNullOrEmpty(userSession))
			{
				// Redirect to Login page
				context.Result = new RedirectToActionResult("Login", "Account", null);
			}

			base.OnActionExecuting(context);
		}
	}

	public class EmpGuestFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			// Exclude specific actions or controllers

			// Check session
			var userSession = context.HttpContext.Session.GetString("id");
			if (!string.IsNullOrEmpty(userSession))
			{
				// Redirect to Login page
				context.Result = new RedirectToActionResult("Index", "Home", null);
			}

			base.OnActionExecuting(context);
		}
	}
}
