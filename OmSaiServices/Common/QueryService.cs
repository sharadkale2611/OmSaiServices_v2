using OmSaiEnvironment;
using Microsoft.Data.SqlClient;
using System.Data;
	
namespace LmsServices.Common
{
	public static class QueryService
	{

        public static int NonQuery(string procedure, List<KeyValuePair<string, object>> parameters, string outputParamName = null)
		{
			string connString = DBConnection.DefaultConnection;

			using (SqlConnection connection = new SqlConnection(connString))
			{
				using (SqlCommand command = new SqlCommand($"{procedure}", connection))
				{
					command.CommandType = CommandType.StoredProcedure;
					foreach (var param in parameters)
					{
						command.Parameters.Add(new SqlParameter(param.Key, param.Value));
					}

                    // Add output parameter for the primary key (if provided)
                    if (!string.IsNullOrEmpty(outputParamName))
                    {
                        var outputParam = new SqlParameter(outputParamName, SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);
                    }

                    connection.Open();
					
					command.ExecuteNonQuery();

                    // If there is an output parameter, return its value (e.g., last inserted primary key)
                    if (!string.IsNullOrEmpty(outputParamName) && command.Parameters[outputParamName].Value != DBNull.Value)
                    {
                        return (int)command.Parameters[outputParamName].Value;
                    }

                    return 0; // Return 0 if no output parameter was used or no primary key is returned

                }
            }
		}

		public static List<T> Query<T>(string spName, Func<SqlDataReader, T> mapFunction, params SqlParameter[] parameters)
		{
			string connString = DBConnection.DefaultConnection;

			List<T> results = new List<T>();
			using (SqlConnection con = new SqlConnection(connString))
			{
				con.Open();
				using (SqlCommand cmd = new SqlCommand(spName, con))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					// Add parameters to command
					if (parameters != null)
					{
						cmd.Parameters.AddRange(parameters);
					}

					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							// Map each row to the desired model using the provided mapping function
							T item = mapFunction(dr);
							results.Add(item);
						}
					}
				}
			}
			return results;
		}

		public static List<Dictionary<string, object>> Query(string spName, params SqlParameter[] parameters)
		{
			string connString = DBConnection.DefaultConnection;
			var results = new List<Dictionary<string, object>>();

			using (SqlConnection con = new SqlConnection(connString))
			{
				con.Open();
				using (SqlCommand cmd = new SqlCommand(spName, con))
				{
					cmd.CommandType = CommandType.StoredProcedure;

					// Add parameters to command
					if (parameters != null)
					{
						cmd.Parameters.AddRange(parameters);
					}

					using (SqlDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							var row = new Dictionary<string, object>();
							for (int i = 0; i < dr.FieldCount; i++)
							{
								row[dr.GetName(i)] = dr.GetValue(i);
							}
							results.Add(row);
						}
					}
				}
			}
			return results;
		}


	}
}
