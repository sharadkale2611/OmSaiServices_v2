using OmSaiModels.Worker;
using OmSaiServices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Worker.Interfaces
{
	public interface ILeaveRequestService : IRepository<LeaveRequestModel>
	{
		// Add your extra functions
		public void Approve(LeaveRequestApproveModel model);
	}
}
