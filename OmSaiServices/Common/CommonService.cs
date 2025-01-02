using OmSaiModels.Common;
using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LmsServices.Common
{
    public static class CommonService
    {

        private static readonly char[] AllowedChars =
              "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+[]{}|;:,.<>?".ToCharArray();

        public static string GenerateRandomPassword(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Password length must be greater than zero.");

            StringBuilder password = new StringBuilder(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(AllowedChars.Length);
                password.Append(AllowedChars[index]);
            }

            return password.ToString();
        }


		public static int RowCount(string tableName)
		{
			// Declare the output parameter
			var countParameter = new SqlParameter("@Count", SqlDbType.Int)
			{
				Direction = ParameterDirection.Output
			};

			QueryService.Query(
				"sp_GetCount_tables",
				new SqlParameter("@TableName", tableName),
				countParameter
			);

			// Retrieve and return the value from the output parameter
			return (int)countParameter.Value;
		}


		public static int RowCountFirmUser(int firmId)
        {
            // Declare the output parameter
            var countParameter = new SqlParameter("@Count", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            QueryService.Query(
				"sp_GetCount_FirmUsers",
                new SqlParameter("@FirmId", firmId),
                countParameter
            );

            // Retrieve and return the value from the output parameter
            return (int)countParameter.Value;
        }

        public static List<int> SplitInstallments(int totalAmount, int numberOfInstallments)
        {
            List<int> installments = new List<int>();
            int baseInstallment = totalAmount / numberOfInstallments;
            int remainder = totalAmount % numberOfInstallments;

            for (int i = 0; i < numberOfInstallments; i++)
            {
                if (i == numberOfInstallments - 1)
                {
                    installments.Add(baseInstallment + remainder);
                }
                else
                {
                    installments.Add(baseInstallment);
                }
            }

            return installments;
        }


    }
}
