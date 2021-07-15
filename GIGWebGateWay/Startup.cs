using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.IO;

namespace GIGWebGateWay
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot(Configuration);
            services.AddApplicationInsightsTelemetry();
        }
        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();


            //app.UseSwagger("/swagger/v1/swagger.json");
            app.Map("/swagger/v1/swagger.json", b =>
            {
                b.Run(async x =>
                {
                    var json = File.ReadAllText("swagger.json");
                    await x.Response.WriteAsync(json);
                });
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ocelot");
            });
            await app.UseOcelot();
        }
    }
}
