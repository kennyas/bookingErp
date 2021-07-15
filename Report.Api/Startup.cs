using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Report.Core;
using Report.Core.Context;
using Report.Core.Event.EventHandler;
using System;
using System.Linq;
using System.Reflection;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.Extensions;

namespace Report.Api
{
    public partial class Startup
    {
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment HostingEnvironment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            HostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSwagger("Reporting Service");
            services.AddControllers();

            AddEntityFrameworkDbContext(services);
            ConfigureIdentity(services);
            services.AddCurrentUserPermissionHandler();

            AddAzureSignalRMiddleware(services);

            ConfigureDIService(services, Configuration);
            ConfigureIntegrationService(services, Configuration);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
             .AllowAnyHeader()
              .SetIsOriginAllowed((x) => true)
             .AllowCredentials();
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

            ConfigureAzureSignalRRoutes(app);
            ConfigureEventBus(app);
        }

        public IServiceCollection AddEntityFrameworkDbContext(IServiceCollection services)
        {
            string dbConnStr = Configuration.GetConnectionString("Default");

            services.AddDbContext<GigReportContext>(options =>
            {
                options.UseSqlServer(dbConnStr,
                 b =>
                 {
                     b.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                     //b.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                 });
            });

            return services;
        }

        protected virtual void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app?.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<BookingCreatedIntegrationEvent, BookingCreatedEventHandler>();
            eventBus.Subscribe<BookingCompletedIntegrationEvent, BookingCompletedventHandler>();
            eventBus.Subscribe<BookingCancelledIntegrationEvent, BookingCancelledEventHandler>();
        }
    }
}
