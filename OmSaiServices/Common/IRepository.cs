using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Common
{
	public interface IRepository<T> where T : class
	{

		int Create(T entity, string procedureName, List<KeyValuePair<string, object>> parameters, string outputParameterName);
		void Update(T entity, string procedureName, List<KeyValuePair<string, object>> parameters);
		void Delete(string procedureName, List<KeyValuePair<string, object>> parameters);
		void Restore(string procedureName, List<KeyValuePair<string, object>> parameters);
		List<T> GetAll(string procedureName, Func<IDataReader, T> mapEntity, params SqlParameter[] parameters);
		T GetById(int id, string procedureName, Func<IDataReader, T> mapEntity, params SqlParameter[] parameters);

	}
}
