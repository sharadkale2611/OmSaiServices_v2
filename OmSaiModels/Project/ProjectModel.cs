using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Project
{
	public class ProjectModel
	{
		[Key]
		public int ProjectId { get; set; } // Primary key

		[Required(ErrorMessage = "Project name is required.")]
		[StringLength(255, ErrorMessage = "Project name cannot exceed 255 characters.")]
		public string ProjectName { get; set; } // Name of the project

		[Required(ErrorMessage = "Required manpower is required.")]
		[Range(1, int.MaxValue, ErrorMessage = "Required manpower must be greater than 0.")]
		public int RequiredManPower { get; set; } // Required manpower

		[Required(ErrorMessage = "Project budget is required.")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Budget must be greater than 0.")]
		public decimal ProjectBudget { get; set; } // Budget for the project

		[StringLength(500, ErrorMessage = "Remark cannot exceed 500 characters.")]
		public string Remark { get; set; } // Optional remarks

		public bool Status { get; set; } = true; // Active or inactive (default is active)

		[DataType(DataType.DateTime)]
		public DateTime CreatedAt { get; set; } = DateTime.Now; // Timestamp when the record is created

		public bool IsDeleted { get; set; } = false; // Soft delete flag (default is false)

		[DataType(DataType.DateTime)]
		public DateTime? DeletedAt { get; set; } // Timestamp when the record is deleted

	}
}
