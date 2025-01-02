using OmSaiModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
	public class WorkerProfileModel
	{
		public int WorkerId { get; set; }
		public string WorkmanId { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }

		public int DepartmentId { get; set; }
		public string DepartmentName { get; set; }

		//public DepartmentModel Department { get; set; }

		public int WorkerQualificationId { get; set; }
		public string QualificationName { get; set; }

		//public List<WorkerQualificationModel> WorkerQualifications { get; set; }

		public int WorkerMobileNumberId { get; set; }
		public string MobileNumber { get; set; }
		//public List<WorkerMobileNumbersModel> WorkerMobileNumbers { get; set; }

		public int ProjectId { get; set; }
		public string ProjectName { get; set; }

		public int SiteId { get; set; }
		public string SiteName { get; set; }
		public string SiteLocation { get; set; }
		public string GpsLocation { get; set; }
	}
}
