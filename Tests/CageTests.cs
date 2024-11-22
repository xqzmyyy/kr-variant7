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
                .UseInMemoryDatabase(databaseName: "TestDatabase")
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

                var cage = new Cage { EmployeeId = 1 };
                context.Cages.Add(cage);
                context.SaveChanges();

                var chicken = new Chicken { Weight = 2.5, Age = 12, EggsPerMonth = 20, CageId = cage.Id };
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

                var employee = new Employee { Name = "John Doe", Salary = 5000 };
                var cage = new Cage { Employee = employee };

                context.Employees.Add(employee);
                context.Cages.Add(cage);
                context.SaveChanges();

                Assert.Equal(employee.Id, cage.EmployeeId);
            }
        }
    }
}
