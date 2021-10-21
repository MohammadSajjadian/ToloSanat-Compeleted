using Data.Context;
using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.EmailService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brella
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //  Connect to DataBase.
            services.AddDbContext<DBbrella>(x =>
            {
                x.UseSqlServer(Configuration.GetConnectionString("DBbrella"));
            });


            services.AddAuthorization(x =>
            {
                x.AddPolicy("AdminPolicy", p => p.RequireRole("admin"));
            });

            services.ConfigureApplicationCookie(x =>
            {
                x.LoginPath = "/account/login";
                x.AccessDeniedPath = "/account/login";
            });

            // Inject Repository.
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton<IEmail, Send>();

            services.AddAuthentication();
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            BrellaInit(userManager, roleManager).Wait();
        }

        private async Task BrellaInit(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var Roles = new List<string>() { "user", "admin" };
            foreach (var item in Roles)
            {
                var role = new IdentityRole(item);
                await roleManager.CreateAsync(role);
            }

            var user = await userManager.FindByNameAsync("Admin");
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = "mohammad.you41@gmail.com",
                    Email = "mohammad.you41@gmail.com",
                    name = "admin",
                    family = "",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user);
            }

            if (await userManager.IsInRoleAsync(user, "admin") == false)
            {
                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}
