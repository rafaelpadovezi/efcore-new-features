using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EFCore.NewFeatures._2._2
{
    public class Tests : IDisposable
    {
        private readonly TestContext _db;

        public Tests()
        {
            _db = new TestContext();
            _db.Database.EnsureCreated();

            AddEntities();
        }

        public void Dispose() => _db.Database.EnsureDeleted();

        [Fact]
        public void IncludeBehavior_SplitQuery()
        {
            // Get with Include
            var employee = _db.Employees
                .Include(x => x.Entries)
                .First(x => x.Name == "Rafael");
        }

        [Fact]
        public void ClientEvaluation()
        {
            // Get with function
            var employee = _db.Employees
                .First(x => IsRafael(x.Name));
        }

        [Fact]
        public void ClientEvaluation_ThrowError()
        {
            var dbWithoutClientEvaluation = new TestContext(true);
            // Get with function
            Assert.Throws<InvalidOperationException>(
                () => dbWithoutClientEvaluation.Employees.First(x => IsRafael(x.Name)));
        }

        private static bool IsRafael(string name) => name == "Rafael";

        private void AddEntities()
        {
            _db.Employees.Add(new Employee
            {
                Name = "Rafael",
                Entries = new List<TimeEntry>
                {
                    new TimeEntry
                    {

                        Start = TimeSpan.FromHours(8),
                        End = TimeSpan.FromHours(12)
                    }
                }
            });
            _db.Employees.Add(new Employee
            {
                Name = "Elsa",
                Entries = new List<TimeEntry>
                {
                    new TimeEntry
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