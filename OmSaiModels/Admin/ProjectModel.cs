using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
	public class ProjectModel
	{
		[Key]
		public int? ProjectId { get; set; }

		[Required(ErrorMessage = "Project Name is required.")]
		public string ProjectName { get; set; }

		public string? ProjectDiscription { get; set; }

		[Required]
		public int TotalManPower { get; set; }

		[Required]
		public float ProjectBudget { get; set; }

		[Required]
		public string Status { get; set; }

		public string? Remark { get; set; }

		public DateTime? ProjectStartDate { get; set; }

		public DateTime? ProjectEndDate { get; set; }
	}
}
