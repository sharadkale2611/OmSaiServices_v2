using OmSaiModels.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiServices.Worker.Interfaces
{
	public interface IWorkerAttendanceService
	{
		public void ManageAttendance(WorkerAttendanceModel model);
		public List<WorkerAttendanceViewModel> GetAll(int? WorkerId, int? SiteId, DateOnly? CurrentDate, string? WorkmanId, int? RecordCount);
	}
}
