using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using indexPay.Models;
using System;

namespace indexPay.Repositories
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }

        public DbSet<Transactions> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Entity<Transactions>().Property(p => p.Status).HasConversion(o => Enum.GetName(typeof(Status), o),
o => Enum.Parse<Status>(o, true));
        }

    }



    
}
