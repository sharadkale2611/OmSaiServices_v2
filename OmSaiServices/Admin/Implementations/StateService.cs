using Microsoft.Data.SqlClient;
using OmSaiModels.Admin;
using OmSaiServices.Admin.Interfaces;
using OmSaiServices.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Admin.Implementations
{
    public class StateService:Repository<StateModel>,IStateService
    {
        private readonly string usp_cud;
        private readonly string usp_r;
        private readonly Mapper _mapper;

        public StateService()
        {
            usp_cud = "usp_CreateUpdateDeleteRestore_States";
            usp_r = "usp_GetAll_States";
            _mapper = new Mapper();
        }
        public int Create(StateModel model)
        {
            return Create(model, usp_cud, CreateUpdate(model, "create"));
        }

        public void Update(StateModel model)
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

        public List<StateModel> GetAll()
        {

            // Define the mapping function
            var mapEntity = new Func<IDataReader, StateModel>(reader => _mapper.MapEntity<StateModel>(reader));

            return GetAll(usp_r, mapEntity, GetParams());
        }

        public StateModel GetById(int id)
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, StateModel>(reader => _mapper.MapEntity<StateModel>(reader));

            return GetById(id, usp_r, mapEntity, GetParams(id));
        }


        private List<KeyValuePair<string, object>> CreateUpdate(StateModel model, string type)
        {

            var StateId = type == "create" ? 0 : model.StateId;



            return new List<KeyValuePair<string, object>>
            {
                new("@StateId", StateId),
                new("@StateName", model.StateName),
               
            };
        }

        private List<KeyValuePair<string, object>> DeleteRestore(int id)
        {

            return new List<KeyValuePair<string, object>>
            {
                new("@StateId", id),

            };

        }

        private SqlParameter[] GetParams(int? id = null, string? StateName = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@StateId", (object?)id ?? DBNull.Value),
                new SqlParameter("@StateName", (object?)StateName ?? DBNull.Value),
            };
        }
    }
}
