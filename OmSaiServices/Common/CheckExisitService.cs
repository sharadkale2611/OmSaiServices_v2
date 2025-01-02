using OmSaiModels.Common;
using LmsServices.Common;
using Microsoft.Data.SqlClient;
using System.Data;

namespace OmSaiServices.Common
{
	public static class CheckExisitService
	{
		public static List<Dictionary<string, object>> Record(string tableName, string PrimaryKeyColumn, List<ColumnValuePairModel> conditions)
		{
			var conditionsTable = new DataTable();
			conditionsTable.Columns.Add("ColumnName", typeof(string));
			conditionsTable.Columns.Add("Value", typeof(string));

			// Fill the DataTable with the conditions
			foreach (var condition in conditions)
			{
				conditionsTable.Rows.Add(condition.ColumnName, condition.Value);
			}

			List<Dictionary<string, object>> data =  QueryService.Query(
				"sp_CheckExist_TableRecords",
				new SqlParameter("@tableName1", tableName),
				new SqlParameter("@PrimaryKeyColumn", PrimaryKeyColumn),			
				new SqlParameter("@conditions", conditionsTable)
			);

			return data;
		}
	}
}
