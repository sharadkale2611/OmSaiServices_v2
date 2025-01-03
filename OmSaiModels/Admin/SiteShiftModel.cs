using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
	public class SiteShiftModel
	{
		[Key]
		public int? SiteShiftId { get; set; }

		[Required]
		public int SiteId { get; set; }
		public string? SiteName { get; set; }
		
		[Required]
		[StringLength(50)]
		public string ShiftName { get; set; }

		[Required]
		public TimeSpan StartTime { get; set; }

		[Required]
		public TimeSpan EndTime { get; set; }

	}
}
