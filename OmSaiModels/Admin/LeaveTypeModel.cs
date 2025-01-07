using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
	public class LeaveTypeModel
	{
		[Key]
		public int? LeaveTypeId { get; set; }

		[Required(ErrorMessage = "Leave Name is required.")]
		[StringLength(30, ErrorMessage = "Asset Name cannot exceed 30 characters.")]
		public string LeaveTypeName { get; set; }
		[Required]
		public bool Status { get; set; } = true;
	}
}
