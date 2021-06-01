using DataAccessContracts.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessServices {
    public class TurnoverDbContext : IdentityDbContext<TurnoverUser, TurnoverRole, int> {
        public TurnoverDbContext(DbContextOptions options) : base(options) {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<RequestDelivery> RequestDeliverys { get; set; }
    }
}
