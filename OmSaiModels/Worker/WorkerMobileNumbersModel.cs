using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
    public class WorkerMobileNumbersModel
    {
        [Key]
        public int? WorkerMobileNumberId { get; set; }
        public int WorkerId { get; set; }
        public String? WorkerName { get; set; }
        public String MobileNumber { get; set; }
    }
}
