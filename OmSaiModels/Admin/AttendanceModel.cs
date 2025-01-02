using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
    public class AttendanceModel
    {
        [Key]
        public int AttendanceId { get; set; }


        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime CheckInTime { get; set; }

        [Required]
        public DateTime CheckOutTime { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        //[Required]
        //public DateTime CreatedAt { get; set; }

        [Required]
        public string SelfiPath { get; set; }

        [Required]
        public string GeoLocation { get; set; }

        [Required]
        public int LateIn { get; set; }

        [Required]
        public int EarlyOut { get; set; }


        [Required]
        public string Remark { get; set; }


        [Required]
        public string Status { get; set; }

        //HEllo





    }
    }
