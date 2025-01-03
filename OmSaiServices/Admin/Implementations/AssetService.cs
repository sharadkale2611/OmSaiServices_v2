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
	public class AssetService : Repository<AssetModel>, IAssetService
	{
		private readonly string sp_cud;     // sp_CreateUpdateSoftDeleteRestore     
		private readonly string sp_r;       // sp_GetAll
		private readonly Mapper _mapper;    // Generic method to map data from IDataReader to any model

		public AssetService()
		{
			sp_cud = "sp_CreateUpdateDeleteRestore_Assets";
			sp_r = "sp_GetAll_Assets";
			_mapper = new Mapper();
		}

		public int Create(AssetModel model)
		{
			return Create(model, sp_cud, CreateUpdate(model, "create"));
		}

		public void Update(AssetModel model)
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

		public List<AssetModel> GetAll()
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, AssetModel>(reader => _mapper.MapEntity<AssetModel>(reader));

			return GetAll(sp_r, mapEntity, GetParams());
		}

		public AssetModel GetById(int id)
		{
			// Define the mapping function
			var mapEntity = new Func<IDataReader, AssetModel>(reader => _mapper.MapEntity<AssetModel>(reader));

			return GetById(id, sp_r, mapEntity, GetParams(id));
		}


		private List<KeyValuePair<string, object>> CreateUpdate(AssetModel model, string type)
		{

			var AssetId = type == "create" ? 0 : model.AssetId;

			return new List<KeyValuePair<string, object>>
			{
				new("@AssetId", AssetId),
				new("@AssetName", model.AssetName),
				new("@Status", model.Status),

			};
		}

		private List<KeyValuePair<string, object>> DeleteRestore(int id)
		{

			return new List<KeyValuePair<string, object>>
			{
				new("@AssetId", id),

			};

		}

		private SqlParameter[] GetParams(int? id = null, string? AssetName = null, bool? Status = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@AssetId", id),
				new SqlParameter("@AssetName", AssetName),
				new SqlParameter("@Status", Status)
			};
		}



	}
}