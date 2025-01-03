using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
	public class QualificationModel
	{
		[Key]
		public int? QualificationId { get; set; }

		[Required]
		public string QualificationName { get; set; }

		[Required]
        public bool Status { get; set; } = true;

    }
}
