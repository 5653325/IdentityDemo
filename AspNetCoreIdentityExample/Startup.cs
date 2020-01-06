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
                //ע��ʱ�����ʾ���벻����ǿ��Ҫ��,��Ҫ�ڴ˽������á����ص�СBUG
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
                config.LoginPath = "/Home/Login"; //��û��Cookie���û����ʱ������Ľӿ�ʱ����ת�����Api
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
