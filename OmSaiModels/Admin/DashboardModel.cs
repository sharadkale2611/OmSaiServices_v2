using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
    public class DashboardDataModel
    {
        public string RoleName { get; set; }  // Role Name for Total Employees per role
        public int TotalEmployees { get; set; }  // Total employees for each role

        public int TotalWorkers { get; set; }   // Total number of Workers 
    }
}
