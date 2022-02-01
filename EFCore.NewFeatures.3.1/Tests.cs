using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EFCore.NewFeatures._3._1
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
        public void IncludeBehavior_Join()
        {
            // Get with Include
            var employee = _db.Employees
                .Include(x => x.Entries)
                .First(x => x.Name == "Rafael");
        }

        [Fact]
        public void ClientEvaluation_ThrowError()
        {
            // Get with function
            Assert.Throws<InvalidOperationException>(
                () => _db.Employees.First(x => IsRafael(x.Name)));
        }

        [Fact]
        public void ClientEvaluation_Select()
        {
            var result = _db.Employees
                .Select(x => IsRafael(x.Name))
                .ToArray();

            Assert.Equal(new[] { true, false}, result);
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