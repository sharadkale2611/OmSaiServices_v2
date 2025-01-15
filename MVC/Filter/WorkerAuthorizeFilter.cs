using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GeneralTemplate.Filter
{
	public class WorkerAuthorizeFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			// Exclude specific actions or controllers
	
			// Check session
			var userSession = context.HttpContext.Session.GetString("WorkerId");
			if (string.IsNullOrEmpty(userSession))
			{
				// Redirect to Login page
				context.Result = new RedirectToActionResult("Login", "WorkerAuthentication", null);
			}

			base.OnActionExecuting(context);
		}
	}

	public class WorkerGuestFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			// Exclude specific actions or controllers

			// Check session
			var userSession = context.HttpContext.Session.GetString("WorkerId");
			if (!string.IsNullOrEmpty(userSession))
			{
				// Redirect to Login page
				context.Result = new RedirectToActionResult("Profile", "WorkerAuthentication", null);
			}

			base.OnActionExecuting(context);
		}
	}
}
