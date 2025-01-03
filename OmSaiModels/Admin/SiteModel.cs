using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
	public class SiteModel
	{
		[Key]
		public int? SiteId { get; set; }
		public int ProjectId { get; set; }
		public string? ProjectName { get; set; }
		public string SiteName { get; set; }
		public string? SiteLocation { get; set; }
		public string? GpsLocation { get; set; }
		public bool SiteStatus { get; set; } = true;
		
	}
}
