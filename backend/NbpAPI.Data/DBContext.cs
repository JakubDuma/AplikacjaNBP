using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NbpAPI.Data.Models;

namespace NbpAPI.Data
{
    public class DBContext(DbContextOptions<DBContext> options) : DbContext(options)
    {
        public DbSet<RateByDate> RatesByDate { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<RateByDate>(entity =>
    {
        entity.Property(e => e.Mid)
              .HasPrecision(18, 4); 
    });
            // Ustawienia modeli
        }
		
    }
}
