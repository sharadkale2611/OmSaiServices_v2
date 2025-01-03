namespace GeneralTemplate.Areas.Identity.Data
{
	public class UserRolesViewModel
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public List<string> Roles { get; set; }
	}
}
