using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Notify.Core.Context;
using Notify.Core.Event;
using Notify.Core.Event.EventHandler;
using Notify.Core.IOC;
using System;
using System.IO;
using System.Reflection;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.Extensions;
using Tornado.Shared.Messaging.Email;

namespace Notify.Api
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
            services.AddSwagger("Booking Service");
            services.AddControllers();

            services.Configure<SmtpConfig>(options =>
                    Configuration.GetSection("smtp").Bind(options));

            AddEntityFrameworkDbContext(services);
            ConfigureDIService(services);
            ConfigureIntegrationService(services, Configuration);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ContainerModule());
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")),
                RequestPath = "/MyImages"
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Notify Service up and running");
                });
            });
            ConfigureEventBus(app);
        }
        public IServiceCollection AddEntityFrameworkDbContext(IServiceCollection services)
        {
            string dbConnStr = Configuration.GetConnectionString("Default");
            services.AddDbContext<NotificationContext>(options =>
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
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<PasswordResetIntegrationEvent, PasswordResetEventHandler>();
            eventBus.Subscribe<CustomerAccountActivationIntegrationEvent, CustomerAccountActivationHandler>();
            eventBus.Subscribe<UserCreatedIntegrationEvent, UserCreatedEventHandler>();
            eventBus.Subscribe<BookingCompletedIntegrationEvent, BookingCompletedEventHandler>();
            eventBus.Subscribe<BookingCancelledIntegrationEvent, BookingCancelledEventHandler>();
            eventBus.Subscribe<PaymentSuccededIntegrationEvent, PaymentSuccededEventHandler>(); 
        }
    }
}