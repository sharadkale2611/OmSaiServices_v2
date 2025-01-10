using OmSaiModels.Worker;
using OmSaiServices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Worker.Interfaces
{
	public interface IWorkerService : IRepository<WorkerModel>
	{
		public Task<WorkerModel> Login(string workmanId, string password);
		bool ChangePassword(int WorkerId, string oldPassword, string newPassword);

	}
}
