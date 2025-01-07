using Microsoft.Data.SqlClient;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Interfaces;
using OmSaiServices.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Admin.Implementations
{
	public class LeaveTypeService : Repository<LeaveTypeModel>, ILeaveTypeService
	{
		private readonly string sp_cud;     // sp_CreateUpdateSoftDeleteRestore     
		private readonly string sp_r;       // sp_GetAll
		private readonly Mapper _mapper;    // Generic method to map data from IDataReader to any model

		public LeaveTypeService()
		{
			sp_cud = "usp_CreateUpdateDeleteRestore_LeaveTypes";
			sp_r = "usp_GetAll_LeaveTypes";
			_mapper = new Mapper();
		}

		public int Create(LeaveTypeModel model)
		{
			return Create(model, sp_cud, CreateUpdate(model, "create"));
		}

		public void Update(LeaveTypeModel model)
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

		public List<LeaveTypeModel> GetAll()
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, LeaveTypeModel>(reader => _mapper.MapEntity<LeaveTypeModel>(reader));

			return GetAll(sp_r, mapEntity, GetParams());
		}

		public LeaveTypeModel GetById(int id)
		{
			// Define the mapping function
			var mapEntity = new Func<IDataReader, LeaveTypeModel>(reader => _mapper.MapEntity<LeaveTypeModel>(reader));

			return GetById(id, sp_r, mapEntity, GetParams(id));
		}


		private List<KeyValuePair<string, object>> CreateUpdate(LeaveTypeModel model, string type)
		{

			var LeaveTypeId = type == "create" ? 0 : model.LeaveTypeId;

			return new List<KeyValuePair<string, object>>
			{
				new("@LeaveTypeId", LeaveTypeId),
				new("@LeaveTypeName", model.LeaveTypeName),
				new("@Status", model.Status),

			};
		}

		private List<KeyValuePair<string, object>> DeleteRestore(int id)
		{

			return new List<KeyValuePair<string, object>>
			{
				new("@LeaveTypeId", id),

			};

		}

		private SqlParameter[] GetParams(int? id = null, string? LeaveTypeName = null, bool? Status = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@LeaveTypeId", id),
				new SqlParameter("@LeaveTypeName", LeaveTypeName),
				new SqlParameter("@Status", Status)
			};
		}
	}
}
