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
	public class SiteShiftService: Repository<SiteShiftModel>, ISiteShiftService
	{
		private readonly string sp_cud;
		private readonly string sp_r;
		private readonly Mapper _mapper;

		public SiteShiftService()
		{
			sp_cud = "usp_CreateUpdateDeleteRestore_SiteShifts";
			sp_r = "usp_GetAll_SiteShifts";
			_mapper = new Mapper();
		}

		public int Create(SiteShiftModel model)
		{
			return Create(model, sp_cud, CreateUpdate(model, "create"));
		}

		public void Update(SiteShiftModel model)
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

		public List<SiteShiftModel> GetAll()
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, SiteShiftModel>(reader => _mapper.MapEntity<SiteShiftModel>(reader));

			return GetAll(sp_r, mapEntity, GetParams());
		}

		public List<SiteShiftModel> GetBySiteId(int id)
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, SiteShiftModel>(reader => _mapper.MapEntity<SiteShiftModel>(reader));

			return GetAll(sp_r, mapEntity, GetParams(null, id));
		}



		public SiteShiftModel GetById(int id)
		{
			// Define the mapping function
			var mapEntity = new Func<IDataReader, SiteShiftModel>(reader => _mapper.MapEntity<SiteShiftModel>(reader));

			return GetById(id, sp_r, mapEntity, GetParams(id));
		}


		private List<KeyValuePair<string, object>> CreateUpdate(SiteShiftModel model, string type)
		{

			var SiteShiftId = type == "create" ? 0 : model.SiteShiftId;

			return new List<KeyValuePair<string, object>>
			{
				new("@SiteShiftId", SiteShiftId),
				new("@SiteId", model.SiteId),
				new("@ShiftName", model.ShiftName),
				new("@StartTime", model.StartTime),
				new("@EndTime", model.EndTime)				
			};
		}

		private List<KeyValuePair<string, object>> DeleteRestore(int id)
		{

			return new List<KeyValuePair<string, object>>
			{
				new("@SiteShiftId", id)
			};

		}

		private SqlParameter[] GetParams(int? id = null, int? SiteId = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@SiteShiftId", id),
				new SqlParameter("@SiteId", SiteId)
			};
		}


	}
}
