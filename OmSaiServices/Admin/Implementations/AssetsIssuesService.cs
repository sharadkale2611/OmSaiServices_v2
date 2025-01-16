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
            sp_cud = "usp_CreateUpdateDeleteRestore_AssetIssues";
            sp_r = "usp_GetAll_AssetIssues";
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

            var AssetIssueId = type == "create" ? 0 : model.AssetIssueId;

            return new List<KeyValuePair<string, object>>
            {
                new("@AssetIssueId", AssetIssueId),
                new("@WorkerId", model.WorkerId),
                new("@AssetId", model.AssetId),
                new("@IssueBy", model.IssueBy),
                new("@ReturnTo", model.ReturnTo),
                new("@IsReturnable", model.IsReturnable),
                new("@Remark", model.Remark),
                new("@Status", model.Status)
            };
        }

        private List<KeyValuePair<string, object>> DeleteRestore(int id)
        {

            return new List<KeyValuePair<string, object>>
            {
                new("@AssetIssueId", id)
            };

        }

        private SqlParameter[] GetParams(int? id = null, int? WorkerId = null, string? AssetId = null, bool? Status = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@AssetIssueId", id?? (object)DBNull.Value),
                new SqlParameter("@WorkerId", WorkerId?? (object)DBNull.Value),
                new SqlParameter("@AssetId", AssetId?? (object)DBNull.Value),
                new SqlParameter("@Status", Status?? (object)DBNull.Value)
            };
        }
    }
}
