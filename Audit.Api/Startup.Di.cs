using Audit.Core.Services;
using Audit.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.SqlClient;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.Dapper.Interfaces;
using Tornado.Shared.Dapper.Repository;


namespace Audit.Api
{
    public partial class Startup
    {
        public void ConfigureDIService(IServiceCollection services)
        {
            services.AddTransient<IAuditService, AuditService>();         

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
