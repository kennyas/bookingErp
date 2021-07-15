using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Tornado.Shared.Context;
using Wallet.Core.Models.Map;

namespace Wallet.Core.Context
{

    //Fund wallet 
    public class GigWalletContext : GigDbContext
    {
        public GigWalletContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyConfiguration(new CustomerWalletMap());
            modelBuilder.ApplyConfiguration(new PaymentDetailMap());
            modelBuilder.ApplyConfiguration(new CardDetailsMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
