using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Core.Context;
using Report.Core.Services;
using Report.Core.Services.Interfaces;
using System.Data;
using System.Data.SqlClient;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Repository;
using Tornado.Shared.Utils;

namespace Report.Api
{
    public partial class Startup
    {
        public static void ConfigureDIService(IServiceCollection services, IConfiguration configuration)
        {

            services.AddSingleton<IGuidGenerator>((s) => SequentialGuidGenerator.Instance);

            services.AddTransient<IBroadcastCustomerBookingsDashboardService, BroadcastCustomerBookingsDashboardService>();
            services.AddTransient<IBroadcastSalesDashboardService, BroadcastSalesDashboardService>();
            services.AddTransient<IBroadcastService, BookingsAndSalesDashboardBroadcast>();
            services.AddTransient<IBookedBusesReportService, BookedBusesReportService>();
            services.AddTransient<IBookedTicketsReportService, BookedTicketsReportService>();
            services.AddTransient<IBusboySalesReportService, BusboySalesReportService>();
            services.AddTransient<ICustomerBookingsDashboardService, CustomerBookingsDashboardService>();
            services.AddTransient<ICustomerBookingsReportService, CustomerBookingsReportService>();
            services.AddTransient<ITripSalesReportService, TripSalesReportService>();
            services.AddTransient<ISalesDashboardService, SalesDashboardService>();

            services.AddScoped<IDbContext, GigReportContext>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));

            services.AddScoped<IDbConnection>(db => new SqlConnection(
              configuration.GetConnectionString("Default")));

            services.AddScoped(typeof(Tornado.Shared.Dapper.Interfaces.IUnitOfWork), typeof(Tornado.Shared.Dapper.Repository.UnitOfWork));
            services.AddScoped(typeof(Tornado.Shared.Dapper.Interfaces.IDapperRepository<>), typeof(Tornado.Shared.Dapper.Repository.DapperRepository<>));
        }
    }
}