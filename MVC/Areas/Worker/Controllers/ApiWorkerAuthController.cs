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
