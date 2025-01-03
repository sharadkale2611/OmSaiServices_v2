using Microsoft.AspNetCore.Identity;
using OmSaiServices.Admin.Implementations;

namespace GeneralTemplate.Areas.Identity.Data
{
	public class ApplicationSeeder
	{
		private static readonly DocumentService _documentService = new DocumentService();


		public static async Task SeedDocuments(IServiceProvider serviceProvider)
		{

			var model = new OmSaiModels.Admin.DocumentModel { DocumentName = "Passport Photo", Status = true };
			_documentService.Create(model);

			model = new OmSaiModels.Admin.DocumentModel { DocumentName = "Aadhar Card", Status = true };
			_documentService.Create(model);

			model = new OmSaiModels.Admin.DocumentModel { DocumentName = "Pan Card", Status = true };
			_documentService.Create(model);

			model = new OmSaiModels.Admin.DocumentModel { DocumentName = "Education Proof", Status = true };
			_documentService.Create(model);
		}


		public static async Task SeedRolesAndUsers(IServiceProvider serviceProvider)
		{
			// Get RoleManager and UserManager
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

			// Define roles
			string[] roleNames = { "Admin", "Staff" };

			// Create roles
			foreach (var roleName in roleNames)
			{
				if (!await roleManager.RoleExistsAsync(roleName))
				{
					await roleManager.CreateAsync(new IdentityRole(roleName));
				}
			}

			// Create SuperAdmin user
			//var name = "Admin User";
			var adminEmail = "admin@oss.com";
			var adminPassword = "Admin@123";
				
			var adminUser = await userManager.FindByEmailAsync(adminEmail);
			if (adminUser == null)
			{
				adminUser = new AppUser
				{
					UserName = adminEmail,
					Email = adminEmail,
					EmailConfirmed = true
				};

				var createAdminResult = await userManager.CreateAsync(adminUser, adminPassword);
				if (createAdminResult.Succeeded)
				{
					await userManager.AddToRoleAsync(adminUser, "Admin");
				}
			}

			// ---------------------------------- staff ------------------

			var staffEmail = "staff@oss.com";
			var staffPassword = "Staff@123";

			var staffUser = await userManager.FindByEmailAsync(staffEmail);
			if (staffUser == null)
			{
				staffUser = new AppUser
				{
					UserName = staffEmail,
					Email = staffEmail,
					EmailConfirmed = true
				};

				var createstaffResult = await userManager.CreateAsync(staffUser, staffPassword);
				if (createstaffResult.Succeeded)
				{
					await userManager.AddToRoleAsync(staffUser, "Staff");
				}
			}


		}


	}
}
