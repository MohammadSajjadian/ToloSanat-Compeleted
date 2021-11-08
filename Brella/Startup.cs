using Data.Context;
using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.EmailService;
using Services.ResizeService;
using System;
using System.Collections.Generic;
using System.Globalization;
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


            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, x => x.ResourcesPath = "Resource")
                .AddDataAnnotationsLocalization(x =>
                {
                    x.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(ShareResource));
                });


            // Inject Repository and Services.
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddSingleton<IEmail, Send>();
            services.AddSingleton<IResize, ResizerImage>();

            services.AddLocalization(x => x.ResourcesPath = "Resources");
            services.AddAuthentication();
            services.AddSession();
            services.AddCloudscribePagination();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IRepository<Language> repository)
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


            // 404 Error Handler
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Home/Error";
                    await next();
                }
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseCookiePolicy();

            #region Locallize Settings

            var supportCulture = new List<CultureInfo>();
            foreach (var item in repository.Get(null, null, null))
            {
                supportCulture.Add(new CultureInfo(item.title));
            }

            var options = new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture("fa-IR"),
                SupportedCultures = supportCulture,
                SupportedUICultures = supportCulture,
                RequestCultureProviders =
                {
                    new QueryStringRequestCultureProvider(),
                    new CookieRequestCultureProvider()
                }
            };

            #endregion

            app.UseRequestLocalization(options);

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
                await userManager.CreateAsync(user, "pP_0987");
            }

            if (await userManager.IsInRoleAsync(user, "admin") == false)
            {
                await userManager.AddToRoleAsync(user, "admin");
            }
        }
    }
}
