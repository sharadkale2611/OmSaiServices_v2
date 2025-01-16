using OmSaiModels.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
	public class WorkerModel
	{
		[Key]
		public int? WorkerId { get; set; }

		public string? WorkmanId { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string MiddleName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public int DepartmentId { get; set; }

		public string ProfileImage { get; set; }

		public string? DepartmentName { get; set; }
		public string? DepartmentShortName { get; set; }
		public string? SiteName { get; set; }

		[Required]
		public string MarritalStatus { get; set; }

		public DateTime DateofBirth { get; set; }

		[Required]

		public int Age { get; set; }

		[Required]
		public string Gender { get; set; }

		public string? SpouseName { get; set; }

		[Required]
		public DateTime DateofJoining { get; set; }

		public bool? Status { get; set; } = false;

		public string? Password { get; set; }
		//public int? WorkerMobileNumberId1 { get; set; }
		//public int? WorkerMobileNumberId2 { get; set; }
		//public int? WorkerAddressId1 { get; set; }
		//public int? WorkerAddressId2 { get; set; }
		

		List<DepartmentModel> list { get; set; }



	}


	public class WorkerLoginModel
	{
		public string LoginIdentifier { get; set; }
		public string Password { get; set; }

	}




}
