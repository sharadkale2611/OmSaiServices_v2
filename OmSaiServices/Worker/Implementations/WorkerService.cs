using LmsServices.Common;
using Microsoft.Data.SqlClient;
using OmSaiModels.Worker;
using OmSaiServices.Common;
using OmSaiServices.Worker.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Worker.Implimentation
{
	public class WorkerService : Repository<WorkerModel>, IWorkerService
	{
		private readonly string sp_cud;     // sp_CreateUpdateSoftDeleteRestore     
		private readonly string sp_r;       // sp_GetAll
		private readonly Mapper _mapper;    // Generic method to map data from IDataReader to any model

		public WorkerService()
		{
			sp_cud = "usp_CreateUpdateDeleteRestore_Workers";
			sp_r = "usp_GetAll_Workers";
			_mapper = new Mapper();
		}

		public int RowCount()
		{
			return CommonService.RowCount("Workers");
		}


		public int Create(WorkerModel model)
		{
			string WorkerCode = $"W{(RowCount() + 1):000000}"; // Ensures leading zeros up to 6 digits
			model.WorkmanId = WorkerCode;
			return Create(model, sp_cud, CreateUpdate(model, "create"));
		}

		public void Update(WorkerModel model)
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

		public List<WorkerModel> GetByWorkManId(string workmanId)
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, WorkerModel>(reader => _mapper.MapEntity<WorkerModel>(reader));

			return GetAll(sp_r, mapEntity, GetParams(null, workmanId, null));
		}


		public List<WorkerModel> GetAll()
		{

			// Define the mapping function
			var mapEntity = new Func<IDataReader, WorkerModel>(reader => _mapper.MapEntity<WorkerModel>(reader));

			return GetAll(sp_r, mapEntity, GetParams());
		}

		

		public WorkerProfileModel GetProfileById(int? id = null, string? workmanId = null)
		{
			var mapEntity = new Func<IDataReader, WorkerProfileModel>(
				   reader => _mapper.MapEntity<WorkerProfileModel>(reader)
			   );

			var result = QueryService.Query("usp_GetAll_WorkerProfile", mapEntity, GetParamsProfile(id, workmanId));
			return result?.FirstOrDefault();

		}


		public WorkerModel GetById(int id)
		{
			// Define the mapping function
			var mapEntity = new Func<IDataReader, WorkerModel>(reader => _mapper.MapEntity<WorkerModel>(reader));

			return GetById(id, sp_r, mapEntity, GetParams(id));
		}


		private List<KeyValuePair<string, object>> CreateUpdate(WorkerModel model, string type)
		{

			var WorkerId = type == "create" ? 0 : model.WorkerId;

			return new List<KeyValuePair<string, object>>
			{
				new("@WorkerId", WorkerId),
				new("@WorkmanId", model.WorkmanId),
				new("@FirstName", model.FirstName),
				new("@MiddleName", model.MiddleName),
				new("@LastName", model.LastName),
				new("@DepartmentId", model.DepartmentId),
				new("@MarritalStatus", model.MarritalStatus),
				new("@DateofBirth", model.DateofBirth),
				new("@Age", model.Age),
				new("@Gender", model.Gender),
				new("@SpouseName", model.SpouseName),
				new("@DateofJoining", model.DateofJoining),
		        new("@Password", model.Password)


            };
		}

		private List<KeyValuePair<string, object>> DeleteRestore(int id)
		{

			return new List<KeyValuePair<string, object>>
			{
				new("@WorkerId", id),

			};

		}

		private SqlParameter[] GetParams(int? id = null, string? WorkmanId = null, int? DepartmentId = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@WorkerId", id),
				new SqlParameter("@WorkmanId", WorkmanId),

				new SqlParameter("@DepartmentId", DepartmentId)

			};
		}

		private SqlParameter[] GetParamsProfile(int? id = null, string? WorkmanId = null)
		{
			return new SqlParameter[]
			{
				new SqlParameter("@WorkerId", id),
				new SqlParameter("@WorkmanId", WorkmanId)

            };
		}

		public bool ChangePassword(int WorkerId, string oldPassword, string newPassword)
		{
			// Fetch the worker details using the WorkerId
			var worker = GetById(WorkerId);
			if (worker == null)
			{
				throw new Exception("Worker not found."); // Handle worker not existing
			}

			// Check if the old password matches
			if (worker.Password != oldPassword)
			{
				throw new Exception("Old password is incorrect.");
			}


			// Update the worker's password directly
			worker.Password = newPassword;

			// Call the existing update method to save changes
			Update(worker);

			return true; // Indicate successful password change
		}
	}
}
