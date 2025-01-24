using LmsServices.Common;
using Microsoft.Data.SqlClient;
using OmSaiModels.Worker;
using OmSaiServices.Common;
using OmSaiServices.Worker.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace OmSaiServices.Worker.Implementations
{
	public class WorkerDocumentService : Repository<WorkerDocumentModel>, IWorkerDocumentService
	{
		private readonly string sp_cud;
		private readonly string sp_r;
		private readonly Mapper _mapper;


		public WorkerDocumentService()
		{
			sp_cud = "usp_CreateUpdate_WorkerDocuments";
			sp_r = "usp_Get_WorkerDocuments";
			_mapper = new Mapper();
		}
		public int Create(WorkerDocumentModel model)
		{

			return QueryService.NonQuery(sp_cud, CreateUpdate(model), "@LastInsertedId");
		}

		public int Update(WorkerDocumentModel model)
		{
			return QueryService.NonQuery(sp_cud, CreateUpdate(model), "@LastInsertedId");
		}

		public List<WorkerDocumentModel> GetAll(int? WorkerId = null, bool? IsVerified = null)
		{
			// Define the mapping function
			var mapEntity = new Func<IDataReader, WorkerDocumentModel>(reader => _mapper.MapEntity<WorkerDocumentModel>(reader));

			var result = GetAll(sp_r, mapEntity, GetParams(WorkerId, IsVerified));

			if (result == null) result = new List<WorkerDocumentModel>();  // Ensure result is not null

			return result;
		}


		private List<KeyValuePair<string, object>> CreateUpdate(WorkerDocumentModel model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			return new List<KeyValuePair<string, object>>
			{
				new("@WorkerId", model.WorkerId),
				new("@DocumentId", model.DocumentId),
				new("@DocumentNumber", model.DocumentNumber),
				new("@DocumentImage", model.DocumentImage)
			};
		}

		private SqlParameter[] GetParams( int? WorkerId = null, bool? IsVerified = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@WorkerId", WorkerId),
				new SqlParameter("@IsVerified", IsVerified)

			};
		}

		public void VerifyDocument(int id, bool IsVerified = true)
		{
			var parameters = new List<KeyValuePair<string, object>>
			{
				new("@WorkerDocumentId", id ),
				new("@IsVerified", IsVerified),
			};
			QueryService.NonQuery("VerifyWorkerDocument", parameters);
		}	
	}
}
