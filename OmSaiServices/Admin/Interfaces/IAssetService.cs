using OmSaiModels.Admin;
using OmSaiServices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Admin.Interfaces
{
	public interface IAssetService: IRepository<AssetModel>
	{
		// Add any branch-specific methods if needed
	}
}
