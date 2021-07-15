using System;
using System.Linq;
using Audit.Core.Context;
using Audit.Core.Events;
using Audit.Core.Events.EventHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tornado.Shared.AuditLogEvent;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.Extensions;

namespace Audit.Api
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
            services.AddSwagger("Audit Api");
            services.AddControllers();

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

            ConfigureEventBus(app);
        }

        public IServiceCollection AddEntityFrameworkDbContext(IServiceCollection services)
        {
            string dbConnStr = Configuration.GetConnectionString("Default");

            services.AddDbContextPool<GigAuditContext>(options =>
            {
                options.UseSqlServer(dbConnStr,
                 b => b.MigrationsAssembly(typeof(GigAuditContext).FullName));
            });

            return services;
        }

        protected virtual void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app?.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<AuditLogIntegrationEvent, AuditLogEventHandler>();
        }
    }
}
