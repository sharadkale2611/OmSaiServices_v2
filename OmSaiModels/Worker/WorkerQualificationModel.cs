using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
    public class WorkerQualificationModel
    {
        [Key]
        public int? WorkerQualificationId { get; set; }

        [Required]
        public int WorkerId { get; set; }

        [Required]
        public int QualificationId { get; set; }
        
        public string? WorkerName { get; set; }

        public string? QualificationName { get; set; }
    }
}
