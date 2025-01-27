using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
    public class ProjectSiteModel
    {
        public string ProjectName { get; set; }  // Name of the project
        public int SiteCount { get; set; }      // Count of sites under that project
    }

}
