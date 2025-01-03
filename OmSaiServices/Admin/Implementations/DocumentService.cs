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
    public class DocumentService : Repository<DocumentModel>, IDocumentService
    {
        private readonly string usp_cud;
        private readonly string usp_r;
        private readonly Mapper _mapper;

        public DocumentService()
        {
            usp_cud = "usp_CreateUpdateDeleteRestore_Documents";
            usp_r = "usp_GetAll_Documents";
            _mapper = new Mapper();
        }
        public int Create(DocumentModel model)
        {
            return Create(model, usp_cud, CreateUpdate(model, "create"));
        }

        public void Update(DocumentModel model)
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

        public List<DocumentModel> GetAll()
        {

            // Define the mapping function
            var mapEntity = new Func<IDataReader, DocumentModel>(reader => _mapper.MapEntity<DocumentModel>(reader));

            return GetAll(usp_r, mapEntity, GetParams());
        }

        public DocumentModel GetById(int id)
        {
            // Define the mapping function
            var mapEntity = new Func<IDataReader, DocumentModel>(reader => _mapper.MapEntity<DocumentModel>(reader));

            return GetById(id, usp_r, mapEntity, GetParams(id));
        }


        private List<KeyValuePair<string, object>> CreateUpdate(DocumentModel model, string type)
        {

            var DocumentId = type == "create" ? 0 : model.DocumentId;



            return new List<KeyValuePair<string, object>>
            {
                new("@DocumentId", DocumentId),
                new("@DocumentName", model.DocumentName),
                new("@Status", model.Status),

            };
        }

        private List<KeyValuePair<string, object>> DeleteRestore(int id)
        {

            return new List<KeyValuePair<string, object>>
            {
                new("@DocumentId", id),

            };

        }

        private SqlParameter[] GetParams(int? id = null, string? DocumentName = null, bool? Status = null)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@DocumentId", id),
                new SqlParameter("@DocumentName", DocumentName),
                new SqlParameter("@Status", Status)
            };
        }

    }
}
