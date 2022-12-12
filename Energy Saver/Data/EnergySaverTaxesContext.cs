using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Energy_Saver.Model;

namespace Energy_Saver.DataSpace
{
    public class EnergySaverTaxesContext : DbContext
    {
        public EnergySaverTaxesContext (DbContextOptions<EnergySaverTaxesContext> options)
            : base(options)
        {
        }

        public DbSet<Taxes> Taxes { get; set; }
        public DbSet<UserInfo> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Taxes>().ToTable("taxes");
            modelBuilder.Entity<UserInfo>().ToTable("users");
        }
    }
}
