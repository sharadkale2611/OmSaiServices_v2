using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using OmSaiModels.Common;
using OmSaiModels.Worker;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Worker.Implimentation;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;


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

			
		}

        [HttpPost("upload-profile-image")]
        public IActionResult UploadProfileImage([FromForm] IFormFile? file, [FromForm] int workerId, [FromForm] string? profileImage = "")
        {
            string uploadPath = "media/profile_images";

            var model = new ProfileImageModel
            {
                WorkerId = workerId,
                ProfileImage = profileImage
            };

            try
            {
                var worker = _workerService.GetById(workerId); 
                if (worker == null)
                {
                    var errors = new
                    {
                        WorkerId = new[] { "Worker not found!" },
                        File = file == null ? new[] { "No file uploaded!" } : null,
                        ProfileImage = string.IsNullOrEmpty(profileImage) ? new[] { "Profile image path is missing!" } : null
                    };

                    return BadRequest(new ApiResponseModel<object>(false, null, errors));
                }

                if (file != null && file.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", uploadPath);

                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var completeFilePath = Path.Combine(filePath, uniqueFileName);

                    using (var stream = new FileStream(completeFilePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    model.ProfileImage = $"{uploadPath}/{uniqueFileName}";

                    _workerService.UploadProfileImage(workerId, model.ProfileImage);

                    return Ok(new ApiResponseModel<object>(true, new
                    {
                       
                        message = "Profile image uploaded successfully!",
                        filePath = $"{uploadPath}/{uniqueFileName}"
                    }, null));
                }
                else
                {
                    model.ProfileImage = profileImage;

                    _workerService.UploadProfileImage(workerId, model.ProfileImage);

                    return Ok(new ApiResponseModel<object>(true, new
                    {
                        
                        message = "Profile image updated successfully!",
                        filePath = profileImage
                    },null) );


                   
                }
               
            }
            catch (Exception ex)
            {
                var errors = new
                {

                    message = $"An error occurred: {ex.Message}"
                    
                };

                return BadRequest(new ApiResponseModel<object>(false, null, errors));

            }
        }


    }
}
