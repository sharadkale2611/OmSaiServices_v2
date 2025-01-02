using Microsoft.AspNetCore.Mvc;
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
		private readonly WorkerMobileNumbersService _workerMobileNumbersService;
		private readonly WorkerAttendanceService _attendanceService;
		private readonly WorkerDocumentService _workerDocumentService;


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

		public IActionResult Profile(int id)
		{
			ViewBag.AllData = _workerService.GetProfileById(id, null);
			ViewBag.AttendanceHistory = _attendanceService.GetAll(id);
			ViewBag.WorkerDocuments = _workerDocumentService.GetAll(id);


			return View();
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
		public IActionResult Create(WorkerModel model, int ProjectId, int SiteId, int QualificationId, string MobileNumber, bool Status)
		{
			try
			{
				model.WorkmanId = "";

				if (ModelState.IsValid)
				{
					TempData["success"] = "Record added successfully!";
					
					var lastWorkerId = _workerService.Create(model);
					
					WorkerProjectSiteModel model2 = new WorkerProjectSiteModel
					{
						WorkerId = lastWorkerId,
						SiteId = SiteId,
						ProjectId = ProjectId,
						Status = true
					};
					_workerProjectSiteService.Create(model2);


					WorkerQualificationModel model3 = new WorkerQualificationModel
					{
						WorkerId = lastWorkerId,
						QualificationId = QualificationId
					};
					_workerQualificationService.Create(model3);

					WorkerMobileNumbersModel model4 = new WorkerMobileNumbersModel
					{
						WorkerId = lastWorkerId,
						MobileNumber = MobileNumber
					};
					_workerMobileNumbersService.Create(model4);


					var documentIds = new List<int> { 1, 2, 3, 4, 5, 6, 7 };

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
			ViewBag.AllData = _workerService.GetAll();
			ViewBag.Department = _departmentService.GetAll();


			// masters
			ViewBag.Sites = _siteService.GetAll();
			ViewBag.Projects = _projectService.GetAll();
			ViewBag.Workers = _workerService.GetAll();
			ViewBag.Qualifications = _qualificationService.GetAll();



			var p = _workerService.GetById(id);
			return View(p);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(WorkerModel model)
		{

			try
			{
				if (ModelState.IsValid)
				{
					TempData["success"] = "Record updated successfully!";
					_workerService.Update(model);
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

				return RedirectToAction(nameof(Index));             // nameof checks method compiletime to avoid errors
			}
			catch
			{
				TempData["error"] = "Something went wrong!";
				return View("Index", model);
			}
			//_projectService.Update(model);
			//TempData["success"] = "Project Updated successfully!";
			//return View(model);
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
