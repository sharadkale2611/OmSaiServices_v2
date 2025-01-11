using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OmSaiModels.Worker;
using OmSaiServices.Common;
using OmSaiServices.Worker.Interfaces;

namespace OmSaiServices.Worker.Implementations
{
    public class WorkerMobileNumbersService : Repository<WorkerMobileNumbersModel>, IWorkerMobileNumbersService
    {
        private readonly string sp_cud;
        private readonly string sp_r;
        private readonly Mapper _mapper;

        public WorkerMobileNumbersService()
        {
            sp_cud = "usp_CreateUpdateDeleteRestore_WorkerMobileNumbers";
            sp_r = "usp_GetAll_WorkerMobileNumbers";
            _mapper = new Mapper();
        }

        public int Create(WorkerMobileNumbersModel model)
        {
            return Create(model, sp_cud, CreateUpdate(model, "create"));
        }

        public void Update(WorkerMobileNumbersModel model)
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

        public List<WorkerMobileNumbersModel> GetAll()
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, WorkerMobileNumbersModel>(reader => _mapper.MapEntity<WorkerMobileNumbersModel>(reader));

            var result = GetAll(sp_r, mapEntity, GetParams());

            if (result == null) result = new List<WorkerMobileNumbersModel>();  // Ensure result is not null

            return result;
        }

        public WorkerMobileNumbersModel GetById(int id)
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, WorkerMobileNumbersModel>(reader => _mapper.MapEntity<WorkerMobileNumbersModel>(reader));

            return GetById(id, sp_r, mapEntity, GetParams(id));
        }

		public List<WorkerMobileNumbersModel> GetByWorkerId(int id)
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, WorkerMobileNumbersModel>(reader => _mapper.MapEntity<WorkerMobileNumbersModel>(reader));

			return GetAll(sp_r, mapEntity, GetParams(null, id));
		}


		private List<KeyValuePair<string, object>> CreateUpdate(WorkerMobileNumbersModel model, string type)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var WorkerMobileNumberId = type == "create" ? 0 : model.WorkerMobileNumberId;

            return new List<KeyValuePair<string, object>>
            {
                new("@WorkerMobileNumberId", WorkerMobileNumberId),
                new("@WorkerId", model.WorkerId),
                new("@MobileNumber", model.MobileNumber)
        
            };
        }


        private List<KeyValuePair<string, object>> DeleteRestore(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid WorkerMobileNumberId", nameof(id));

            return new List<KeyValuePair<string, object>>
    {
        new("@WorkerMobileNumberId", id),
    };
        }


        private SqlParameter[] GetParams(int? id = null, int? WorkerId = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@WorkerMobileNumberId", id),
                new SqlParameter("@WorkerId", WorkerId)				
			};
        }

    }
}
