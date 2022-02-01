using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EFCore.NewFeatures._6._0
{
    public class Tests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly TestContext _db;

        public Tests(ITestOutputHelper output)
        {
            _output = output;
            _db = new TestContext();
            _db.Database.EnsureCreated();

            AddEntities();
        }

        public void Dispose()
        {
            _db.Database.EnsureDeleted();
        }

        [Fact]
        public void TemporalTables()
        {
            _db.Employees.First().Name = "Rafael Miranda";
            _db.SaveChanges();

            var history = _db
                .Employees
                .TemporalAll()
                .Where(e => e.Id == 1)
                .OrderBy(e => EF.Property<DateTime>(e, "PeriodEnd"))
                .Select(
                    e => new
                    {
                        Employee = e,
                        ValidFrom = EF.Property<DateTime>(e, "PeriodStart"),
                        ValidTo = EF.Property<DateTime>(e, "PeriodEnd")
                    })
                .ToList();

            foreach (var item in history)
            {
                _output.WriteLine($"Name: [{item.Employee.Name}] from {item.ValidFrom} to {item.ValidTo}");
            }
        }

        private void AddEntities()
        {
            _db.Employees.Add(new Employee
            {
                Name = "Rafael",
                Entries = new List<TimeEntry>
                {
                    new()
                    {
                        Start = TimeSpan.FromHours(8),
                        End = TimeSpan.FromHours(12)
                    },
                    new()
                    {
                        Start = TimeSpan.FromHours(19),
                        End = TimeSpan.FromHours(20)
                    }
                }
            });
            _db.Employees.Add(new Employee
            {
                Name = "Elsa",
                Entries = new List<TimeEntry>
                {
                    new()
                    {
                        Start = TimeSpan.FromHours(8),
                        End = TimeSpan.FromHours(12)
                    }
                }
            });
            _db.SaveChanges();
        }
    }
}