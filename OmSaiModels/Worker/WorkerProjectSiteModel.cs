
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
    public class WorkerProjectSiteModel
    {

        [Key]
        public int? WorkerProjectSitesId { get; set; }
        public int WorkerId { get; set; }
        public String? WorkerName { get; set; }
        public int ProjectId { get; set; }
        public String? ProjectName { get; set; }
        public int SiteId    { get; set; }
        public String? SiteName { get; set; }
        public bool Status { get; set; } = true;
    }
}
