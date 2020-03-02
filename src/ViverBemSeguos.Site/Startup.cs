using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ViverBemSeguos.Site.Data;
using AutoMapper;
using ViverBem.Infra.CrossCutting.IoC;
using ViverBem.Infra.CrossCutting.Identity.Models;
using ViverBem.Infra.CrossCutting.Identity.Services;
using Microsoft.AspNetCore.Http;
using ViverBem.Infra.CrossCutting.Bus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Elmah.Io.Extensions.Logging;

namespace ViverBemSeguos.Site
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddAuthorization(options => {

                options.AddPolicy("PodeTudo", policy => policy.RequireClaim("Administrador"));
                options.AddPolicy("PodeAprovarLerCadastrar", policy => policy.RequireClaim("Aprovador"));
                options.AddPolicy("PodeGerarProposta", policy => policy.RequireClaim("Digitador"));

            });
            services.AddLogging();
            services.AddMvc(options => {});
            services.AddAutoMapper();

            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHttpContextAccessor accessor, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/erro-de-aplicacao");
                app.UseStatusCodePagesWithReExecute("/erro-de-aplicacao/{0}");
            }

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();


            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            InMemoryBus.ContainerAcessor = () => accessor.HttpContext.RequestServices;
        }
        private static void RegisterServices(IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}
