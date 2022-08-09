using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ArizaApp.CustomValidations;
using ArizaApp.Extensions;
using ArizaApp.Models.DbContexts;
using ArizaApp.Models.Entities;
using ArizaApp.Models.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ArizaApp
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
            
            services.AddDbContext<ArizaDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration["ConnectionStrings:DefaultConnectionString"]);
            });

            services.LoadCustomServices();

            services.AddIdentity<AppUser, AppRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.User.AllowedUserNameCharacters =
                        "abcçdefghıijklmnoöpqrsştuüvwxyzABCDEFGHİIJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._";
                    options.Password.RequiredLength = 4;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                }).AddEntityFrameworkStores<ArizaDbContext>().AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            var cookieBuilder = new CookieBuilder
            {
                Name = "ArizaApp",
                HttpOnly = false,
                SameSite = SameSiteMode.Lax,
                SecurePolicy = CookieSecurePolicy.SameAsRequest
            };

            services.ConfigureApplicationCookie(opts =>
            {
                opts.LoginPath = new PathString("/Home/Login");
                opts.LogoutPath = new PathString("/Home/Logout");
                opts.Cookie = cookieBuilder;
                opts.SlidingExpiration = true;
                opts.ExpireTimeSpan = TimeSpan.FromDays(60);
                opts.AccessDeniedPath = new PathString("/Home/AccessDenied");
            });

            services.Configure<SeedObject>(Configuration.GetSection("DefaultAdminUser"));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.Use(async (context, next) =>
            {
                // TODO: just for test, middleware is not a good choice for db seeding, move them.
                var dbContext = context.RequestServices.GetService<ArizaDbContext>();
                var seedObject = app.ApplicationServices.GetRequiredService<IOptions<SeedObject>>().Value;
                await dbContext.Database.MigrateAsync();
                await SeedDb.InitializeRoles(context.RequestServices);
                await SeedDb.InitializeUser(context.RequestServices, seedObject);
                await next.Invoke();
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}