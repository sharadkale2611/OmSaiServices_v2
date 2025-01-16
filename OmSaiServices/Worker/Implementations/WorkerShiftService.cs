using LmsServices.Common;
using Microsoft.Data.SqlClient;
using OmSaiModels.Worker;
using OmSaiServices.Common;
using OmSaiServices.Worker.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Worker.Implementations
{
	public class WorkerShiftService : Repository<WorkerShiftModel>, IWorkerShiftService
	{
		private readonly string sp_cud;     // sp_CreateUpdateSoftDeleteRestore     
		private readonly string sp_r;       // sp_GetAll
		private readonly Mapper _mapper;    // Generic method to map data from IDataReader to any model

		public WorkerShiftService()
		{
			sp_cud = "usp_CreateUpdateDeleteRestore_WorkerShifts";
			sp_r = "usp_GetAll_WorkerShifts";
			_mapper = new Mapper();

		}

		public int Create(WorkerShiftModel model)
		{
			return Create(model, sp_cud, CreateUpdate(model, "create"));
		}

		public void Update(WorkerShiftModel model)
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

		public List<WorkerShiftModel> GetAll()
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, WorkerShiftModel>(reader => _mapper.MapEntity<WorkerShiftModel>(reader));

			return GetAll(sp_r, mapEntity, GetParams());
		}

		public List<WorkerShiftModel> GetAllByWorkerId(int id)
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, WorkerShiftModel>(reader => _mapper.MapEntity<WorkerShiftModel>(reader));

			return GetAll(sp_r, mapEntity, GetParams(null, id));
		}


		public WorkerShiftModel GetById(int id)
		{
			// Define the mapping function
			var mapEntity = new Func<IDataReader, WorkerShiftModel>(reader => _mapper.MapEntity<WorkerShiftModel>(reader));

			return GetById(id, sp_r, mapEntity, GetParams(id));
		}

		
		private List<KeyValuePair<string, object>> CreateUpdate(WorkerShiftModel model, string type)
		{

			var LeaveRequestId = type == "create" ? 0 : model.WorkerShiftId;

			return new List<KeyValuePair<string, object>>
			{
				new("@WorkerShiftId", LeaveRequestId),
				new("@WorkerId", model.WorkerId),
				new("@SiteShiftId", model.SiteShiftId)
			};
		}

		private List<KeyValuePair<string, object>> DeleteRestore(int id)
		{

			return new List<KeyValuePair<string, object>>
			{
				new("@WorkerShiftId", id),

			};

		}

		private SqlParameter[] GetParams(int? id = null, int? WorkerId = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@WorkerShiftId", id),
				new SqlParameter("@WorkerId", WorkerId)
			};
		}


	}
}
