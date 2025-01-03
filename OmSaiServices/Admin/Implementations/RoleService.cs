using OmSaiModels.Admin;
using OmSaiServices.Admin.Interfaces;
using OmSaiServices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Admin.Implementations
{
	public class RoleService : Repository<RoleModel>, IRoleService
	{
		private readonly string sp_cud;     // sp_CreateUpdateSoftDeleteRestore     
		private readonly string sp_r;       // sp_GetAll
		private readonly Mapper _mapper;    // Generic method to map data from IDataReader to any model

		public RoleService() {
			sp_cud = "sp_CreateUpdateDeleteRestore_Assets";
			sp_r = "sp_GetAll_Assets";
			_mapper = new Mapper();
		}

	}
}
