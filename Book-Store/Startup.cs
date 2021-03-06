﻿using BookStore.BusinessLogic.Init;
using BookStore.DataAccess.Initialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Book_Store.Middlewaren;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BookStore.BusinessLogic.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BookStore.Helper;
using BookStore.Controllers;
using BookStore.Helper.Interface;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace BookStore
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

            Initializer.Init(services, Configuration.GetConnectionString("DefaultConnection"));
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddTransient<IJwtHelper, JwtHelper>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/login");
                });

            services.AddAuthentication(options =>
            { 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = AuthOptions._issuer,
                            ValidateAudience = true,
                            ValidAudience = AuthOptions._audience,
                            ValidateLifetime = true,
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataBaseInitialization initializer, ILoggerFactory loggerFactory)
        {
            initializer.StartInit();
            app.UseMiddleware<ExceptionHandlerMiddleware>();    

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            //app.UseAuthorization();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
