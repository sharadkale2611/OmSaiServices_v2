using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
    public class AssetsIssuesModel
    {
        [Key]
        public int? AssetIssueId { get; set; }

        [Required]
        public int WorkerId { get; set; }

        public string? WorkerName { get; set; }

        [Required]
        public int AssetId { get; set; }

        public string? AssetName { get; set; }

       
        public string? IssueBy { get; set; }

        public string? IssueByName {  get; set; }


        public DateTime? IssueAt { get; set; }

      
        public int? ReturnTo { get; set; }

         
        public DateTime? ReturnAt { get; set; }


        public bool IsReturnable { get; set; } 
        
        public DateTime? CreatedAt { get; set; }

       
        public int? CreatedBy { get; set; }

         
        public DateTime? UpdatedAt { get; set; }

        
        public int? UpdatedBy { get; set; }

        public string? Remark { get; set; }


        public bool Status { get; set; } = true;
    }
}
