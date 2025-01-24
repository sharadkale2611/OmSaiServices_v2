using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace GeneralTemplate.Filter
{

	public class SessionInfoFilter : IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext context)
		{
			var controller = context.Controller as Controller;

			if (controller != null)
			{
				var employeeId = context.HttpContext.Session.GetString("id");
				var username = context.HttpContext.Session.GetString("username");
				var employeeRoles = context.HttpContext.Session.GetString("employeeRoles");
				var employeeRolesArray = context.HttpContext.Session.GetString("employeeRolesArray");

				var workerName = context.HttpContext.Session.GetString("WorkerName");
				var workerId = context.HttpContext.Session.GetInt32("WorkerId");

				controller.ViewBag.IsSignedIn = workerId != null ? true : false;
				controller.ViewBag.IsSignedInAdmin = employeeId != null ? true : false;

				controller.ViewBag.UserId = employeeId;
				controller.ViewBag.EmployeeId = employeeId;
				controller.ViewBag.EmployeeName = username;
				controller.ViewBag.employeeRoles = employeeRoles;
				controller.ViewBag.employeeRolesArray = employeeRolesArray;
				

				controller.ViewBag.WorkerName = workerName;
				controller.ViewBag.WorkerId = workerId;
			}
		}

		public void OnActionExecuted(ActionExecutedContext context)
		{
			// Optionally, do something after the action executes
		}
	}


}
