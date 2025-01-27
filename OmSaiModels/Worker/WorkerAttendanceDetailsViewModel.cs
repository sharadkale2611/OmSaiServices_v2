using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
    public class WorkerAttendanceDetailsViewModel
    {
        public int WorkerId { get; set; }
        public string WorkerName { get; set; }
        public int? SiteId { get; set; }
        public string SiteName { get; set; }
        public int? SiteShiftId { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public string Status { get; set; }
    }



}
