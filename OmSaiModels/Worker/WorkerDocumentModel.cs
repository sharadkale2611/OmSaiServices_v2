using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
	public class WorkerDocumentModel
	{
		[Key]
		public int? WorkerDocumentId { get; set; }
		
		[Required]
		public int WorkerId { get; set; }
		public string? WorkerName { get; set; }

		[Required]
		public int DocumentId { get; set; }
		public string? DocumentName { get; set; }
		public bool? IsDocumentImage { get; set; }
		public bool? IsDocumentNumber { get; set; }

		[StringLength(100)]
		public string? DocumentNumber { get; set; }

		[StringLength(100)]
		public string? DocumentImage { get; set; }

		public bool IsVerified { get; set; } 
		//public bool IsVerified { get; set; } = false;

		public DateTime? CreatedAt { get; set; } = DateTime.Now;
		public DateTime? UpdatedAt { get; set; } = DateTime.Now;

		public DateTime? VerifiedAt { get; set; }

		public int? VerifiedBy { get; set; }

		[StringLength(100)]
		public string? DocumentRemark { get; set; }

	}
}
