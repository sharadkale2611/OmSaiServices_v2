using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
    public class DepartmentModel
    {
        [Key]
        public int? DepartmentId { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        [Required]
        public bool Status { get; set; }

    }
}
