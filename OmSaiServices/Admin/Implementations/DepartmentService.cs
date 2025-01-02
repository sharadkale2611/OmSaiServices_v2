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
    public class DepartmentService : Repository<DepartmentModel>, IDepartmentService
    {

        private readonly string sp_cud;
        private readonly string sp_r;
        private readonly Mapper _mapper;

        public DepartmentService()
        {
            sp_cud = "usp_CreateUpdateDeleteRestore_Departments";
            sp_r = "usp_GetAll_Departments";
            _mapper = new Mapper();
        }


        public int Create(DepartmentModel model)
        {
            return Create(model, sp_cud, CreateUpdate(model, "create"));
        }

        public void Update(DepartmentModel model)
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

        public List<DepartmentModel> GetAll()
        {

            // Define the mapping function
            var mapEntity = new Func<IDataReader, DepartmentModel>(reader => _mapper.MapEntity<DepartmentModel>(reader));

            return GetAll(sp_r, mapEntity, GetParams());
        }

        public DepartmentModel GetById(int id)
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, DepartmentModel>(reader => _mapper.MapEntity<DepartmentModel>(reader));

            return GetById(id, sp_r, mapEntity, GetParams(id));
        }


        private List<KeyValuePair<string, object>> CreateUpdate(DepartmentModel model, string type)
        {

            var DepartmentId = type == "create" ? 0 : model.DepartmentId;

            return new List<KeyValuePair<string, object>>
            {
                new("@DepartmentId", DepartmentId),
                new("@DepartmentName", model.DepartmentName),
                new("@Status", model.Status)
            };
        }

        private List<KeyValuePair<string, object>> DeleteRestore(int id)
        {

            return new List<KeyValuePair<string, object>>
            {
                new("@DepartmentId", id)
            };

        }

        private SqlParameter[] GetParams(int? id = null, string? DepartmentName = null, bool? Status = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@DepartmentId", id),
                new SqlParameter("@DepartmentName", DepartmentName),
                new SqlParameter("@Status", Status)
            };
        }

      
    }
}
