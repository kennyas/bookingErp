using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using Tornado.Shared.AspNetCore.Filters;
using Tornado.Shared.Extensions;
using Wallet.Core.Context;

namespace Wallet.Api
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSwagger("Wallet Api");
            services.AddControllers();

            services.AddScoped<GIGAuditLogAttribute>();

            AddEntityFrameworkDbContext(services);
            ConfigureIdentity(services);
            ConfigureDIService(services);

            services.AddCurrentUserPermissionHandler();

            ConfigureIntegrationService(services, Configuration);

        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; set; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

            //ConfigureEventBus(app);
        }

        public IServiceCollection AddEntityFrameworkDbContext(IServiceCollection services)
        {
            string dbConnStr = Configuration.GetConnectionString("Default");

            services.AddDbContextPool<GigWalletContext>(options =>
            {
                options.UseSqlServer(dbConnStr,
                 b => b.MigrationsAssembly(typeof(GigWalletContext).FullName));
            });

            return services;
        }
       
    }
}

