using Xunit;
using farm.Data;
using farm.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Tests
{
    public class EmployeeTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_EmployeeTests")
                .Options;
            return new AppDbContext(options);
        }

        private void ClearDatabase(AppDbContext context)
        {
            context.Chickens.RemoveRange(context.Chickens);
            context.Cages.RemoveRange(context.Cages);
            context.Employees.RemoveRange(context.Employees);
            context.SaveChanges();
        }

        [Fact]
        public void CanCountChickensPerEmployee()
        {
            using (var context = GetInMemoryContext())
            {
                ClearDatabase(context);

                var employee = new Employee { Name = "John Doe", Salary = 5000, Cages = new List<Cage>() };
                var cages = new[]
                {
                    new Cage { Employee = employee },
                    new Cage { Employee = employee }
                };

                context.Employees.Add(employee);
                context.Cages.AddRange(cages);
                context.SaveChanges();

                var chickenCount = cages.Count(c => c.EmployeeId == employee.Id);

                Assert.Equal(2, chickenCount);
            }
        }
    }
}