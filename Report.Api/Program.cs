using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Report.Core.Context;
using Report.Core.Utilities.Settings;
using Serilog;
using System;
using System.IO;
using Tornado.Shared.AspNetCore.Hosting;
using AutoFacDI = Autofac.Extensions.DependencyInjection;

namespace Report.Api
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
                CreateHostBuilder(args, configuration).Build();
            try
            {
                host.MigrateDbContext<GigReportContext>((context, services) =>
                 {
                     var env = services.GetService<IWebHostEnvironment>();
                     var settings = services.GetService<IOptions<ReportSettings>>();
                     var logger = services.GetService<ILogger<GigReportContextSeed>>();

                     new GigReportContextSeed()
                         .SeedAsync(context, env, settings, logger)
                         .Wait();
                 });

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
            _ = builder.Build();

            return builder.Build();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
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
