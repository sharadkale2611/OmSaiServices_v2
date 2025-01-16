﻿using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Admin;
using OmSaiModels.Worker;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Admin.Interfaces;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Worker.Implimentation;
using OmSaiServices.Worker.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GeneralTemplate.Areas.Worker.Controllers
{
	[Area("Worker")]
	public class WorkerController : Controller
	{
		private readonly WorkerService _workerService;
		private readonly DepartmentService _departmentService;
		private readonly ProjectService _projectService;
		private readonly SiteService _siteService;
		private readonly WorkerProjectSiteService _workerProjectSiteService;
		private readonly QualificationService _qualificationService;
		private readonly WorkerQualificationService _workerQualificationService;
		private readonly WorkerAttendanceService _attendanceService;
		private readonly WorkerDocumentService _workerDocumentService;
		private readonly WorkerMobileNumbersService _workerMobileNumbersService;
		private readonly WorkerAddressService _workerAddressService;

		public WorkerController()
		{
			_workerService = new WorkerService();
			_workerQualificationService = new WorkerQualificationService();
			_departmentService = new DepartmentService();
			_projectService = new ProjectService();
			_siteService = new SiteService();
			_qualificationService = new QualificationService();
			_workerProjectSiteService = new WorkerProjectSiteService();
			_workerMobileNumbersService = new WorkerMobileNumbersService();
			_attendanceService = new WorkerAttendanceService();
			_workerDocumentService = new WorkerDocumentService();
			_workerAddressService = new WorkerAddressService();

		}
		public IActionResult Index()
		{
			ViewBag.AllData = _workerService.GetAll();
			ViewBag.Department=_departmentService.GetAll();
			ViewBag.Sites= _siteService.GetAll();

			ViewBag.Projects = _projectService.GetAll();
			ViewBag.Workers = _workerService.GetAll();
			return View();
		}



        //public IActionResult Profile(int id)
        //{
        //	ViewBag.AllData = _workerService.GetProfileById(id, null);
        //	if (ViewBag.AllData == null)
        //	{
        //		return RedirectToAction(nameof(Index));// nameof checks method compiletime to avoid errors
        //	}
        //	ViewBag.AttendanceHistory = _attendanceService.GetAll(id);
        //	ViewBag.WorkerDocuments = _workerDocumentService.GetAll(id);
        //	ViewBag.Addresses = _workerAddressService.GetByWorkerId(id);

        //	return View();

        //}
        public IActionResult Profile(int id, string attendanceMonth = null)
        {
            var workerProfile = _workerService.GetProfileById(id, null);

            if (workerProfile == null)
            {
                return RedirectToAction(nameof(Index)); 
            }

            
            attendanceMonth ??= DateTime.Now.ToString("MMMM");

           
            var attendanceSummary = _attendanceService.GetAttendanceSummary(id, attendanceMonth);

            
            var attendanceHistory = _attendanceService.GetAll(id);
            var workerDocuments = _workerDocumentService.GetAll(id);
            var workerAddresses = _workerAddressService.GetByWorkerId(id);

            // Check if the request is AJAX (XHR Request)
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                
                return Json(new
                {
                    presentCount = attendanceSummary.PresentCount,
                    absentCount = attendanceSummary.AbsentCount,
                    leaveCount = attendanceSummary.LeaveCount,
                    missCount = attendanceSummary.MissCount
                });
            }

           
            ViewBag.AllData = workerProfile;
            ViewBag.AttendanceMonth = attendanceMonth;
            ViewBag.PresentCount = attendanceSummary.PresentCount;
            ViewBag.AbsentCount = attendanceSummary.AbsentCount;
            ViewBag.LeaveCount = attendanceSummary.LeaveCount;
            ViewBag.MissCount = attendanceSummary.MissCount;
            ViewBag.AttendanceHistory = attendanceHistory;
            ViewBag.WorkerDocuments = workerDocuments;
            ViewBag.Addresses = workerAddresses;

            return View();
        }



        public IActionResult ProfilePrint(int id)
		{
			ViewBag.AllData = _workerService.GetProfileById(id, null);
			if (ViewBag.AllData == null)
			{
				return RedirectToAction(nameof(Index));// nameof checks method compiletime to avoid errors
			}
			ViewBag.AttendanceHistory = _attendanceService.GetAll(id);
			ViewBag.WorkerDocuments = _workerDocumentService.GetAll(id);
			ViewBag.Addresses = _workerAddressService.GetByWorkerId(id);

			return View();
		}

		[HttpPost]
		[Route("api/Worker/ChangePassword")]
		public IActionResult ChangePassword(int workerId, string oldPassword, string newPassword)
		{
			try
			{
				// Validate the input data
				if (workerId == 0 || string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
				{
					return BadRequest(new
					{
						Success = false,
						Message = "Invalid request data."
					});
				}

				// Attempt to change the password using the service method
				var result = _workerService.ChangePassword(workerId, oldPassword, newPassword);

				return Ok(new
				{
					Success = true,
					Message = "Password changed successfully.",
					Data = result
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new
				{
					Success = false,
					Message = ex.Message
				});
			}
		}
        //[HttpPost]
        //[Route("api/Worker/UploadProfileImage")]
        //public IActionResult UploadProfileImage([FromForm] IFormFile file, [FromForm] int workerId, [FromForm] string? profileImage = "")
        //{
        //    if (file == null || workerId <= 0)
        //    {
        //        return BadRequest(new { success = false, message = "Invalid input parameters." });
        //    }

        //    var uploadPath = Path.Combine("wwwroot", "media/profile_images");

        //    if (!Directory.Exists(uploadPath))
        //    {
        //        Directory.CreateDirectory(uploadPath);
        //    }

        //    var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        //    var filePath = Path.Combine(uploadPath, uniqueFileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        file.CopyTo(stream);
        //    }

        //    try
        //    {
        //        var profileImagePath = $"/media/profile_images/{uniqueFileName}";
        //        var profileImageModel = _workerService.UploadProfileImage(workerId, profileImagePath);

        //        return Ok(new
        //        {
        //            success = true,
        //            message = "Profile image uploaded successfully.",
        //            profileImage = profileImageModel.ProfileImage
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { success = false, message = $"Error: {ex.Message}" });
        //    }
        //}


        [HttpPost]
        [Route("api/Worker/UploadProfileImage")]
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

                    return Ok(new
                    {
                        success = true,
                        message = "Profile image uploaded successfully!",
                        filePath = $"{uploadPath}/{uniqueFileName}"
                    });
                }
                else
                {
                    model.ProfileImage = profileImage;
                    
                    _workerService.UploadProfileImage(workerId, model.ProfileImage);

                    return Ok(new
                    {
                        success = true,
                        message = "Profile image updated successfully!",
                        filePath = profileImage
                    });
                }

                return BadRequest(new { success = false, message = "No file uploaded." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }


        [HttpPost]
		[Route("api/Worker/UploadDocument")]
		public IActionResult UploadDocument([FromForm] IFormFile? file, [FromForm] int WorkerId, int DocumentId, [FromForm] string? documentNumber = "", string? documentImage = null)
		{
			string uploadPath = "media/documents";

			var model = new WorkerDocumentModel
			{
				DocumentId = DocumentId,
				DocumentNumber = documentNumber,
				WorkerId = WorkerId

			};
			try
			{
				if (file != null && file.Length > 0)
				{

					var documentPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", uploadPath);


					if (!Directory.Exists(documentPath))
						Directory.CreateDirectory(documentPath);


					var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
					var filePath = Path.Combine(documentPath, uniqueFileName);


					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						file.CopyTo(stream);
					}
					model.DocumentImage = $"{uploadPath}/{uniqueFileName}";
					_workerDocumentService.Update(model);

					return Ok(new
					{
						success = true,
						message = "Document uploaded successfully!",
						filePath = $"{uploadPath}/{uniqueFileName}"
					});
				}
				else
				{
					model.DocumentImage = documentImage;

					_workerDocumentService.Update(model);
					return Ok(new
					{
						success = true,
						message = "Document updated successfully!",
						filePath = documentImage
					});

				}

				return BadRequest(new { success = false, message = "No file uploaded." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, message = $"An error occurred: {ex.Message}" });

			}
		}

		public IActionResult Create()
		{
			ViewBag.AllData = _workerService.GetAll();
			ViewBag.Department = _departmentService.GetAll();
			ViewBag.Sites = _siteService.GetAll();

			ViewBag.Projects = _projectService.GetAll();
			ViewBag.Workers = _workerService.GetAll();
			ViewBag.Qualifications = _qualificationService.GetAll();


			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(WorkerModel model, int ProjectId, int SiteId, int QualificationId, string MobileNumber, bool Status, string? MobileNumber2="", string? Address1="", string? Address2="")
		{
			try
			{
				model.WorkmanId = "";

				if (ModelState.IsValid)
				{
					TempData["success"] = "Record added successfully!";

					var lastWorkerId = _workerService.Create(model);
					
					WorkerProjectSiteModel workerProjectSiteModel = new WorkerProjectSiteModel
					{
						WorkerId = lastWorkerId,
						SiteId = SiteId,
						ProjectId = ProjectId,
						Status = true
					};
					_workerProjectSiteService.Create(workerProjectSiteModel);


					WorkerQualificationModel workerQualificationModel = new WorkerQualificationModel
					{
						WorkerId = lastWorkerId,
						QualificationId = QualificationId
					};
					_workerQualificationService.Create(workerQualificationModel);

					WorkerMobileNumbersModel workerMobileNumbersModel = new WorkerMobileNumbersModel
					{
						WorkerId = lastWorkerId,
						MobileNumber = MobileNumber
					};
					_workerMobileNumbersService.Create(workerMobileNumbersModel);

					if(MobileNumber2 == "")
					{
						MobileNumber2 = MobileNumber;
					}
					WorkerMobileNumbersModel workerMobileNumbersModel_2 = new WorkerMobileNumbersModel
					{
						WorkerId = lastWorkerId,
						MobileNumber = MobileNumber2
					};
					_workerMobileNumbersService.Create(workerMobileNumbersModel_2);

					if (Address1 != "" && Address1 != null)
					{
						WorkerAddressModel workerAddressModel_1 = new WorkerAddressModel
						{
							WorkerId = lastWorkerId,
							AddressType	= "Permanent",
							Address = Address1
						};
						_workerAddressService.Create(workerAddressModel_1);

					}

					if (Address2 != "" && Address2 != null)
					{
						WorkerAddressModel workerAddressModel_2 = new WorkerAddressModel
						{
							WorkerId = lastWorkerId,
							AddressType = "Current",
							Address = Address2
						};
						_workerAddressService.Create(workerAddressModel_2);
					}

					var documentIds = new List<int> { 8, 9, 10, 1, 2, 3, 4, 5, 6, 7, 11 };

					foreach (var docId in documentIds)
					{
						var workerDocument = new WorkerDocumentModel
						{
							WorkerId = lastWorkerId,
							DocumentId = docId
						};
						_workerDocumentService.Create(workerDocument);
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
													   //return RedirectToAction(nameof(Index), model);    // If we pass data, it will append to url as a query string

			}
			catch
			{
				TempData["error"] = "Something went wrong!";
				return View("Index", model);
			}
		}


		public IActionResult Edit(int id)
		{


			// masters
			ViewBag.Department = _departmentService.GetAll();
			ViewBag.Sites = _siteService.GetAll();
			ViewBag.Projects = _projectService.GetAll();
			ViewBag.Qualifications = _qualificationService.GetAll();

			var p = _workerService.GetById(id);

			ViewBag.Worker = _workerService.GetProfileById(id, null);
			if (ViewBag.Worker == null)
			{
				return RedirectToAction(nameof(Index));// nameof checks method compiletime to avoid errors
			}

			ViewBag.Addresses = _workerAddressService.GetByWorkerId(id);
			ViewBag.MobilesNumbers = _workerMobileNumbersService.GetByWorkerId(id);

			return View(p);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(WorkerModel model, int ProjectId, int SiteId, int QualificationId, string MobileNumber, bool Status, int WorkerMobileNumberId1, int WorkerMobileNumberId2, int WorkerAddressId1, int WorkerAddressId2, string? MobileNumber2 = "", string? Address1 = "", string? Address2 = "")
		{
			try
			{
				if (!model.WorkerId.HasValue)
				{
					TempData["error"] = "WorkerId is required for updating.";
					return View(model);
				}

				if (ModelState.IsValid)
				{
					TempData["success"] = "Record updated successfully!";

					// Update main Worker details
					_workerService.Update(model);

					// Update project-sites
					var workerProjectSite = _workerProjectSiteService.GetByWorkerId(model.WorkerId.Value);

					WorkerProjectSiteModel workerProjectSiteModel = new WorkerProjectSiteModel
					{
						WorkerProjectSitesId = workerProjectSite.WorkerProjectSitesId,
						WorkerId = model.WorkerId ?? 0,
						ProjectId = ProjectId,
						SiteId = SiteId,
						Status = true
					};
					_workerProjectSiteService.Update(workerProjectSiteModel);


					// Update qualification
					var workerQualification = _workerQualificationService.GetByWorkerId(model.WorkerId.Value);
					WorkerQualificationModel workerQualificationModel = new WorkerQualificationModel
					{
						WorkerQualificationId = QualificationId,
						WorkerId = model.WorkerId ?? 0,
						QualificationId = QualificationId
					};
					_workerQualificationService.Create(workerQualificationModel);


					if (workerQualification != null)
					{
						workerQualification.QualificationId = QualificationId;
						_workerQualificationService.Update(workerQualification);
					}

					// Update primary mobile number
					WorkerMobileNumbersModel workerMobileNumbersModel = new WorkerMobileNumbersModel
					{
						WorkerMobileNumberId = WorkerMobileNumberId1,
						WorkerId = model.WorkerId ?? 0,
						MobileNumber = MobileNumber
					};
					_workerMobileNumbersService.Update(workerMobileNumbersModel);

					if (MobileNumber2 == "")
					{
						MobileNumber2 = MobileNumber;
					}
					WorkerMobileNumbersModel workerMobileNumbersModel_2 = new WorkerMobileNumbersModel
					{
						WorkerMobileNumberId = WorkerMobileNumberId2,
						WorkerId = model.WorkerId ?? 0,
						MobileNumber = MobileNumber2
					};
					_workerMobileNumbersService.Update(workerMobileNumbersModel_2);
					if (Address1 != "" && Address1 != null)
					{
						WorkerAddressModel workerAddressModel_1 = new WorkerAddressModel
						{
							WorkerAddressId = WorkerAddressId1,
							WorkerId = model.WorkerId ?? 0,
							AddressType = "Permanent",
							Address = Address1
						};
						_workerAddressService.Update(workerAddressModel_1);

					}

					if (Address2 != "" && Address2 != null)
					{
						WorkerAddressModel workerAddressModel_2 = new WorkerAddressModel
						{
							WorkerAddressId = WorkerAddressId2,
							WorkerId = model.WorkerId ?? 0,
							AddressType = "Current",
							Address = Address2
						};
						_workerAddressService.Update(workerAddressModel_2);
					}

					return RedirectToAction(nameof(Index));
				}
				else
				{
					// Handle model state errors
					var errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
					TempData["errors"] = errorMessages;
				}
			}
			catch (Exception ex)
			{
				TempData["error"] = $"Something went wrong! {ex.Message}";
			}

			return View(model);
		}




		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
			_workerService.Delete(id);

			TempData["success"] = "Project deleted successfully!";

			return RedirectToAction("Index");
		}
	}
}
