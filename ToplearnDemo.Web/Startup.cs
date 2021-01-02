using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToplearnDemo.DataLayer.Context;
using ToplearnDemo.Repository.Repository;
using ToplearnDemo.Repository.Service;
using AutoMapper;
using ToplearnDemo.Repository.Mappers;
using Microsoft.AspNetCore.Authentication.Cookies;
using ToplearnDemo.Utility.Convertors;
using ToplearnDemo.Utility.Helpers.Interface;
using ToplearnDemo.Utility.Helpers;

namespace ToplearnDemo.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddAuthorization();
            services.AddSession(x => x.Cookie.IsEssential = true);
            services.AddDbContext<TopleranDemoContext>(options=>{
                options.UseSqlServer(_configuration.GetConnectionString("ToplaernDemoContextConnectionString"));
            });
            
            #region DI
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(Mapping));
            services.AddScoped<IViewRenderService, RenderViewToString>();
            services.AddScoped<IMessageSender, MessageSender>();
            #endregion

            #region Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options=> {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
            });
            #endregion
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                      name: "areas",
                      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                });
                endpoints.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");


            });
        }
    }
}
