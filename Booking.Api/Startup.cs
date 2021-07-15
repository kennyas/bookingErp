using Booking.Core.Context;
using Booking.Core.Events;
using Booking.Core.Events.EventHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.Extensions;
using static Booking.Core.Helpers.CoreConstants;

[assembly: ApiController]
namespace Booking.Api
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
            services.AddSwagger("Booking Service");
            services.AddControllers();


            services.Configure<GigLogisticsUrlConfig>(options =>
                 Configuration.GetSection(nameof(GigLogisticsUrlConfig)).Bind(options));

            services.Configure<GigMobilityUrlConfig>(options =>
                Configuration.GetSection(nameof(GigMobilityUrlConfig)).Bind(options));

            AddEntityFrameworkDbContext(services);
            ConfigureIdentity(services);
            services.AddCurrentUserPermissionHandler();
            ConfigureDIService(services, Configuration);

            ConfigureIntegrationService(services, Configuration);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379";
                options.InstanceName = "";
                //options.Configuration = Configuration.GetConnectionString("RedisConnectionString"); ;
                //options.InstanceName = "gigredis";
            });
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment HostingEnvironment { get; set; }

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



            services.AddDbContext<GigBookingContext>(options =>
            {
                options.UseSqlServer(dbConnStr,
                 b =>
                 {
                     b.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                     //b.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                 });
            });

            services.AddDbContext<BookingIntegrationLogContext>(options =>
            {
                options.UseSqlServer(dbConnStr,
                 b =>
                 {
                     b.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                     b.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                 });
            });

            return services;
        }

        protected virtual void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app?.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<UserCreatedIntegrationEvent, UserCreatedEventHandler>();
            eventBus.Subscribe<PaymentSucceededIntegrationEvent, PaymentSucceededEventHandler>();
        }
    }
}
