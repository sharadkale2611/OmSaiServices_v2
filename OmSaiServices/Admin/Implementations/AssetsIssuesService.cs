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
    public class AssetsIssuesService : Repository<AssetsIssuesModel>, IAssetsIssuesService
    {
        private readonly string sp_cud;
        private readonly string sp_r;
        private readonly Mapper _mapper;

        public AssetsIssuesService()
        {
            sp_cud = "usp_CreateUpdateDeleteRestore_AssetsIssues";
            sp_r = "usp_GelAll_AssetsIssues";
            _mapper = new Mapper();
        }


        public int Create(AssetsIssuesModel model)
        {
            return Create(model, sp_cud, CreateUpdate(model, "create"));
        }

        public void Update(AssetsIssuesModel model)
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

        public List<AssetsIssuesModel> GetAll()
        {

            // Define the mapping function
            var mapEntity = new Func<IDataReader, AssetsIssuesModel>(reader => _mapper.MapEntity<AssetsIssuesModel>(reader));

            return GetAll(sp_r, mapEntity, GetParams());
        }

        public AssetsIssuesModel GetById(int id)
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, AssetsIssuesModel>(reader => _mapper.MapEntity<AssetsIssuesModel>(reader));

            return GetById(id, sp_r, mapEntity, GetParams(id));
        }


        private List<KeyValuePair<string, object>> CreateUpdate(AssetsIssuesModel model, string type)
        {

            var AssetsIssuesId = type == "create" ? 0 : model.AssetsIssuesId;

            return new List<KeyValuePair<string, object>>
            {
                new("@AssetsIssuesId", AssetsIssuesId),
                new("@EmployeeId", model.EmployeeId),
                new("@AssetId", model.AssetId),
                new("@IssuesBy", model.IssuesBy),
                new("@IssuesAt", model.IssuesAt),
                new("@ReturnTo", model.ReturnTo),
                new("@ReturnAt", model.ReturnAt),
                new("@IsReturnAble", model.IsReturnAble),
                new("@Remark", model.Remark),
                new("@Status", model.Status)
            };
        }

        private List<KeyValuePair<string, object>> DeleteRestore(int id)
        {

            return new List<KeyValuePair<string, object>>
            {
                new("@AssetsIssuesId", id)
            };

        }

        private SqlParameter[] GetParams(int? id = null, int? EmployeeId = null, string? AssetId = null, string? IssuesBy = null, string? IssuesAt = null, string? ReturnTo = null, string? ReturnAt = null, string? IsReturnAble = null, string? Remark = null, bool? Status = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@AssetsIssuesId", id),
                new SqlParameter("@EmployeeId", EmployeeId),
                new SqlParameter("@AssetId", AssetId),
                new SqlParameter("@IssuesBy", IssuesBy),
                new SqlParameter("@IssuesAt", IssuesAt),
                new SqlParameter("@ReturnTo", ReturnTo),
                new SqlParameter("@ReturnAt", ReturnAt),
                new SqlParameter("@IsReturnAble", IsReturnAble),
                new SqlParameter("@Remark", Remark),
                new SqlParameter("@Status", Status)
            };
        }
    }
}
