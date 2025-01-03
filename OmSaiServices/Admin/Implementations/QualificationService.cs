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
	public class QualificationService : Repository<QualificationModel>, IQualificatinoService
	{
		private readonly string sp_cud;
		private readonly string sp_r;
		private readonly Mapper _mapper;

		public QualificationService()
		{
			sp_cud = "usp_CreateUpdateDeleteRestore_Qualifications";
			sp_r = "usp_GetAll_Qualifications";
			_mapper = new Mapper();
		}


		public int Create(QualificationModel model)
		{
			return Create(model, sp_cud, CreateUpdate(model, "create"));
		}

		public void Update(QualificationModel model)
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

		public List<QualificationModel> GetAll()
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, QualificationModel>(reader => _mapper.MapEntity<QualificationModel>(reader));

			return GetAll(sp_r, mapEntity, GetParams());
		}

		public QualificationModel GetById(int id)
		{
			// Define the mapping function
			var mapEntity = new Func<IDataReader, QualificationModel>(reader => _mapper.MapEntity<QualificationModel>(reader));

			return GetById(id, sp_r, mapEntity, GetParams(id));
		}


		private List<KeyValuePair<string, object>> CreateUpdate(QualificationModel model, string type)
		{

			var QualificationId = type == "create" ? 0 : model.QualificationId;

			return new List<KeyValuePair<string, object>>
			{
				new("@QualificationId", QualificationId),
				new("@QualificationName", model.QualificationName),
				new("@Status", model.Status)
			};
		}

		private List<KeyValuePair<string, object>> DeleteRestore(int id)
		{

			return new List<KeyValuePair<string, object>>
			{
				new("@QualificationId", id)
			};

		}

		private SqlParameter[] GetParams(int? id = null, string? QualificationName = null, bool? Status = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@QualificationId", id),
				new SqlParameter("@QualificationName", QualificationName),
				new SqlParameter("@Status", Status)
			};
		}

	}
}
