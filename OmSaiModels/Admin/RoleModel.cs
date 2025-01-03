using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmSaiModels.Admin
{
	public class RoleModel
	{
		[Key]
		public string? Id { get; set; }
		public string Name { get; set; }
		public string? NormalizedName { get; set; }
	}
}
