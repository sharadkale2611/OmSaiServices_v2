using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
    public class WorkerPresentAbsentModel
    {
        public int WorkerPresent { get; set; }  // Total Present Workers
        public int WorkerAbsent { get; set; }   // Total Absent Workers
    }
}