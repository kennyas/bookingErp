using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.AuditLogEvent.AuditLogService;
using Tornado.Shared.AuditLogEvent.Interface;
using Tornado.Shared.Dapper.Interfaces;
using Tornado.Shared.Dapper.Repository;
//using Tornado.Shared.EF;
//using Tornado.Shared.EF.Repository;
using Wallet.Core.Services;
using Wallet.Core.Services.Interfaces;

namespace Wallet.Api
{
    public partial class Startup
    {
        public void ConfigureDIService(IServiceCollection services)
        {
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<ICustomerWalletService, CustomerWalletService>();
            services.AddTransient<IWalletEventService, WalletEventService>();
            services.AddTransient<ICardDetailsService, CardDetailsService>();
            services.AddTransient<IAuditLogEventService, AuditLogEventService>();

            services.AddScoped<IHttpUserService, HttpUserService>();
            services.AddHttpContextAccessor();

            services.AddScoped<IDbConnection>(db => new SqlConnection(
                    Configuration.GetConnectionString("Default")));

            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), typeof(DapperRepository<>));
            // services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
        }
    }
}
