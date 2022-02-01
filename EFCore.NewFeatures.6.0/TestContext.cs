using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EFCore.NewFeatures._6._0
{
    public class TestContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    "Data Source=localhost,1433;Initial Catalog=Timesheet_6_0;User Id=sa;Password=Password1")
                .LogTo(Console.WriteLine, LogLevel.Information);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Customer> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Employee>()
                .ToTable("Employees", b => b.IsTemporal())
                .Property(x => x.Name).HasMaxLength(100);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder
                .Properties<string>()
                .HaveMaxLength(200);
        }
    }

    public class Customer
    {
        public int Id { get; set; }
        public List<User> Users { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public List<Customer> Customer { get; set; }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<TimeEntry> Entries { get; set; }
    }

    public class TimeEntry
    {
        public int Id { get; set; }
        public DateTime Day { get; set; } = DateTime.Today;
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}