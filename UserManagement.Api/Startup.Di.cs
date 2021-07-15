using AspNetCore.Totp;
using AspNetCore.Totp.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Data.Common;
using System.IO;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Repository;
using Tornado.Shared.FileStorage;
using Tornado.Shared.IntegrationEventLogEF.Services;
using UserManagement.Core.Context;
using UserManagement.Core.Services;
using UserManagement.Core.Services.Interfaces;

namespace UserManagement.Api
{
    public partial class Startup
    {
        public void ConfigureDIService(IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<ICorporateAccountService, CorporateAccountService>();

            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IPartnerService, PartnerService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddTransient<ICaptainService, CaptainService>();
            services.AddTransient<IBusBoyService, BusBoyService>();
            services.AddTransient<IRoleService, RoleService>();

            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
               sp => (DbConnection c) => new IntegrationEventLogService(c));

            services.AddTransient<IUserMnagementEventService, UserMangementEventService>();

            services.AddTransient<ITotpGenerator, TotpGenerator>();
            services.AddSingleton<ITotpService, TotpService>();

            services.AddScoped<IDbContext, GigAuthDbContext>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));


            services.AddSingleton<IFileProvider>((d) =>
            {
                var path = "wwwroot/" + Configuration.GetValue<string>("Filestore") ?? "files";

                return new PhysicalFileProvider(
                    //Path.IsPathRooted(path) ? path :
                    Path.Combine(HostingEnvironment.ContentRootPath, path));
            });

            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
        }
    }
}