using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
	public class WorkerAttendanceModel
	{
		[Key]
		public int? WorkerAttendanceId { get; set; }

		[Required(ErrorMessage = "The WorkerId field is required.")]
		public int? WorkerId { get; set; }

		[Required(ErrorMessage = "The SiteId field is required.")]
		public int? SiteId { get; set; }

		[Required(ErrorMessage = "The ShiftId field is required.")]
		public int? ShiftId { get; set; }

		[MaxLength(500)]
		public string? SelfieImage { get; set; }

		[Required(ErrorMessage = "The GeoLocation field is required.")]
		[MaxLength(50)]
		public string? GeoLocation { get; set; }
		
		public DateTime? CurrentTime { get; set; }


		public int? VerifyBy { get; set; }
		public string? InOutType { get; set; }

		public string Status { get; set; }

		//public DateTime? InTime { get; set; }

		//public DateTime? OutTime { get; set; }

		//[MaxLength(500)]
		//public string? InSelfiPath { get; set; }

		//[MaxLength(500)]
		//public string? OutSelfiPath { get; set; }

		//[MaxLength(50)]
		//public string? InGeoLocation { get; set; }

		//[MaxLength(50)]
		//public string? OutGeoLocation { get; set; }

		//public int LateIn { get; set; } = 0;

		//public int EarlyOut { get; set; } = 0;

		//[Required]
		//[MaxLength(20)]
		//[RegularExpression("Absent|Present|Leave|Miss", ErrorMessage = "Invalid Status")]
		//public string? Status { get; set; }

		//public DateTime? CreatedAt { get; set; }

		//public bool? IsDeleted { get; set; } = false;

		//public DateTime? DeletedAt { get; set; }
	}


	public class WorkerAttendanceViewModel
	{
		[Key]
		public int WorkerAttendanceId { get; set; }  // Primary Key
		public int WorkerId { get; set; }
		public string WorkerName { get; set; }
		
		public int SiteId { get; set; }
		public string SiteName { get; set; }
		public int ShiftId { get; set; }
		public DateTime? InTime { get; set; }
		public DateTime? OutTime { get; set; }
		public string InSelfiPath { get; set; }
		public string OutSelfiPath { get; set; }
		public string InGeoLocation { get; set; }
		public string OutGeoLocation { get; set; }
		public bool LateIn { get; set; }
		public bool EarlyOut { get; set; }
		public string Status { get; set; }
		public DateTime CreatedAt { get; set; }

	}


}
