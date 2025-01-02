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

        [Required]
        public Boolean Status { get; set; } = true;

    }
}
