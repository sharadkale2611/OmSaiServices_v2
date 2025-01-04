using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
    public  class HolidayModel
    {
        [Key]
        public int? HolidayId { get; set; }

        [Required]
        [StringLength(100)]
        public string HolidayName { get; set;}

        [Required]
        public DateTime HolidayDate {  get; set; }

        [Required]
        public Boolean IsRecurring { get; set; }

        [Required]
        public Boolean Status {  get; set; }

    }
}
