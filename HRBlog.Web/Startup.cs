﻿using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HRBlog.Model.CustomModel;
using Common.DomainRouter;

namespace HRBlog.Web
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
            services.AddOptions();

            //配置数据库连接串
            services.Configure<MyOptions>(Configuration.GetSection("ConnectionStrings"));

            //AddCookie时,添加SchemeName,不添加时报错
            services.AddAuthentication("MyCookieAuthenticationScheme")
                .AddCookie("MyCookieAuthenticationScheme", options => {
                    options.AccessDeniedPath = "/Areas/Admin/Forbidden";
                    options.LoginPath = "/Areas/Admin/Login";
                    options.LogoutPath = "/Areas/Admin/Login";
                });

            // Add framework services.
            services.AddMvc();

            

            //添加gb2312的支持
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //添加身份认证
            app.UseAuthentication();
            app.UseSession();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                //网站域名,区域名,控制器名
                routes.MapDomainRoute("www.dayali.net", "Admin", "Login");

                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
