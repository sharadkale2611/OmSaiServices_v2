using LmsServices.Common;
using Microsoft.Data.SqlClient;
using OmSaiModels.Admin;
using OmSaiModels.Worker;
using OmSaiServices.Common;
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
	public class LeaveRequestService : Repository<LeaveRequestModel>, ILeaveRequestService
	{
		private readonly string sp_cud;     // sp_CreateUpdateSoftDeleteRestore     
		private readonly string sp_r;       // sp_GetAll
		private readonly Mapper _mapper;    // Generic method to map data from IDataReader to any model

		public LeaveRequestService()
		{
			sp_cud = "usp_CreateUpdateDeleteRestore_LeaveRequests";
			sp_r = "usp_GetAll_LeaveRequests";
			_mapper = new Mapper();
		}

		public int Create(LeaveRequestModel model)
		{
			return Create(model, sp_cud, CreateUpdate(model, "create"));
		}

		public void Update(LeaveRequestModel model)
		{
			Update(model, sp_cud, CreateUpdate(model, "update"));
		}


		public void Delete(int id)
		{
			Delete(sp_cud, DeleteRestore(id));
		}

		public void Restore(int id)
		{
			Restore(sp_cud, DeleteRestore(id));

		}

		public List<LeaveRequestModel> GetAll()
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, LeaveRequestModel>(reader => _mapper.MapEntity<LeaveRequestModel>(reader));

			return GetAll(sp_r, mapEntity, GetParams());
		}

		public List<LeaveRequestModel> GetAllByWorkerId(int id)
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, LeaveRequestModel>(reader => _mapper.MapEntity<LeaveRequestModel>(reader));

			return GetAll(sp_r, mapEntity, GetParams(null, id));
		}


		public LeaveRequestModel GetById(int id)
		{
			// Define the mapping function
			var mapEntity = new Func<IDataReader, LeaveRequestModel>(reader => _mapper.MapEntity<LeaveRequestModel>(reader));

			return GetById(id, sp_r, mapEntity, GetParams(id));
		}

		public void Approve(LeaveRequestApproveModel model)
		{
			// Add a @type parameter manually
			var parameters = new List<KeyValuePair<string, object>>
			{
				new("@Type", "APPROVE"),
				new("@LeaveRequestId", model.LeaveRequestId),
				new("@ApproverId", model.ApproverId),
				new("@Remark", model.Remark),
				new("@Status", model.Status),
			};
			QueryService.NonQuery(sp_cud, parameters, "@LastInsertedId");
		}


		private List<KeyValuePair<string, object>> CreateUpdate(LeaveRequestModel model, string type)
		{

			var LeaveRequestId = type == "create" ? 0 : model.LeaveRequestId;

			return new List<KeyValuePair<string, object>>
			{
				new("@LeaveRequestId", LeaveRequestId),
				new("@WorkerId", model.WorkerId),
				new("@LeaveTypeId", model.LeaveTypeId),
				new("@StartDate", model.StartDate),
				new("@EndDate", model.EndDate),
				new("@Reason", model.Reason)
				
			};
		}

		private List<KeyValuePair<string, object>> DeleteRestore(int id)
		{

			return new List<KeyValuePair<string, object>>
			{
				new("@LeaveRequestId", id),

			};

		}

		private SqlParameter[] GetParams(int? id = null, int? WorkerId = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@LeaveRequestId", id),
				//new SqlParameter("@LeaveTypeId", LeaveTypeId),
				new SqlParameter("@WorkerId", WorkerId),

				//new SqlParameter("@ApproverId", ApproverId),
				//new SqlParameter("@Status", Status)
			};
		}

		//public int Approve(LeaveRequestModel model)
		//{
		//	return Approve(model, sp_cud, Approve(model, "approve"));
		//}

		//private List<KeyValuePair<string, object>> Approve(LeaveRequestModel model, string type)
		//{

		//	var LeaveRequestId = type == "approve" ? 0 : model.LeaveRequestId;

		//	return new List<KeyValuePair<string, object>>
		//	{
		//		//new("@LeaveRequestId", LeaveRequestId),
		//		//new("@WorkerId", model.WorkerId),
		//		new("@ApproverId", model.ApproverId),

		//		new("@Remark", model.Remark),
		//		new("@Status", model.Status),
		//		new("@UpdatedAt", model.UpdatedAt),
		//	};
		//}

		
	}
}
