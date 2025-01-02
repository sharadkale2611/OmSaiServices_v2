using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
	public class BranchModel
	{
		[Key]
		public int BranchId { get; set; } // Auto-incrementing primary key

		[Required(ErrorMessage = "Branch Name is required.")]
		[StringLength(150, ErrorMessage = "Branch Name cannot exceed 150 characters.")]
		public string BranchName { get; set; } // Name of the branch

		[Required]
		public bool Status { get; set; } = true; // Status: true for Active, false for Inactive
		
		public int? OrderNumber { get; set; } // For sorting purpose

		public DateTime? CreatedAt { get; set; } = DateTime.Now; // Timestamp for creation

		
		public string CreatedBy { get; set; } // User who created the branch

		public DateTime? UpdatedAt { get; set; } // Timestamp for last update

		public string UpdatedBy { get; set; } // User who last updated the branch

	}

	public class BranchModelGetAll
	{
		[Key]
		public int? BranchId { get; set; } // Auto-incrementing primary key

		[Required(ErrorMessage = "Branch Name is required.")]
		public string BranchName { get; set; } // Name of the branch
		public bool Status { get; set; } = true; // Status: true for Active, false for Inactive
		public int? OrderNumber { get; set; } = 0;// For sorting purpose

		public DateTime? CreatedAt { get; set; } = DateTime.Now; // Timestamp for creation
		public string? CreatedBy { get; set; } // User who created the branch

		public DateTime? UpdatedAt { get; set; } // Timestamp for last update
		public string? UpdatedBy { get; set; } // User who last updated the branch


	}

}
