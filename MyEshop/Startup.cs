using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyEshop.Data;
using MyEshop.Data.Repositories;

namespace MyEshop
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages();


            #region Db Context

            services.AddDbContext<MyEshopContext>(options =>
                { options.UseSqlServer("Data Source =.;Initial Catalog=EshopCore_DB;Integrated Security=true"); });

            #endregion

            #region IoC

            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            //services.AddSingleton<IGroupRepository, GroupRepository>();
            //services.AddTransient<IGroupRepository, GroupRepository>();

            #endregion

            #region Authentication

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Account/Login";
                    option.LogoutPath = "/Account/Logout";
                    option.ExpireTimeSpan = TimeSpan.FromDays(10);
                });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            // sathe dastresi kharbarn aadi be panel modiriat ra taeen mokonim
            // be bastare context bro va baad...
            app.Use(async (context, next) =>
            {
                // agar masire darkhast az bastar ba Admin shoroe shode bood
                if (context.Request.Path.StartsWithSegments("/Admin"))
                {
                    // va tebghe Claims ke rooye modele Userzade bodim, aga karbar login nabood
                    if (!context.User.Identity.IsAuthenticated)
                    {
                        // karbar ra be safheye Login az controller Account befrest
                        context.Response.Redirect("/Account/Login");

                    }
                    // va ya karbar Admin nabood
                    else if (!bool.Parse(context.User.FindFirstValue("IsAdmin")))
                    {
                        // karbar ra be safheye Login az controller Account befrest
                        context.Response.Redirect("/Account/Login");
                    }
                    // darsoorati 2 shart bala barghara naboodand karbar mitavanad be Panel Admin beravad
                }
                await next.Invoke();
                });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
