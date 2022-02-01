using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;

namespace EFCore.NewFeatures._2._2
{
    public class TestContext : DbContext
    {
        private readonly bool _errorOnClientEvaluation = false;
        public TestContext() { }

        public TestContext(bool errorOnClientEvaluation) : this() =>
            _errorOnClientEvaluation = errorOnClientEvaluation;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                    "Data Source=localhost,1433;Initial Catalog=Timesheet_2_2;User Id=sa;Password=Password1")
                .UseLoggerFactory(LoggerFactory)
                .EnableSensitiveDataLogging();

            if (_errorOnClientEvaluation)
                optionsBuilder
                    .ConfigureWarnings(warning => warning.Throw(RelationalEventId.QueryClientEvaluationWarning));
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        private static readonly LoggerFactory LoggerFactory
            = new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });
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