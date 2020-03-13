using Application.IoC.Services;
using Infrastructure.DBConfiguration;
using Infrastructure.DBConfiguration.Dapper;
using Infrastructure.IoC;
using Infrastructure.IoC.ORMs.EFCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplication
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
            IConfigurationSection dbConnectionSettings = DatabaseConnection.ConnectionConfiguration.GetSection("ConnectionStrings");

            services.Configure<DataSettings>(dbConnectionSettings);

            //EF Core
            //services.ApplicationServicesIoC();
            //services.InfrastructureORM<EntityFrameworkIoC>();

            //Dapper
            services.ApplicationServicesIoC();
            services.InfrastructureORM<DapperIoC>();

            services.InfrastructureORM<EntityFrameworkIoC>();

            services.InfrastructureORM<DapperIoC>();

            services.AddControllersWithViews();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=User}/{action=Index}/{id?}");
            });
        }
    }
}
