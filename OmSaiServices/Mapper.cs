using OmSaiModels.Admin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices
{
	public class Mapper
	{
		// Generic method to map data from IDataReader to any model
		public T MapEntity<T>(IDataReader reader) where T : class, new()
		{
			var entity = new T();

			var properties = typeof(T).GetProperties();
			foreach (var property in properties)
			{
				// Skip properties with the IgnoreProperty attribute
				//if (Attribute.IsDefined(property, typeof(IgnorePropertyAttribute)))
				//	continue;

				//var columnValue = reader[property.Name];
				//var columnValue = reader.GetSchemaTable()
				//						.Rows
				//						.Cast<DataRow>()
				//						.FirstOrDefault(row => row["ColumnName"].ToString().Equals(property.Name, StringComparison.OrdinalIgnoreCase));


				// Check if column value is not DBNull
				if (!reader.IsDBNull(reader.GetOrdinal(property.Name))) // Check if the column value is not DBNull
				{
					var columnValue = reader[property.Name]; // Retrieve the actual column value

					var propertyType = property.PropertyType;

					// Handle Nullable types
					if (Nullable.GetUnderlyingType(propertyType) != null)
					{
						propertyType = Nullable.GetUnderlyingType(propertyType);
					}

					// Auto-conversion based on property type
					if (propertyType == typeof(int))
					{
						property.SetValue(entity, Convert.ToInt32(columnValue));
					}
					else if (propertyType == typeof(string))
					{
						property.SetValue(entity, columnValue.ToString());
					}
					else if (propertyType == typeof(bool))
					{
						property.SetValue(entity, Convert.ToBoolean(columnValue));
					}
					else if (propertyType == typeof(DateTime?))
					{
						property.SetValue(entity, columnValue as DateTime?);
					}
					else if (propertyType == typeof(DateTime))
					{
						property.SetValue(entity, Convert.ToDateTime(columnValue));
					}
					else if (propertyType == typeof(DateOnly))
					{
						property.SetValue(entity, DateOnly.FromDateTime(Convert.ToDateTime(columnValue)));
					}
					else if (propertyType == typeof(DateOnly?))
					{
						var dateValue = columnValue as DateTime?;
						property.SetValue(entity, dateValue.HasValue ? DateOnly.FromDateTime(dateValue.Value) : null);
					}
					else if (propertyType == typeof(TimeSpan?))
					{
						property.SetValue(entity, columnValue as TimeSpan?);
					}
					else
					{
						// For other types (e.g., decimal, float, etc.)
						property.SetValue(entity, Convert.ChangeType(columnValue, propertyType));
					}
				}
				else
				{
					// Set the property to null if DBNull
					property.SetValue(entity, null);
				}
			}

			return entity;
		}
	}
}
