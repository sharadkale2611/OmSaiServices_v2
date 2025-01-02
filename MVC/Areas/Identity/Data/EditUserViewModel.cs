namespace GeneralTemplate.Areas.Identity.Data
{
	public class EditUserViewModel
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public List<string>? Roles { get; set; }
		public List<string> SelectedRoles { get; set; } // A list of selected roles
	}
}
