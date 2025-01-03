using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Interfaces;
using OmSaiServices.Common;

namespace OmSaiServices.Admin.Implementations
{
    public class SiteService : Repository<SiteModel>,ISiteService
    {
        private readonly string sp_cud;
        private readonly string sp_r;
        private readonly Mapper _mapper;

        public SiteService()
        {
            sp_cud = "usp_CreateUpdateDeleteRestore_Sites";
            //sp_r = "usp_GetAll_Sites";
            sp_r = "usp_GetAll_Project_Sites";
            _mapper = new Mapper();
        }


        public int Create(SiteModel model)
        {
            return Create(model, sp_cud, CreateUpdate(model, "create"));
        }

        public void Update(SiteModel model)
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

        public List<SiteModel> GetAll()
        {

            // Define the mapping function
            var mapEntity = new Func<IDataReader, SiteModel>(reader => _mapper.MapEntity<SiteModel>(reader));

            return GetAll(sp_r, mapEntity, GetParams());
        }

        public SiteModel GetById(int id)
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, SiteModel>(reader => _mapper.MapEntity<SiteModel>(reader));

            return GetById(id, sp_r, mapEntity, GetParams(id));
        }


        private List<KeyValuePair<string, object>> CreateUpdate(SiteModel model, string type)
        {

            var SiteId = type == "create" ? 0 : model.SiteId;

            return new List<KeyValuePair<string, object>>
            {
                new("@SiteId", SiteId),
                new("@ProjectId", model.ProjectId),
                new("@SiteName", model.SiteName),
                new("@SiteLocation", model.SiteLocation),
                new("@GpsLocation", model.GpsLocation),
                new("@Status", model.SiteStatus)
            };
        }

        private List<KeyValuePair<string, object>> DeleteRestore(int id)
        {

            return new List<KeyValuePair<string, object>>
            {
                new("@SiteId", id)
            };

        }

        private SqlParameter[] GetParams(int? id = null, int? ProjectId = null,string? SiteName=null, string? CreatedAt = null, bool? Status = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@SiteId", id),
                new SqlParameter("@ProjectId", ProjectId),
                new SqlParameter("@Status", Status)
            };
        }



    }
}
