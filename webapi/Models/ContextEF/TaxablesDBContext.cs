using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
//using System.Collections.Generic;

namespace webapi.Models.ContextEF
{
    public class TaxablesDBContext : DbContext
    {
        public TaxablesDBContext(DbContextOptions<TaxablesDBContext> options)
               : base(options)
        {
            try
            {
                this.Database.EnsureCreated();
            }
            catch { }

        }

        public DbSet<TaxTables> TaxTable { get; set; } = null!;

        public DbSet<TaxableExemptions> TaxExemptions { get; set; } = null!;

        public DbSet<TaxRebates> TaxRebate { get; set; } = null!;
       
    }
}
