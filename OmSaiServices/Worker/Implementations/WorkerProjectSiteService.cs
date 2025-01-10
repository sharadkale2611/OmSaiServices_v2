using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OmSaiModels.Worker;
using OmSaiServices.Worker.Interfaces;
using OmSaiServices.Common;

namespace OmSaiServices.Worker.Implementations
{
    public class WorkerProjectSiteService : Repository<WorkerProjectSiteModel>,IWorkerProjectSiteService
    {
        private readonly string sp_cud;       
        private readonly string sp_r;       
        private readonly Mapper _mapper;    

        public WorkerProjectSiteService()
        {
            sp_cud = "usp_CreateUpdateDeleteRestore_WorkerProjectSite";
            sp_r = "usp_GetAll_WorkerProjectSites";
            _mapper = new Mapper();
        }

        public int Create(WorkerProjectSiteModel model)
        {
            return Create(model, sp_cud, CreateUpdate(model, "create"));
        }

        public void Update(WorkerProjectSiteModel model)
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

        public List<WorkerProjectSiteModel> GetAll()
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, WorkerProjectSiteModel>(reader => _mapper.MapEntity<WorkerProjectSiteModel>(reader));

            var result = GetAll(sp_r, mapEntity, GetParams());

            if (result == null) result = new List<WorkerProjectSiteModel>();  // Ensure result is not null

            return result;
        }

        public WorkerProjectSiteModel GetById(int id)
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, WorkerProjectSiteModel>(reader => _mapper.MapEntity<WorkerProjectSiteModel>(reader));

            return GetById(id, sp_r, mapEntity, GetParams(id));
        }


		public WorkerProjectSiteModel GetByWorkerId(int id)
		{
			// Define the mapping function
			var mapEntity = new Func<IDataReader, WorkerProjectSiteModel>(reader => _mapper.MapEntity<WorkerProjectSiteModel>(reader));

			return GetById(id, sp_r, mapEntity, GetParams(null,id));
		}


		private List<KeyValuePair<string, object>> CreateUpdate(WorkerProjectSiteModel model, string type)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var WorkerProjectSitesId = type == "create" ? 0 : model.WorkerProjectSitesId;

            return new List<KeyValuePair<string, object>>
    {
        new("@WorkerProjectSitesId", WorkerProjectSitesId),
        new("@WorkerId", model.WorkerId),
        new("@ProjectId", model.ProjectId),
        new("@SiteId", model.SiteId),
        new("@Status", model.Status)
    };
        }


        private List<KeyValuePair<string, object>> DeleteRestore(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid WorkerProjectSitesId", nameof(id));

            return new List<KeyValuePair<string, object>>
    {
        new("@WorkerProjectSitesId", id),
    };
        }


        private SqlParameter[] GetParams(int? id = null, int? WorkerId = null, int? ProjectId = null, int? SiteId = null, bool? Status = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@WorkerProjectSitesId", id),
                new SqlParameter("@WorkerId", WorkerId),
                new SqlParameter("@ProjectId", ProjectId),
                new SqlParameter("@SiteId", SiteId)
            };
        }



    }
}
