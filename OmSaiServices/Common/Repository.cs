using OmSaiEnvironment;
//using EraInfosoft.Areas.Identity.Data;
using OmSaiModels.Admin;
using LmsServices.Common;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Common
{
	public class Repository<T> : IRepository<T> where T : class
	{

		public int Create(T entity, string procedureName, List<KeyValuePair<string, object>> parameters, string outputParameterName= "@LastInsertedId")
		{
			// Add a @type parameter manually
			parameters.Add(new KeyValuePair<string, object>("@Type", "INSERT"));

			return QueryService.NonQuery(procedureName, parameters, outputParameterName);
		}

		public void Update(T entity, string procedureName, List<KeyValuePair<string, object>> parameters)
		{
			string outputParameterName = "@LastInsertedId";
			// Add a @type parameter manually
			parameters.Add(new KeyValuePair<string, object>("@Type", "UPDATE"));			
			QueryService.NonQuery(procedureName, parameters, outputParameterName);
		}

		public void Delete(string procedureName, List<KeyValuePair<string, object>> parameters)
		{
			string outputParameterName = "@LastInsertedId";
			// Add a @type parameter manually
			parameters.Add(new KeyValuePair<string, object>("@Type", "DELETE"));

			QueryService.NonQuery(procedureName, parameters, outputParameterName);
		}

		public void Restore(string procedureName, List<KeyValuePair<string, object>> parameters)
		{
			string outputParameterName = "@LastInsertedId";
			// Add a @type parameter manually
			parameters.Add(new KeyValuePair<string, object>("@Type", "RESTORE"));
			QueryService.NonQuery(procedureName, parameters, outputParameterName);
		}

		public List<T> GetAll(string procedureName, Func<IDataReader, T> mapEntity, params SqlParameter[] parameters)
		{
			return QueryService.Query(procedureName, mapEntity, parameters);
		}

		public T GetById(int id, string procedureName, Func<IDataReader, T> mapEntity, params SqlParameter[] parameters)
		{
			var result = QueryService.Query(procedureName, mapEntity, parameters);
			return result?.FirstOrDefault();
		}


	}
}
