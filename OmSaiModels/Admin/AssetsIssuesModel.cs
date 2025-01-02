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
        public int AssetsIssuesId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int AssetId { get; set; }
        [Required]
        public int IssuesBy { get; set; }
        [Required]
        public DateTime IssuesAt { get; set; }
        [Required]
        public int ReturnTo { get; set; }
         [Required]
        public DateTime ReturnAt { get; set; }
         [Required]
        public bool IsReturnAble { get; set; }
         [Required]
        public DateTime CreatedAt { get; set; }
         [Required]
        public int CreatedBy { get; set; }
         [Required]
        public DateTime UpdatedAt { get; set; }
         [Required]
        public int UpdatedBy { get; set; }
        [Required]
        public DateTime DeletedAt { get; set; }
        [Required]
        public int DeletedBy { get; set; }
         [Required]
        public string Remark { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
