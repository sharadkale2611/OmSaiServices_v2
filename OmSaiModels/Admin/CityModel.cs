using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
    public class CityModel
    {
        [Key]
        public int CityId { get; set; }
        [Required]
        [StringLength(100)]
        public string CityName { get; set; }
        [Required]
        public int StateId { get; set; }
        public string StateName { get; set; }

    }
}
