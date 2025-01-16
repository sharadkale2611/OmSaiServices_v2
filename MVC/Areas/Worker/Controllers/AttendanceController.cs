using Microsoft.AspNetCore.Mvc;
using OmSaiModels.Worker;
using OmSaiServices.Admin.Implementations;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Worker.Implimentation;
using LmsServices.Common;
using Microsoft.AspNetCore.Http;
using OmSaiServices.Admin.Interfaces;

namespace GeneralTemplate.Areas.Worker.Controllers
{
	[Area("Worker")]
	public class AttendanceController : Controller
	{
		private readonly WorkerService _workerService;
		private readonly WorkerAttendanceService _attendanceService;
		private readonly MultiMediaService _mms;
		private readonly SiteService _siteService;
		private readonly SiteShiftService _siteShiftService;


		public AttendanceController()
		{
			_workerService = new WorkerService();
			_attendanceService = new WorkerAttendanceService();
			_mms = new MultiMediaService();
			_siteService = new SiteService();
			_siteShiftService = new SiteShiftService();

		}

		public IActionResult Index()
		{
			ViewBag.AttendanceHistory = _attendanceService.GetAll();
			return View();
		}

		[HttpGet]
		[Route("api/Worker/SiteShiftJson/{siteId}")]
		public IActionResult SiteShiftJson(int siteId)
		{
			var result = _siteShiftService.GetBySiteId(siteId); // Fetch data
			if (result == null)
			{
				return Json(new { success = false, message = "No data found." });
			}
			return Json(new { success = true, data = result });
		}

		[HttpPost]
		public IActionResult CreateLedger(LedgerViewModel model)
		{
			model.SiteId = model.SiteId > 0 ? model.SiteId : null;
			model.SiteShiftId = model.SiteShiftId > 0 ? model.SiteShiftId : null;
			model.Year = model.Year > 0 ? model.Year : null;
			model.Month = model.Month > 0 ? model.Month : null;


			if (ModelState.IsValid)
			{
				ViewData["success"] = "Ledger updated successfully!";
				_attendanceService.CreateLedger(model.SiteId, model.SiteShiftId, model.Year, model.Month);
				return RedirectToAction("Ledger");
			}
			else
			{
				var errorMessages = ModelState.Values
									.SelectMany(v => v.Errors)
									.Select(e => e.ErrorMessage)
									.ToList();

				TempData["errors"] = errorMessages;
			}
			return RedirectToAction("Ledger");

		}

		public IActionResult LedgerNew(LedgerViewModel model, string? hasAllLedger = "yes")
		{
			ViewBag.Sites = _siteService.GetAll();

			if (ModelState.IsValid || hasAllLedger == "yes")
			{
				ViewBag.ShowData = true;

				// Set default or provided values for the filters
				ViewBag.Month = model.Month;
				ViewBag.MonthName = model.Month.HasValue && model.Month >= 1 && model.Month <= 12
					? new DateTime(1, model.Month.Value, 1).ToString("MMM")
					: null;

				ViewBag.Year = model.Year.ToString();
				ViewBag.SiteId = model.SiteId;
				ViewBag.SiteShiftId = model.SiteShiftId;

				// Call the service with appropriate parameters
				var allData = (hasAllLedger == "yes")
					? _attendanceService.GetLedger() // Retrieve all records
					: _attendanceService.GetLedger(null, model.SiteId, model.SiteShiftId, model.Year, model.Month);

				// Handle scenarios when no data is found
				if (allData != null && allData.Count > 0)
				{
					ViewBag.AllData = allData;
					ViewBag.SiteName = model.SiteId != 0 ? allData[0].SiteName : "All Sites";
				}
				else
				{
					ViewBag.AllData = new List<dynamic>(); // Use an empty list to prevent null reference errors
					ViewBag.SiteName = "No Data Available";
				}
			}
			else
			{
				ViewBag.ShowData = false;
			}

			return View();
		}




		public IActionResult Ledger(LedgerViewModel model, string? hasAllLedger="yes")
		{
			if(hasAllLedger == "yes")
			{
				ViewBag.Sites = _siteService.GetAll();

				ViewBag.ShowData = true;
				ViewBag.Month = null;
				ViewBag.MonthName = "---";
				ViewBag.Year = null;
				ViewBag.SiteId = null;
				ViewBag.SiteShiftId = null;
				var allData = _attendanceService.GetLedger(null, null, null, null, null);
				// Handle scenarios when no data is found
				if (allData != null && allData.Count > 0)
				{
					ViewBag.AllData = allData;
					ViewBag.SiteName = "All Sites";
				}
				else
				{
					ViewBag.AllData = new List<dynamic>(); // Use an empty list to prevent null reference errors
					ViewBag.SiteName = "No Data Available";
				}

				return View();

			}
			ViewBag.Sites = _siteService.GetAll();

			if (ModelState.IsValid)
			{
				ViewBag.ShowData = true;

				ViewBag.Month = model.Month;

				ViewBag.MonthName = model.Month.HasValue && model.Month >= 1 && model.Month <= 12
					? new DateTime(1, model.Month.Value, 1).ToString("MMM")
					: null;

				ViewBag.Year = model.Year.ToString();
				ViewBag.SiteId = model.SiteId;
				ViewBag.SiteShiftId = model.SiteShiftId;

				var allData = _attendanceService.GetLedger(null, model.SiteId, model.SiteShiftId, model.Year, model.Month);
				// Handle scenarios when no data is found
				if (allData != null && allData.Count > 0)
				{
					ViewBag.AllData = allData;
					ViewBag.SiteName = model.SiteId != null ? allData[0].SiteName : "All Sites";
				}
				else
				{
					ViewBag.AllData = new List<dynamic>(); // Use an empty list to prevent null reference errors
					ViewBag.SiteName = "No Data Available";
				}

			}
			else
			{
				ViewBag.ShowData = false;
			}

			return View();
		}


		

		public (string statusCode, string remark) ValidateAttendance(WorkerProfileModel worker, int graceTimeIn, int halfDayIn, int graceTimeOut, int halfDayOut, string InOutType)
		{

			string statusCode = "";
			string remark = "";
			
			TimeSpan currentTime = DateTime.Now.TimeOfDay; // Get the current time as TimeSpan

			TimeSpan? startTime = worker.StartTime; // Nullable TimeSpan
			TimeSpan? endTime = worker.EndTime; // Nullable TimeSpan
			
			if (startTime.HasValue && endTime.HasValue) // Check if startTime and endTime has a value
			{

				TimeSpan difference1 = currentTime - startTime.Value; // Calculate the difference
				int timDiffIn = (int)difference1.TotalMinutes; // Get total minutes as an integer

				TimeSpan difference2 = endTime.Value - currentTime; // Calculate the difference
				int timDiffOut = (int)difference2.TotalMinutes; // Get total minutes as an integer

				if (InOutType == "in")
				{
					if (timDiffIn < 0)
					{
						return ("shift-not-started", "Welcome! Please wait a moment, your shift hasn’t started yet.");
					}
					else if (timDiffIn <= graceTimeIn)
					{
						statusCode = "on-time-in";
						remark = "Welcome! You're on time today. Have a productive day ahead.";  // On-Time In
					}
					else if (timDiffIn <= halfDayIn)
					{
						statusCode = "half-day-in";
						remark = "Half-day attendance recorded. If you need to adjust, please inform HR.";  // Half Day In
					}
					else
					{
						statusCode = "absent-in";
						remark = "You are marked absent today. Please provide any updates or request for time off.";  // Absent In
					}
				}
				else if(InOutType == "out")
				{
					// Validate TimDiffOut
					if (timDiffOut <= graceTimeOut)
					{
						statusCode = "on-time-out";
						remark = "Thank you for completing the day on time. See you tomorrow!";  // On Time Out
					}
					else if (timDiffOut <= halfDayOut)
					{
						statusCode = "half-day-out";
						remark = "Half-day attendance recorded. If you need to adjust, please inform HR.";  // Half Day Out
					}
					else
					{
						statusCode = "early-exit";
						remark = "Your early exit has been recorded. Make sure to follow up on any pending work.";  // Early Exit
					}
				}
			}
			else
			{
				statusCode = "shift-not-set";
				remark = "Sorry! your shift not set, please contact to HR.";  // shift-not-set
			}
			return (statusCode, remark);
		}


		public IActionResult Create(string workmanId)
		{

			var worker = _workerService.GetProfileById(null, workmanId);

			if (worker == null || workmanId == null)
			{
				ViewData["Errpr"] = "Invalid WorkerId";
				return RedirectToAction(nameof(Error));
			}

			int GraceTime = 10;     // In minutes
			int halfDayIn = 60;     // In minutes
			int graceTimeOut = 5;     // In minutes
			int halfDayOut = 60;     // In minutes
			var currentDate = DateOnly.FromDateTime(DateTime.Now); // Convert current date and time to DateOnly
			var previousDate = currentDate.AddDays(-1);

			var dateParam = worker.EndTime < worker.StartTime ? previousDate : currentDate;
			var attendanceHistory = _attendanceService.GetAll(null, null, dateParam, workmanId, 1);

			bool isAttendanceAllowed = false;
			string InOutType = attendanceHistory.Count > 0 ? "out":"in";
			string oldStatus = attendanceHistory.Count > 0 ? attendanceHistory[0].Status : "NA";
			var (statusCode, remark) = ValidateAttendance(worker, GraceTime, halfDayIn, graceTimeOut, halfDayOut, InOutType);

			/*

				#################### Attendance Rules ##############################################

				on-time-in	=>	0 To 10 Minutes
				half-day-in =>	11 to 60 Minutes
				absent-in	=>	61 to (Next day shift)	Attendance option closed for current day

				on-time-out	 =>	0 to 5 Minutes early
				half-day-out =>	6 to 60 Minutes early
				early-exit	 => Before 61 Minute Attendanc option will available


					IN				OUT						Remark
				-------------------------------------------------------------------
				on-time-in		on-time-out		-->		P (Present)
				on-time-in		half-day-out	-->		HD (Half Day)
				on-time-in		early-exit		-->		AB (Absent)
				on-time-in		 -- 			-->		M (Miss Punch)

				half-day-in		on-time-out		-->		HD (Half Day)
				half-day-in		half-day-out	-->		AB (Absent)
				half-day-in		early-exit		-->		AB (Absent)	
				half-day-in		--				-->		M (Miss Punch)	

				absent-in		on-time-out		-->		AB (Absent)
				absent-in		half-day-out	-->		AB (Absent)
				absent-in		early-exit		-->		AB (Absent)
				absent-in		--				-->		AB (Absent)

			*/


			switch (statusCode)
			{
				case "on-time-in":
						isAttendanceAllowed = true;
						//InOutType = "in";

					break;
				case "half-day-in":
						isAttendanceAllowed = true;
						//InOutType = "in";
					break;
				case "absent-in":
						isAttendanceAllowed = false;
						//InOutType = "in";

					break;
				case "on-time-out":
						isAttendanceAllowed = true;
						//InOutType = "out";
					break;
				case "half-day-out":
						isAttendanceAllowed = true;
						//InOutType = "out";
					break;
				case "early-exit":
					isAttendanceAllowed = false;
					//InOutType = "out";
					break;
				case "shift-not-set":
					isAttendanceAllowed = false;
					break;
				case "shift-not-started":
					isAttendanceAllowed = false;
					break;
				default:
						isAttendanceAllowed = false;
						remark = "Something Went Wrong!";
					break;
			}

				if(attendanceHistory.Count > 0)
			{
				if (attendanceHistory[0].InTime != null && attendanceHistory[0].OutTime != null)
				{
					isAttendanceAllowed = false;
					remark = "Your today's attendance (In-Out) has been successfully recorded. Thank you!";
				}

			}

			string NewStatus = "AB";

			/*
						IN				OUT						Remark
				---------------------------------------------------------
				1	on-time-in		on-time-out		-->		P (Present)
				2	on-time-in		half-day-out	-->		HD (Half Day)
				3	on-time-in		early-exit		-->		AB (Absent)
				4	on-time-in		 -- 			-->		M (Miss Punch)

				5	half-day-in		on-time-out		-->		HD (Half Day)
				6	half-day-in		half-day-out	-->		AB (Absent)
				7	half-day-in		early-exit		-->		AB (Absent)	
				8	half-day-in		--				-->		M (Miss Punch)	

				9	absent-in		on-time-out		-->		AB (Absent)
				10	absent-in		half-day-out	-->		AB (Absent)
				11	absent-in		early-exit		-->		AB (Absent)
				12	absent-in		--				-->		AB (Absent)
			*/

			if (oldStatus == "on-time-in")
			{
				NewStatus = statusCode switch
				{
					"on-time-out" => "Present",     // P (Present)
					"half-day-out" => "Half-Day",   // HD (Half Day)
					"early-exit" => "Absent",       // AB (Absent)
					_ => NewStatus
				};
			}
			else if (oldStatus == "half-day-in")
			{
				NewStatus = statusCode switch
				{
					"on-time-out" => "Half-Day",    // HD (Half Day)
					_ => "Absent"                   // AB (Absent)
				};
			}
			else if (oldStatus == "absent-in")
			{
				NewStatus = "Absent";               // AB (Absent)
			}


			ViewBag.attendanceHistory = attendanceHistory;
			ViewBag.isAttendanceAllowed = isAttendanceAllowed;
			ViewBag.remark = remark;
			ViewBag.InOutType = InOutType;
			ViewBag.Worker = worker;
			ViewBag.CurrentTime = dateParam;
			ViewBag.Status = InOutType=="in"? statusCode: NewStatus;
			return View();
		}


		[HttpPost]
		[Route("api/WorkerAttendance")]
		public async Task<IActionResult> WorkerAttendance([FromForm] WorkerAttendanceModel model, IFormFile SelfieImage)
		{
			model.CurrentTime = DateTime.Now;
			try
			{
				// Validate the model
				if (!ModelState.IsValid)
				{
					return Json(new
					{
						success = false,
						message = "Invalid data provided.",
						errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
					});
				}

				// Check if an image is provided
				if (SelfieImage == null || SelfieImage.Length == 0)
				{
					return Json(new
					{
						success = false,
						message = "Selfie image is required. too"
					});
				}

								try
				{
					var uploadPath = "selfies"; // Define your folder path
					var maxFileSizeKb = 30; // 30KB size limit
					var quality = 20;
					var filePath = await _mms.SaveAndCompressImageAsync(SelfieImage, uploadPath, maxFileSizeKb, quality);

					// Set the selfie path in the model
					model.SelfieImage = filePath;

					// Call the service to manage attendance
					_attendanceService.ManageAttendance(model);



					//return Ok(new { FilePath = filePath, Message = "Image uploaded successfully." });
				}
				catch (Exception ex)
				{

					return Json(new
					{
						success = false,
						message = "An error occurred while processing attendance.",
						error = ex.Message
					});

				}


				// Return a success response
				return Json(new
				{
					success = true,
					message = "Attendance processed successfully.",
				});
			}
			catch (Exception ex)
			{
				// Log the exception if necessary

				// Return a failure response
				return Json(new
				{
					success = false,
					message = "An error occurred while processing attendance.",
					error = ex.Message
				});
			}
		}





		public IActionResult Error(string id)
		{
			ViewBag.WorkerId = id;

			return View();
		}


	}
}
