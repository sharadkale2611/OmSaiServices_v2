using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using OmSaiModels.Common;
using OmSaiModels.Worker;
using OmSaiServices.Worker.Implimentation;
using OmSaiServices.Worker.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeneralTemplate.Areas.Worker.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApiWorkerAuthController : ControllerBase
	{
		private readonly WorkerService _workerService;
		private readonly IConfiguration _configuration;


		public ApiWorkerAuthController(IConfiguration configuration)
		{
			_workerService = new WorkerService();
			_configuration = configuration;

		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] WorkerLoginModel model)
		{
			if (model == null || string.IsNullOrEmpty(model.LoginIdentifier) || string.IsNullOrEmpty(model.Password))
			{
				// Return failure with validation errors
				var errors = new
				{
					LoginIdentifier = string.IsNullOrEmpty(model?.LoginIdentifier) ? new[] { "The WorkmanId OR Mobile Number field is required." } : null,
					Password = string.IsNullOrEmpty(model?.Password) ? new[] { "The Password field is required." } : null
				};
				return BadRequest(new ApiResponseModel<object>(false, null, errors)); 
			}

			// Attempt to log in using the service
			var worker = await _workerService.Login(model.LoginIdentifier, model.Password);

			if (worker == null)
			{
				// Return failure for invalid credentials
				var errors = new
				{
					WorkmanId = new[] { "Invalid WorkmanId OR Mobile-Number OR Password." }
				};
				return Unauthorized(new ApiResponseModel<object>(false, null, errors));
			}

			var token = GenerateToken(worker.WorkerId ?? 0, worker.WorkmanId, worker.FirstName, worker.LastName);
			// Return success with worker details
			return Ok(new ApiResponseModel<object>(true, new
			{
				worker.WorkerId,
				worker.WorkmanId,
				worker.ProfileImage,
				worker.FirstName,
				worker.MiddleName,
				worker.LastName,
				Token = token // Add the token to the response

			}, null));



		}


		//[HttpPost]
		//[Route("api/Worker/ChangePassword")]
		//public async Task<IActionResult> ChangePassword(int? workerId, string oldPassword, string newPassword, string confirmPassword)
		//{


		//	if (!workerId.HasValue || workerId.Value <= 0 || string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
		//	{
		//		 //Return failure with validation errors
		//		var errors = new
		//		{
		//			WorkerId = new[] { "The WorkerId field is required and must be a valid positive number." },
		//			OldPassword = string.IsNullOrEmpty(oldPassword) ? new[] { "The old password field is required." } : null,
		//			NewPassword = string.IsNullOrEmpty(newPassword) ? new[] { "The new password field is required." } : null,
		//			ConfirmPassword = string.IsNullOrEmpty(confirmPassword) ? new[] { "The confirm password field is required." } : null,
		//		};
		//		return BadRequest(new ApiResponseModel<object>(false, null, errors));
		//	}

		//	try
		//	{
		//		 //Validate the input data
		//		if (workerId == 0 || string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
		//		{
		//			return BadRequest(new
		//			{
		//				Success = false,
		//				Message = "Invalid request data."
		//			});
		//		}

		//		// Attempt to change the password using the service method
		//		var result = _workerService.ChangePassword(WorkerId, oldPassword, newPassword);

		//		return Ok(new
		//		{
		//			Success = true,
		//			Message = "Password changed successfully.",
		//			Data = result
		//		});
		//	}
		//	catch (Exception ex)
		//	{
		//		return BadRequest(new
		//		{
		//			Success = false,
		//			Message = ex.Message
		//		});
		//	}
		//}

		[HttpPost]
		[Route("ChangePassword")]
		public async Task<IActionResult> ChangePassword(WorkerChangePasswordModel model)
		{
			try
			{
				// Validate the input data
				if (model.WorkerId == 0 || string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword))
				{
					var errors = new
					{
						WorkerId = new[] { "The WorkerId field is required and must be a valid positive number." },
						OldPassword = string.IsNullOrEmpty(model.OldPassword) ? new[] { "The old password field is required." } : null,
						NewPassword = string.IsNullOrEmpty(model.NewPassword) ? new[] { "The new password field is required." } : null,
						ConfirmPassword = string.IsNullOrEmpty(model.ConfirmPassword) ? new[] { "The confirm password field is required." } : null,
					};
					return BadRequest(new ApiResponseModel<object>(false, null, errors));	
				}

				if (model.NewPassword != model.ConfirmPassword)
				{
					var errors = new
					{
						WorkmanId = new[] { "Worker not found!" }
					};
					return BadRequest(new ApiResponseModel<object>(false, null, errors));
				}

				// Attempt to change the password using the service method
				var result = _workerService.ChangePassword(model.WorkerId, model.OldPassword, model.NewPassword);

				return Ok(new ApiResponseModel<object>(true, result, null));

				//return Ok(new
				//{
				//	Success = true,
				//	Message = "Password changed successfully.",
				//	Data = result
				//});
			}
			catch (Exception ex)
			{
				var errors = new
				{					
					Message = new[] {ex.Message},					
				};
				return BadRequest(new ApiResponseModel<object>(false, null, errors));	
			}
		}

		[HttpGet("test")]
		public IActionResult GetTestInfo()
		{
			return Ok(new { message = "This is Test API" });
		}

		public string GenerateToken(int workerId , string workmanId, string firstName, string lastName)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, workmanId),
				new Claim("workerId",  workerId.ToString()),
				new Claim("firstName", firstName),
				new Claim("lastName", lastName),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtSettings:Issuer"],
				audience: _configuration["JwtSettings:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryMinutes"])),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}


	}
}
