using Booking.Payment.HttpAggregator.core.Helpers;
using Booking.Payment.HttpAggregator.core.Services;
using Booking.Payment.HttpAggregator.Core.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Linq;
using System.Net.Http;
using Tornado.Shared.Extensions;
using static System.Net.HttpStatusCode;

[assembly: ApiController]
namespace Booking.Payment.HttpAggregator
{
    public partial class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddSwagger("Booking Aggregator");

            ConfigureJWT(services);

            services.Configure<WalletServiceUrlsConfig>(options =>
                 Configuration.GetSection(nameof(WalletServiceUrlsConfig)).Bind(options));

            services.Configure<BookingServiceUrlsConfig>(options =>
                 Configuration.GetSection(nameof(BookingServiceUrlsConfig)).Bind(options));

            services.AddCurrentUserPermissionHandler();
            services.AddHttpServices();
            services.AddControllers();

            ConfigureIntegrationService(services, Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

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
                endpoints.MapControllers();
            });
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpServices(this IServiceCollection services)
        {
            services.AddTransient<TokenHttpHandler>();

            //services.AddScoped<IPaymentEventService, PaymentEventService>();

            services.AddHttpClient<IBookingService, BookingService>()
               .AddHttpMessageHandler<TokenHttpHandler>()
               .AddPolicyHandler(GetRetryPolicy())
               .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<IWalletService, WalletService>()
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