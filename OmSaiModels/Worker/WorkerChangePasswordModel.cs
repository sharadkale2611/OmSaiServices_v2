using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Worker
{
	public class WorkerChangePasswordModel
	{
		[Key]
		public int WorkerId { get; set; }
		[Required(ErrorMessage = "Please enter existing password")]
		public string OldPassword { get; set; }
		[Required(ErrorMessage = "Please enter new password")]

		public string NewPassword { get; set; }
		[Required(ErrorMessage = "Please enter confirm password")]
		[Compare("NewPassword", ErrorMessage = "New password and confirm password is not matched")]
		public string ConfirmPassword { get; set; }
	}
}
