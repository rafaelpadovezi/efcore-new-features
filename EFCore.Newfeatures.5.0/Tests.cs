using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace EFCore.Newfeatures._5._0
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
        public void IncludeBehavior_Join()
        {
            var name = "Rafael";
            // Get with Include
            var employee = _db.Employees
                .Include(x => x.Entries)
                .First(x => x.Name == name);
        }

        [Fact]
        public void IncludeBehavior_Join_SplitQueries()
        {
            var name = "Rafael";
            // Get with Include
            var employee = _db.Employees
                .Include(x => x.Entries)
                .AsSplitQuery()
                .First(x => x.Name == name);
        }

        [Fact]
        public void ToQueryString()
        {
            var name = "Rafael";
            // Get with Include
            _output.WriteLine(_db.Employees.ToQueryString());
        }

        [Fact]
        public void ChangeTracker_DebugView()
        {
            using var db = new TestContext();

            db.Customers.Add(new Customer
            {
                Users = new List<User> { new() }
            });
            var employee = db.Employees
                .Include(x => x.Entries)
                .First();

            employee.Name = "Rafael Miranda";
            db.Update(employee);
            db.Remove(employee.Entries.First());

            _output.WriteLine(db.ChangeTracker.DebugView.LongView);
        }

        [Fact]
        public void FilteredInclude()
        {
            using var db = new TestContext();

            var query = db.Employees
                .Include(x => x.Entries
                    .Where(timeEntry => timeEntry.Start > TimeSpan.FromHours(18)));

            _output.WriteLine(query.ToQueryString());

            var employees = query.ToList();
            Assert.Collection(employees,
                employee =>
                {
                    Assert.Equal("Rafael", employee.Name);
                    Assert.Single(employee.Entries);
                },
                employee =>
                {
                    Assert.Equal("Elsa", employee.Name);
                    Assert.Empty(employee.Entries);
                });
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