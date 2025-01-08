using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OmSaiModels.Worker
{
	public class LeaveRequestModel
	{
		[Key]
		public int? LeaveRequestId { get; set; }
		[Required]
		public int? WorkerId { get; set; }
		public string? WorkerName { get; set; }

		[Required]
		public int? LeaveTypeId { get; set; }
		public string? LeaveTypeName { get; set; }

		[Required]
		public DateOnly StartDate { get; set; }
		[Required]
		public DateOnly EndDate { get; set; }
		[Required]
		public string Reason { get; set; }
		public string? Status { get; set; }
		public string? Remark { get; set; }
		public string? ApproverId { get; set; }
		public string? UserName { get; set; }
		public DateTime? CreatedAt { get; set; }
		public DateTime? UpdatedAt { get; set; }

	}
	public class LeaveRequestApproveModel
	{
		[Key]
		public int? LeaveRequestId { get; set; }

		[Required]
		public string? Status { get; set; }

		[Required]
		public string? Remark { get; set; }

		[Required]
		public string? ApproverId { get; set; }


	}

}
