using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
    public class WorkerAddressModel
    {
        //[Key]
      public int? WorkerAddressId { get; set; }
        [Required]

        public int WorkerId { get; set; }
        
        public string? WorkerName { get; set; }

        [Required]
        public string AddressType { get; set; }        

        public string? StateName { get; set; }
        [Required]
        public int? StateId { get; set; }
        [Required]

        public int? CityId { get; set; }
        public string? CityName { get; set; }

        [Required]
        public int? Pincode { get; set; }
        
        public string Address { get; set; }
        
        
    }
}
