using Xunit;
using farm.Data;
using farm.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Tests
{
    public class CageTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_CageTests")
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
        public void CanAssignChickenToCage()
        {
            using (var context = GetInMemoryContext())
            {
                ClearDatabase(context);

                var employee = new Employee { Name = "John Doe", Salary = 5000, Cages = new List<Cage>() };
                context.Employees.Add(employee);
                context.SaveChanges();

                var cage = new Cage { EmployeeId = employee.Id, Employee = employee };
                context.Cages.Add(cage);
                context.SaveChanges();

                var chicken = new Chicken { Weight = 2.5, Age = 12, EggsPerMonth = 20, CageId = cage.Id, Cage = cage, Breed = "Леггорн" };
                context.Chickens.Add(chicken);
                context.SaveChanges();

                var retrievedChicken = context.Chickens.First();
                Assert.Equal(cage.Id, retrievedChicken.CageId);
            }
        }

        [Fact]
        public void CanAssignEmployeeToCage()
        {
            using (var context = GetInMemoryContext())
            {
                ClearDatabase(context);

                var employee = new Employee { Name = "Jane Doe", Salary = 4500, Cages = new List<Cage>() };
                var cage = new Cage { EmployeeId = employee.Id, Employee = employee };

                context.Employees.Add(employee);
                context.Cages.Add(cage);
                context.SaveChanges();

                Assert.Equal(employee.Id, cage.EmployeeId);
            }
        }
    }
}
