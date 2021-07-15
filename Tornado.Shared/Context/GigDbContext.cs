using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models;

namespace Tornado.Shared.Context
{
    public class GigDbContext : AuthDbContext
    {
        public GigDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore(typeof(GigmUser));
            modelBuilder.Ignore(typeof(GigmRole));
            modelBuilder.Ignore(typeof(GigmUserClaim));
            modelBuilder.Ignore(typeof(GigmUserRole));
            modelBuilder.Ignore(typeof(GigmUserLogin));
            modelBuilder.Ignore(typeof(GigmRoleClaim));
            modelBuilder.Ignore(typeof(GigmUserToken));
        }
    }
}
