using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using Tornado.Shared.Extensions;
using UserManagement.Core.Context;

[assembly: ApiController]
namespace UserManagement.Api
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment HostingEnvironment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSwagger("Usermanagement Service");
            services.AddControllers();

            AddEntityFrameworkDbContext(services);
            ConfigureIdentity(services);
            AddIdentityProvider(services);
            ConfigureDIService(services);

            services.AddCurrentUserPermissionHandler();

            ConfigureIntegrationService(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                InitializeIdentityDbAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();
            }

            app.UseStaticFiles();

            app.UseCors(x =>
            {
                x.WithOrigins(Configuration["AllowedCorsOrigin"]
                  .Split(",", StringSplitOptions.RemoveEmptyEntries)
                  .Select(o => o.RemovePostFix("/"))
                  .ToArray())
             .AllowAnyMethod()
             .AllowAnyHeader();
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseCustomSwaggerApi();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                .RequireAuthorization();
            });

        }

        public IServiceCollection AddEntityFrameworkDbContext(IServiceCollection services)
        {
            string dbConnStr = Configuration.GetConnectionString("Default");

            services.AddDbContextPool<GigAuthDbContext>(options =>
            {
                options.UseSqlServer(dbConnStr,
                 b => b.MigrationsAssembly(typeof(GigAuthDbContext).FullName));
            });

            return services;
        }
    }
}