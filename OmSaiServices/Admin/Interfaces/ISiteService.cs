using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OmSaiModels.Admin;
using OmSaiServices.Common;

namespace OmSaiServices.Admin.Interfaces
{
    public interface ISiteService:IRepository<SiteModel>
    {
		// Add any branch-specific methods if needed
	}
}
