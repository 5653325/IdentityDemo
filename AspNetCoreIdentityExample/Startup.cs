using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreIdentityExample.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreIdentityExample
{
    public class Startup
    {

        private IConfiguration configuration;

        public Startup(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseSqlServer(configuration.GetSection("ConnectionString").Value, opt =>
                {
                    //opt.CommandTimeout(6000);
                });
            });

            services.AddIdentity<IdentityUser, IdentityRole>(config=> {
                //注册时候会提示密码不符合强度要求,需要在此进行设置。隐藏的小BUG
                //config.Password.RequireDigit = false;
                //config.Password.RequireLowercase = false;
                //config.Password.RequireUppercase = false;
                //config.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = ".NetCoreIdentity.Cookies";
                config.LoginPath = "/Home/Login"; //让没有Cookie的用户访问被保护的接口时候跳转到这个Api
            });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
