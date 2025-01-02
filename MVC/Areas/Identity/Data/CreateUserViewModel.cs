using System.ComponentModel.DataAnnotations;

namespace GeneralTemplate.Areas.Identity.Data
{
	public class CreateUserViewModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[Display(Name = "Roles")]
		public List<string>? Roles { get; set; } // List of all roles

		[Display(Name = "Assigned Roles")]
		public List<string> SelectedRoles { get; set; } // List of selected roles
	}
}
