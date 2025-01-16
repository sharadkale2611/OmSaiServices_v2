using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
	public class WorkerShiftModel
	{
		[Key]
		public int WorkerShiftId { get; set; }

		[Required]
		public int WorkerId { get; set; }
		public int SiteShiftId { get; set; }
		

		[Required]
		[StringLength(25)]
		public string ShiftName { get; set; }

		[Required]
		public TimeSpan StartTime { get; set; }

		[Required]
		public TimeSpan EndTime { get; set; }

		public bool IsDeleted { get; set; } = false;

		public DateTime? DeletedAt { get; set; }
	}
}
