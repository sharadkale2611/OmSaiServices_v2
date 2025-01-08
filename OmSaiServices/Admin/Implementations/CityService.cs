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
    public  class CityService:Repository<CityModel>,ICityServices
    {
        private readonly string usp_cud;
        private readonly string usp_r;
        private readonly Mapper _mapper;

        public CityService()
        {
            usp_cud = "usp_CreateUpdateDeleteRestore_Cities";
            usp_r = "usp_GetAll_Cities";
            _mapper = new Mapper();
        }
        public int Create(CityModel model)
        {
            return Create(model, usp_cud, CreateUpdate(model, "create"));
        }

        public void Update(CityModel model)
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

        public List<CityModel> GetAll()
        {

            // Define the mapping function
            var mapEntity = new Func<IDataReader, CityModel>(reader => _mapper.MapEntity<CityModel>(reader));

            return GetAll(usp_r, mapEntity, GetParams());
        }

        public CityModel GetById(int id)
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, CityModel>(reader => _mapper.MapEntity<CityModel>(reader));

            return GetById(id, usp_r, mapEntity, GetParams(id));
        }


        private List<KeyValuePair<string, object>> CreateUpdate(CityModel model, string type)
        {

            var CityId = type == "create" ? 0 : model.CityId;



            return new List<KeyValuePair<string, object>>
            {
                new("@CityId", CityId),
                new("@CityName", model.CityName),
                new("@StateId", model.StateId)

            };
        }

        private List<KeyValuePair<string, object>> DeleteRestore(int id)
        {

            return new List<KeyValuePair<string, object>>
            {
                new("@CityId", id),

            };

        }

        private SqlParameter[] GetParams(int? id = null, string? CityName = null, int? StateId = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@CityId", id),
                new SqlParameter("@CityName", CityName),
                new SqlParameter("@StateId", StateId)

            };
        }
    }
}
