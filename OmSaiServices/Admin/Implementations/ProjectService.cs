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
	public class ProjectService : Repository<ProjectModel>, IProjectService
	{
		private readonly string sp_cud;     // sp_CreateUpdateSoftDeleteRestore     
		private readonly string sp_r;       // sp_GetAll
		private readonly Mapper _mapper;    // Generic method to map data from IDataReader to any model

		public ProjectService()
		{
			sp_cud = "usp_CreateUpdateDeleteRestore_Projects";
			sp_r = "usp_GetAll_Projects";
			_mapper = new Mapper();
		}

		public int Create(ProjectModel model)
		{
			return Create(model, sp_cud, CreateUpdate(model, "create"));
		}

		public void Update(ProjectModel model)
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

		public List<ProjectModel> GetAll()
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, ProjectModel>(reader => _mapper.MapEntity<ProjectModel>(reader));

			return GetAll(sp_r, mapEntity, GetParams());
		}

		public ProjectModel GetById(int id)
		{
			// Define the mapping function
			var mapEntity = new Func<IDataReader, ProjectModel>(reader => _mapper.MapEntity<ProjectModel>(reader));

			return GetById(id, sp_r, mapEntity, GetParams(id));
		}


		private List<KeyValuePair<string, object>> CreateUpdate(ProjectModel model, string type)
		{

			var ProjectId = type == "create" ? 0 : model.ProjectId;

			return new List<KeyValuePair<string, object>>
			{
				new("@ProjectId", ProjectId),
				new("@ProjectName", model.ProjectName),
				new("@ProjectDiscription", model.ProjectDiscription),
				new("@TotalManPower", model.TotalManPower),
				new("@ProjectBudget", model.ProjectBudget),
				new("@Status", model.Status),
				new("@Remark", model.Remark),
				new("@ProjectStartDate", model.ProjectStartDate),
				new("@ProjectEndDate", model.ProjectEndDate)
			};
		}

		private List<KeyValuePair<string, object>> DeleteRestore(int id)
		{

			return new List<KeyValuePair<string, object>>
			{
				new("@ProjectId", id),

			};

		}

		private SqlParameter[] GetParams(int? id = null, string? ProjectName = null, bool? Status = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@ProjectId", id),
				new SqlParameter("@ProjectName", ProjectName),

				new SqlParameter("@Status", Status),

			};
		}
	}
}
