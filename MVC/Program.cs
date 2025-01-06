using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GeneralTemplate.Areas.Identity.Data;
using Microsoft.AspNetCore.Rewrite;
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

//builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<AppDbContext>();

// Register Identity services (with role support)
builder.Services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<AppUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();


// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddRazorPages()
	 .AddRazorPagesOptions(options =>
	 {
		 // Map Identity routes without the "Identity" prefix
		 options.Conventions.AddAreaPageRoute("Identity", "/Account/AccessDenied", "Account/AccessDenied");
		 options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "Account/Login");
		 options.Conventions.AddAreaPageRoute("Identity", "/Account/Register", "Account/Register");
		 options.Conventions.AddAreaPageRoute("Identity", "/Account/Logout", "Account/Logout");
		 options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/Index", "Account/Manage");
		 options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/Email", "Account/Manage/Email");
		 options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/ChangePassword", "Account/Manage/ChangePassword");
		 options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/TwoFactorAuthentication", "Account/Manage/TwoFactorAuthentication");
		 options.Conventions.AddAreaPageRoute("Identity", "/Account/Manage/PersonalData", "Account/Manage/PersonalData");
	 });


	// Add services to the container.
	builder.Services.AddDistributedMemoryCache(); // Enables in-memory session storage
	builder.Services.AddSession(options =>
	{
		options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
		options.Cookie.HttpOnly = true; // Security setting
		options.Cookie.IsEssential = true; // GDPR compliance
	});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Seed roles and users
/*
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	try
	{
		await ApplicationSeeder.SeedRolesAndUsers(services);
		//await ApplicationSeeder.SeedDocuments(services);		
	}
	catch (Exception ex)
	{
		var logger = services.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "An error occurred while seeding roles and users.");
	}
}
*/

app.MapRazorPages();

// Add URL Rewrite Rules
var rewriteOptions = new RewriteOptions()
	.AddRedirect(@"^Identity/(.*)", "$1"); // Redirect "Identity/..." to root
app.UseRewriter(rewriteOptions);

app.UseSession(); // Enable session middleware


app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
	  name: "areas",
	  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
	);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
