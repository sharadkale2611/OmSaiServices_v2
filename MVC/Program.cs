using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GeneralTemplate.Areas.Identity.Data;
using Microsoft.AspNetCore.Rewrite;
using OmSaiEnvironment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using LmsServices.Common;
using OmSaiServices.Worker.Implementations;
using OmSaiServices.Worker.Implimentation;
using Microsoft.AspNetCore.Mvc;
using GeneralTemplate.Filter;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(DBConnection.DefaultConnection));


//// JWT Authentication setup
builder.Services.AddAuthentication(options =>
{
	// Set default scheme to Identity (if you use it in your app)
	options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
	options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
.AddJwtBearer("Jwt", options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
		ValidAudience = builder.Configuration["JwtSettings:Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])) // Use the key from appsettings.json
	};
});

builder.Services.AddControllers(options =>
{
	options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; // Suppress automatic validation
});


// Default API Model validation response customized
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.InvalidModelStateResponseFactory = context =>
	{
		var errors = context.ModelState
			.Where(x => x.Value.Errors.Count > 0)
			.ToDictionary(
				kvp => kvp.Key,
				kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
			);

		return new BadRequestObjectResult(new
		{
			success = false,
			data = (object)null,
			errors = errors
		});
	};
});



// Set default authentication scheme for Identity
// Register Identity services (with role support)
builder.Services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
	.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.AddScoped<UserManager<AppUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();

//builder.Services.AddControllersWithViews(options =>
//{
//	options.Filters.Add<WorkerAuthorizeFilter>();
//}).AddRazorRuntimeCompilation();


//builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options =>
{
	options.FallbackPolicy = null; // Allow unauthenticated access by default
});


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
		options.IdleTimeout = TimeSpan.FromMinutes(30);
		options.Cookie.HttpOnly = true;
		options.Cookie.IsEssential = true;
		options.Cookie.SameSite = SameSiteMode.None; // Allows cross-origin requests
		options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensures cookies are sent over HTTPS
	});


// Register CORS service
builder.Services.AddCors(options =>
{
	//options.AddPolicy("AllowMyApp", policy =>
	//{
	//	policy.WithOrigins("http://localhost:3000", "file://").AllowAnyMethod().AllowAnyHeader();
	//});

	options.AddPolicy("AllowAllOrigins", policy =>
	{
		policy.AllowAnyOrigin()      // Allows all origins
			  .AllowAnyMethod()     // Allows all HTTP methods (GET, POST, etc.)
			  .AllowAnyHeader();    // Allows all headers
	});
});

builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.SameSite = SameSiteMode.None; // Allows cross-origin requests
	options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Ensures cookies are sent over HTTPS
	options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect path for access denied
});



var app = builder.Build();



// Configure the HTTP request pipeline.
app.UseCors("AllowAllOrigins");  // Use the CORS policy


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


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

app.UseRouting();

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
