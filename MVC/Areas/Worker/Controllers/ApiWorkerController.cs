using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using OmSaiModels.Common;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Worker.Implimentation;


namespace GeneralTemplate.Areas.Worker.Controllers
{
	[Route("api/worker")]
	[ApiController]
	public class ApiWorkerController : ControllerBase
	{

		private readonly WorkerService _workerService;
		private readonly WorkerAddressService _workerAddressService;
		private readonly WorkerMobileNumbersService _workerMobileNumbersService;
		private readonly WorkerDocumentService _workerDocumentService;


		public ApiWorkerController()
		{
			_workerService = new WorkerService();
			_workerAddressService = new WorkerAddressService();
			_workerMobileNumbersService = new WorkerMobileNumbersService();
			_workerDocumentService = new WorkerDocumentService();

		}

		[HttpGet("profile/{id}")]
		[Authorize(AuthenticationSchemes = "Jwt")]
		public IActionResult GetProfile(int id)
		{
			var worker = _workerService.GetProfileById(id, null);
			if (worker == null)
			{
				var errors = new
				{
					WorkmanId = new[] { "Worker not found!" }
				};
				return Unauthorized(new ApiResponseModel<object>(false, null, errors));
			}
			var documents = _workerDocumentService.GetAll(id);
			var address = _workerAddressService.GetByWorkerId(id);

			return Ok(new ApiResponseModel<object>(true, new
			{
				worker,
				address,
				documents,
			}, null));

			return Ok(new { message = "This is a protected profile" });
		}

	}
}
