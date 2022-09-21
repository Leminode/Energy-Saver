using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Energy_Saver.Model;

namespace Energy_Saver.Data
{
    public class Energy_SaverContext : DbContext
    {
        public Energy_SaverContext (DbContextOptions<Energy_SaverContext> options)
            : base(options)
        {
        }

        public DbSet<Energy_Saver.Model.Taxes> Taxes { get; set; } = default!;
    }
}
