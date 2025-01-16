using LmsServices.Common;
using Microsoft.Data.SqlClient;
using OmSaiModels.Worker;
using OmSaiServices.Worker.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Worker.Implementations
{
	public class WorkerAttendanceService : IWorkerAttendanceService
	{
		private readonly Mapper _mapper;

		public WorkerAttendanceService()
		{
			_mapper = new Mapper();
		}

		public void ManageAttendance(WorkerAttendanceModel model)
		{

			var parameters =  new List<KeyValuePair<string, object>>
			{

				new("@WorkerId", model.WorkerId),
				new("@SiteId", model.SiteId),
				new("@ShiftId", model.ShiftId),
				new("@SelfieImage", model.SelfieImage),
				new("@GeoLocation", model.GeoLocation),
				new("@CurrentTime", DateTime.Now)
			};

			QueryService.NonQuery("usp_ManageWorkerAttendance", parameters);
		}


		public List<WorkerAttendanceViewModel> GetAll(int? WorkerId = null, int? SiteId = null, DateOnly? CurrentDate = null )
		{
			var mapEntity = new Func<IDataReader, WorkerAttendanceViewModel>(reader => _mapper.MapEntity<WorkerAttendanceViewModel>(reader));

			var parameters = new[]
			   {
					new SqlParameter("@WorkerId", WorkerId ?? (object)DBNull.Value),
					new SqlParameter("@SiteId", SiteId ?? (object)DBNull.Value),
					new SqlParameter("@CurrentDate", CurrentDate)
				};

			return QueryService.Query("usp_GetAttendanceData", mapEntity, parameters);
		}
      //new method create
        public WorkerAttendanceSummaryModel GetAttendanceSummary(int workerId, string attendanceMonth = null)
    {
        // Set the default attendance month to the current month if not provided
        attendanceMonth ??= DateTime.Now.ToString("MMMM");

        
        var mapEntity = new Func<IDataReader, WorkerAttendanceSummaryModel>(reader => _mapper.MapEntity<WorkerAttendanceSummaryModel>(reader));

        
        var parameters = new[]
        {
            new SqlParameter("@WorkerId", workerId),
            new SqlParameter("@AttendanceMonth", attendanceMonth)
        };

       
        var result = QueryService.Query("usp_WorkerAttendanceSummary", mapEntity, parameters);

        // If there is any result, return the first item, otherwise return a default WorkerAttendanceSummaryModel
        return result.Count > 0 ? result[0] : new WorkerAttendanceSummaryModel();
    }



    }
}
