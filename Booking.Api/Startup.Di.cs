using Booking.Core.Context;
using Booking.Core.Services;
using Booking.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Repository;
using Tornado.Shared.IntegrationEventLogEF.Services;
using Tornado.Shared.Utils;
using Wallet.Core.BackGroundJobs;
using Polly;
using Booking.Core.Helpers;
using System.Net.Http;
using System;
using static System.Net.HttpStatusCode;
using Polly.Extensions.Http;

namespace Booking.Api
{
    public partial class Startup
    {
        public static void ConfigureDIService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpServices();

            services.AddTransient<IRouteHikeService, RouteHikeService>();
            services.AddSingleton<IGuidGenerator>((s) => SequentialGuidGenerator.Instance);
            services.AddTransient<ICaptainService, CaptainService>();
            services.AddTransient<IBusBoyService, BusBoyService>();
            services.AddTransient<ICountryService, CountryService>();
            services.AddTransient<IStateService, StateService>();
            services.AddTransient<IPickupPointService, PickupPointService>();
            services.AddTransient<IRoutePickupPointService, RoutePickupPointService>();
            services.AddTransient<IVehicleMakeService, VehicleMakeService>();
            services.AddTransient<IVehicleModelService, VehicleModelService>();
            services.AddTransient<IVehicleService, VehicleService>();
            services.AddTransient<IRouteService, RouteService>();
            services.AddTransient<IVehicleExcludedSeatService, VehicleExcludedSeatService>();
            services.AddTransient<ISubRouteFeeService, SubRouteFeeService>();

            services.AddTransient<ITripService, TripService>();
            services.AddTransient<ITripDaysService, TripDaysService>();

            services.AddTransient<ITripManagementService, TripManagementService>();
            services.AddTransient<IScheduledTripService, ScheduledTripService>();
            services.AddTransient<IExternalBookingDetailsService, ExternalBookingDetailsService>();

            services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService>();

            //services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<IVehicleModelRouteFeeService, VehicleModelRouteFeeService>();

            services.AddTransient<IBookingConfigService, BookingConfigService>();
            services.AddTransient<IBookingEventService, BookingEventService>();
            services.AddTransient<IAreaService, AreaService>();

            services.AddHostedService<IntEventLogsPublishingTask>();


            services.AddScoped<IDbContext, GigBookingContext>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));

            services.AddScoped<IDbConnection>(db => new SqlConnection(
              configuration.GetConnectionString("Default")));

            services.AddScoped(typeof(Tornado.Shared.Dapper.Interfaces.IUnitOfWork), typeof(Tornado.Shared.Dapper.Repository.UnitOfWork));
            services.AddScoped(typeof(Tornado.Shared.Dapper.Interfaces.IDapperRepository<>), typeof(Tornado.Shared.Dapper.Repository.DapperRepository<>));
        
        
        }
    }


    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpServices(this IServiceCollection services)
        {
            services.AddTransient<TokenHttpHandler>();


            services.AddHttpClient<IBookingService, BookingService>()
               .AddHttpMessageHandler<TokenHttpHandler>()
               .AddPolicyHandler(GetRetryPolicy())
               .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()
              .OrResult(msg => msg.StatusCode == NotFound)
              .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}