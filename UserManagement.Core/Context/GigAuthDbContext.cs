using Microsoft.EntityFrameworkCore;
using Tornado.Shared.Context;
using Tornado.Shared.Models.Map;
using UserManagement.Core.Models.Map;
using System.Reflection;

namespace UserManagement.Core.Context
{
    public class GigAuthDbContext : AuthDbContext
    {
        public GigAuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MyAppRoleMap());
            modelBuilder.ApplyConfiguration(new MyAppUserMap());
            modelBuilder.ApplyConfiguration(new MyAppUserRoleMap());
            modelBuilder.ApplyConfiguration(new MyAppUserTokenMap());
            modelBuilder.ApplyConfiguration(new MyAppUserLoginMap());
            modelBuilder.ApplyConfiguration(new MyAppUserClaimMap());
            modelBuilder.ApplyConfiguration(new MyAppRoleClaimMap());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.UseOpenIddict();
        }
    }
}
