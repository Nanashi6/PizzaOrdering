using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Models;

namespace PizzaOrdering
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<PizzeriaContext>(options =>
                options.UseSqlite("Data Source=./pizzeria.db"));

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<PizzeriaContext>()
                .AddDefaultTokenProviders();
            
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "API.Session";
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseSession();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            using (var scope = app.Services.CreateScope()) {
                var serviceProvider = scope.ServiceProvider;
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
    
                EnsureRoleExists("Admin", roleManager).Wait();
                EnsureRoleExists("User", roleManager).Wait();

                if (userManager.GetUsersInRoleAsync("Admin").Result.Count() == 0)
                {
                    User user = new User()
                    {
                        UserName = "admin@example.com",
                        Name = "Admin",
                        Surname = "Admin",
                        Email = "admin@example.com"
                    };
                    userManager.CreateAsync(user, "Password123*").Wait();
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

            app.Run();

            async Task EnsureRoleExists(string roleName, RoleManager<IdentityRole> roleManager) {
                if (!await roleManager.RoleExistsAsync(roleName)) {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
