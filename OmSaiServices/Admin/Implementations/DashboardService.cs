using LmsServices.Common;
using Microsoft.Data.SqlClient;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Interfaces;
using OmSaiServices.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace OmSaiServices.Admin.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly string usp_GetEmployeeAndWorkerStats;//1st procedure
        private readonly string usp_GetProjectSiteCounts;//4th procedure
        private readonly string usp_GetSiteWorkerCount; //2nd procedure 
        private readonly string usp_GetWorkerAttendanceSummary; // 3rd procedure 


        private readonly Mapper _mapper;

        public DashboardService()
        {
            usp_GetEmployeeAndWorkerStats = "usp_GetEmployeeAndWorkerStats";  //1st procedure
            usp_GetProjectSiteCounts = "GetProjectSiteCounts";//4th procedure
            usp_GetSiteWorkerCount = "usp_GetSiteWorkerCount"; //2nd procedure 
            usp_GetWorkerAttendanceSummary = "usp_GetWorkerAttendanceSummary";  // 3rd procedure 


            _mapper = new Mapper();
        }

        //public List<DashboardDataModel> GetDashboardData()
        //{

        //    var mapEntity = new Func<IDataReader, DashboardDataModel>(reader =>
        //    {
        //        return new DashboardDataModel
        //        {
        //            RoleName = reader["RoleName"].ToString(),
        //            TotalEmployees = Convert.ToInt32(reader["TotalEmployees"]),
        //            TotalWorkers = Convert.ToInt32(reader["TotalWorkers"])
        //        };
        //    });

        //    //  QueryService.Query to fetch the data
        //    var result = QueryService.Query(usp_GetEmployeeAndWorkerStats, mapEntity, new SqlParameter[] { });

        //    if (result == null) result = new List<DashboardDataModel>(); 

        //    return result;
        //}

        public List<DashboardDataModel> GetDashboardData()
        {
            var dashboardData = new List<DashboardDataModel>();
            int totalWorkers = 0;

            using (var conn = new SqlConnection("Server=A2NWPLSK14SQL-v04.shr.prod.iad2.secureserver.net;Database=om_sai_services_db_v1;User Id=omuser;Password=YSAAS#2024;TrustServerCertificate=True"))
            {
                conn.Open();
                using (var cmd = new SqlCommand(usp_GetEmployeeAndWorkerStats, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        //  first result set (role name and total employees)
                        while (reader.Read())
                        {
                            var item = new DashboardDataModel
                            {
                                RoleName = reader["RoleName"].ToString(),
                                TotalEmployees = Convert.ToInt32(reader["TotalEmployees"])
                            };
                            dashboardData.Add(item);
                        }

                        // Move to the second result set (total workers)
                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                totalWorkers = Convert.ToInt32(reader["TotalWorkers"]);
                            }
                        }
                    }
                }
            }

            // Set the same TotalWorkers value to all items in dashboardData list
            foreach (var item in dashboardData)
            {
                item.TotalWorkers = totalWorkers;
            }

            return dashboardData;
        }
        public List<ProjectSiteModel> GetProjectSiteData()
        {
            var projectSiteData = new List<ProjectSiteModel>();
            using (var conn = new SqlConnection("Server=A2NWPLSK14SQL-v04.shr.prod.iad2.secureserver.net;Database=om_sai_services_db_v1;User Id=omuser;Password=YSAAS#2024;TrustServerCertificate=True"))
            {
                conn.Open();
                using (var cmd = new SqlCommand(usp_GetProjectSiteCounts, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            projectSiteData.Add(new ProjectSiteModel
                            {
                                ProjectName = reader["ProjectName"].ToString(),
                                SiteCount = Convert.ToInt32(reader["SiteCount"])
                            });
                        }
                    }
                }
            }

            return projectSiteData;




        }
        public List<SiteWorkerModel> GetSiteWorkerData()
        {
            var siteWorkerData = new List<SiteWorkerModel>();

            using (var conn = new SqlConnection("Server=A2NWPLSK14SQL-v04.shr.prod.iad2.secureserver.net;Database=om_sai_services_db_v1;User Id=omuser;Password=YSAAS#2024;TrustServerCertificate=True"))
            {
                conn.Open();
                using (var cmd = new SqlCommand(usp_GetSiteWorkerCount, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new SiteWorkerModel
                            {
                                SiteName = reader["SiteName"].ToString(),
                                WorkerCount = Convert.ToInt32(reader["WorkerCount"])
                            };
                            siteWorkerData.Add(item);
                        }
                    }
                }
            }

            return siteWorkerData;
        }
        public WorkerPresentAbsentModel GetWorkerAttendanceSummary(DateTime? inputDate = null)
        {
            // If no date is provided, default to the current date
            if (!inputDate.HasValue)
            {
                inputDate = DateTime.Today;
            }

           
            var attendanceSummary = new WorkerPresentAbsentModel();

            using (var conn = new SqlConnection("Server=A2NWPLSK14SQL-v04.shr.prod.iad2.secureserver.net;Database=om_sai_services_db_v1;User Id=omuser;Password=YSAAS#2024;TrustServerCertificate=True"))
            {
                conn.Open();
                using (var cmd = new SqlCommand(usp_GetWorkerAttendanceSummary, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@InputDate", SqlDbType.Date) { Value = inputDate.Value });
                   
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            attendanceSummary.WorkerPresent = Convert.ToInt32(reader["WorkerPresent"]);
                            attendanceSummary.WorkerAbsent = Convert.ToInt32(reader["WorkerAbsent"]);
                        }
                    }
                }
            }

            return attendanceSummary;
        }



    }

}
