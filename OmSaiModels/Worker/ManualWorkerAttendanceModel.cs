using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
    public class ManualWorkerAttendanceModel
    {
        public int WorkerId { get; set; }         // Worker ID
        public DateTime InTime { get; set; }      // Check-in time
        public DateTime OutTime { get; set; }     // Check-out time
        public string Status { get; set; }        // Attendance status (e.g., Present, Absent)
    }

}
