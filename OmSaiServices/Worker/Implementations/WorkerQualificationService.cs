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
    public class WorkerQualificationService : Repository<WorkerQualificationModel>
    {
        private readonly string usp_cud;
        private readonly string usp_r;
        private readonly Mapper _mapper;

        public WorkerQualificationService()
        {
            usp_cud = "usp_CreateUpdateDeleteRestore_WorkerQualifications";
            usp_r = "usp_GetAll_WorkerQualifications";
            _mapper = new Mapper();
        }
        public int Create(WorkerQualificationModel model)
        {
            return Create(model, usp_cud, CreateUpdate(model, "create"));
        }

        public void Update(WorkerQualificationModel model)
        {
            Update(model, usp_cud, CreateUpdate(model, "update"));
        }


        public void Delete(int id)
        {
            Delete(usp_cud, DeleteRestore(id));
        }

        public void Restore(int id)
        {
            Restore(usp_cud, DeleteRestore(id));

        }

        public List<WorkerQualificationModel> GetAll()
        
        {

            // Define the mapping function
            var mapEntity = new Func<IDataReader, WorkerQualificationModel>(reader => _mapper.MapEntity<WorkerQualificationModel>(reader));

            return GetAll(usp_r, mapEntity, GetParams());
        }


		public WorkerQualificationModel GetByWorkerId(int id)
		{
			// Define the mapping function
			var mapEntity = new Func<IDataReader, WorkerQualificationModel>(reader => _mapper.MapEntity<WorkerQualificationModel>(reader));

			return GetById(id, usp_r, mapEntity, GetParams(null,id));
		}

		public WorkerQualificationModel GetById(int id)
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, WorkerQualificationModel>(reader => _mapper.MapEntity<WorkerQualificationModel>(reader));

            return GetById(id, usp_r, mapEntity, GetParams(id));
        }


        private List<KeyValuePair<string, object>> CreateUpdate(WorkerQualificationModel model, string type)
        {

            var WorkerQualificationId = type == "create" ? 0 : model.WorkerQualificationId;



            return new List<KeyValuePair<string, object>>
            {
                new("@WorkerQualificationId", WorkerQualificationId),
                new("@WorkerId", model.WorkerId),
                new("@QualificationId", model.QualificationId),

            };
        }

        private List<KeyValuePair<string, object>> DeleteRestore(int id)
        {

            return new List<KeyValuePair<string, object>>
            {
                new("@WorkerQualificationId", id),

            };

        }

        private SqlParameter[] GetParams(int? id = null, int? WorkerId = null, int? QualificationId = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@WorkerQualificationId", id),
                new SqlParameter("@WorkerId", WorkerId),
                new SqlParameter("@QualificationId", QualificationId)
            };
        }

    }

}
