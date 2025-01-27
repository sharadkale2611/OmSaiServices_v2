using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
    public class SiteWorkerModel
    {
        public string SiteName { get; set; } // Name of the site
        public int WorkerCount { get; set; } // Number of workers at that site
    }
}