using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiEnvironment
{
	public class DBConnection
	{
		//LocalConnection
		//public static string DefaultConnection = "Server=BOTMASTER-SRD\\MSSQLEXPRESS;Database=om_sai_services_db_v1;User Id=sa;Password=pass@1234;TrustServerCertificate=True";

		//ProdConnection
		//public static string DefaultConnection = "Server=115.124.106.98;Database=om_sai_services_db_v1;User Id=omuser;Password=SaiServices#2024;TrustServerCertificate=True";

		// TestConnection
		public static string DefaultConnection = "Server=A2NWPLSK14SQL-v04.shr.prod.iad2.secureserver.net;Database=om_sai_services_db_v1;User Id = omuser; Password=YSAAS#2024;TrustServerCertificate=True";
	}
}