using AutoFacDI = Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Booking.Core.Context;
using Booking.Core.Utilities.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using Tornado.Shared.AspNetCore.Hosting;
using System.IO;

namespace Booking.Api
{
    public static class Program
    {

        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

            var configuration = GetConfiguration();

            var host =
                CreateHostBuilder(args , configuration).Build();
            try
            {
                host.MigrateDbContext<GigBookingContext>((context, services) =>
                {
                    var env = services.GetService<IWebHostEnvironment>();
                    var settings = services.GetService<IOptions<BookingSettings>>();
                    var logger = services.GetService<ILogger<GigBookingContextSeed>>();
                    
                    new GigBookingContextSeed()
                        .SeedAsync(context, env, settings, logger)
                        .Wait();

                })
                    .MigrateDbContext<BookingIntegrationLogContext>((_, __) => { });

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            return builder.Build();
        }
        public static IHostBuilder CreateHostBuilder(string[] args , IConfiguration configuration) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                        .Enrich.FromLogContext();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    var currentDir = Directory.GetCurrentDirectory();
                    webBuilder.UseStartup<Startup>()
                        .UseConfiguration(configuration)
                        .UseContentRoot(currentDir);
                })
                .UseServiceProviderFactory(new AutoFacDI.AutofacServiceProviderFactory());
    }

}
