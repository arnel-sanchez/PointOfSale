using PointOfSale.Models.DataBaseModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PointOfSale.DataAccess
{
    public class PointOfSaleDbContext : IdentityDbContext<User>
    {
        public PointOfSaleDbContext(DbContextOptions<PointOfSaleDbContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }

        public DbSet<Modifier> Modifiers { get; set; }

        public DbSet<Sale> Sales { get; set; }
    }
}
