using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
    public class DocumentModel
    {
        [Key]
        public int? DocumentId { get; set; }

        [Required(ErrorMessage = "DocumentName is required.")]
        [StringLength(100, ErrorMessage = "DocumentName cannot exceed 100 characters.")]
		public string DocumentName { get; set; }
        public bool IsDocumentNumber { get; set; } = false;
        public bool IsDocumentImage { get; set; } = true;
		
		[Required]
        public bool Status { get; set; } = true;

    }
}
