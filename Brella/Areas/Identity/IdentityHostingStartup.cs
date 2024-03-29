﻿using System;
using Data.Context;
using Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Brella.Areas.Identity.IdentityHostingStartup))]
namespace Brella.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddDbContext<DBbrella>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DBbrellaConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<DBbrella>();


                services.Configure<IdentityOptions>(x =>
                {
                    x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                    x.Password.RequireDigit = true;
                    x.Password.RequireLowercase = false;
                    x.Password.RequireUppercase = false;
                    x.Password.RequireNonAlphanumeric = false;
                    x.Password.RequiredUniqueChars = 0;
                });
            });
        }
    }
}
