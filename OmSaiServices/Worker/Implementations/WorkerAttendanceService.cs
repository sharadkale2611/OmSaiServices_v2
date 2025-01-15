﻿using LmsServices.Common;
using Microsoft.Data.SqlClient;
using OmSaiModels.Worker;
using OmSaiServices.Worker.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
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

		public void ManageAttendance(WorkerAttendanceModel model )
		{

			// Use DateTime.Now if CurrentTime is null
			model.CurrentTime ??= DateTime.Now;

			var parameters =  new List<KeyValuePair<string, object>>
			{

				new("@WorkerId", model.WorkerId),
				new("@SiteId", model.SiteId),
				new("@ShiftId", model.ShiftId), 
				new("@Status", model.Status),
				new("@SelfieImage", model.SelfieImage),
				new("@GeoLocation", model.GeoLocation),
				new("@CurrentTime", model.CurrentTime),
				new("@InOutType", model.InOutType)
			};

			QueryService.NonQuery("usp_ManageWorkerAttendance", parameters);
		}


		public List<WorkerAttendanceViewModel> GetAll(int? WorkerId = null, int? SiteId = null, DateOnly? CurrentDate = null, string? WorkmanId = null, int? RecordCount = null)
		{
			var mapEntity = new Func<IDataReader, WorkerAttendanceViewModel>(reader => _mapper.MapEntity<WorkerAttendanceViewModel>(reader));

			var parameters = new[]
			   {
					new SqlParameter("@WorkerId", WorkerId ?? (object)DBNull.Value),
					new SqlParameter("@SiteId", SiteId ?? (object)DBNull.Value),
					new SqlParameter("@CurrentDate", CurrentDate),
					new SqlParameter("@WorkmanId", WorkmanId),
					new SqlParameter("@RecordCount", RecordCount)
				};

			return QueryService.Query("usp_GetAttendanceData", mapEntity, parameters);
		}


	}
}
