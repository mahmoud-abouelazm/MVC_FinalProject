using Library.Web.Core.Models;
using Library.Web.Data;
using Library.Web.Repository.IRepositories;
using Library.Web.Repository.Repositories;
using Library.Web.Services;
using Library.Web.Services.HelperServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration
                .GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found.");


            //var shadyConnection = builder.Configuration
            //    .GetConnectionString("shadyConnection")
            //    ?? throw new InvalidOperationException("Connection string not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IBookRepository, BooksRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<IAuthorService, AuthorService>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICopyRepository, CopyRepository>();
            builder.Services.AddScoped<ICopyService, CopyService>();
            builder.Services.AddScoped<IRentalRepository, RentalRepository>();
            builder.Services.AddScoped<IRentalService, RentalService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();    



            //builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
            //    options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
              .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();  
            

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Prevents client-side scripts (JS) from accessing the cookie
                options.Cookie.HttpOnly = true;

                // Only sends the cookie over HTTPS
                // Use CookieSecurePolicy.Always for production
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                // Helps prevent Cross-Site Request Forgery (CSRF)
                options.Cookie.SameSite = SameSiteMode.Strict;

                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = "/identity/Account/Login"; // redirect here if not logged in
                options.AccessDeniedPath = "/identity/Account/Login";
                options.SlidingExpiration = true;
                
            });

            builder.Services.AddControllersWithViews();
			builder.Services.AddRazorPages();

			var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapStaticAssets();
            app.MapRazorPages();

            app.Run();
        }
    }
}