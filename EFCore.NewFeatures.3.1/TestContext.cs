using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace EFCore.NewFeatures._3._1
{
    public class TestContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    "Data Source=localhost,1433;Initial Catalog=Timesheet_3_1;User Id=sa;Password=Password1")
                .UseLoggerFactory(LoggerFactory);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        private static readonly ILoggerFactory LoggerFactory
            = Microsoft.Extensions.Logging.LoggerFactory.Create(builder => { builder.AddConsole(); });
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
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}