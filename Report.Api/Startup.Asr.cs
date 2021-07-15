using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Report.Core.Hubs;

namespace Report.Api
{
    public partial class Startup
    {

        private static void ConfigureAzureSignalRRoutes(IApplicationBuilder app)
        {


            app.UseAzureSignalR(routes =>
            {
                routes.MapHub<CustomerBookingsDashboardHub>("/bookingsHub");
                routes.MapHub<SalesDashboardHub>("/salesHub");
            });

        }

        private void AddAzureSignalRMiddleware(IServiceCollection services)
        {
            string azureSignalRConnection = Configuration["AzureSignalRConnection"];

            //Added Signal R service to current project 
            services.AddSignalR().AddAzureSignalR(azureSignalRConnection);
        }
    }
}