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

        public DbSet<Taxes> Taxes { get; set; } = default!;
        public DbSet<Users> Users { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Taxes>().ToTable("taxes");
            modelBuilder.Entity<Users>().ToTable("users");
        }
    }
}
