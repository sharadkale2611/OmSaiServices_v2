using OmSaiModels.Worker;
using OmSaiServices.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Worker.Interfaces
{
	public interface IWorkerDocumentService : IRepository<WorkerDocumentModel>
	{
		public void VerifyDocument(int id,bool IsVerified);
		
	}
}
