using Xunit;
using farm.Data;
using farm.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Tests
{
    public class ChickenTests
    {
        private AppDbContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_ChickenTests")
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
        public void CanAddChicken()
        {
            using (var context = GetInMemoryContext())
            {
                ClearDatabase(context);

                var employee = new Employee { Name = "John Doe", Salary = 5000, Cages = new List<Cage>() };
                var cage = new Cage { EmployeeId = 1, Employee = employee };
                context.Employees.Add(employee);
                context.Cages.Add(cage);
                context.SaveChanges();

                var chicken = new Chicken { Weight = 2.5, Age = 12, EggsPerMonth = 20, Breed = "Леггорн", CageId = cage.Id, Cage = cage };
                context.Chickens.Add(chicken);
                context.SaveChanges();

                Assert.Single(context.Chickens);
                Assert.Equal(2.5, context.Chickens.First().Weight);
            }
        }

        [Fact]
        public void CanCalculateAverageEggs()
        {
            using (var context = GetInMemoryContext())
            {
                ClearDatabase(context);

                var chicken1 = new Chicken { Weight = 2.5, Age = 12, EggsPerMonth = 20, Breed = "Леггорн", CageId = 1, Cage = new Cage() };
                var chicken2 = new Chicken { Weight = 3.0, Age = 15, EggsPerMonth = 30, Breed = "Орпингтон", CageId = 2, Cage = new Cage() };
                context.Chickens.AddRange(chicken1, chicken2);
                context.SaveChanges();

                var averageEggs = context.Chickens.Average(c => c.EggsPerMonth);

                Assert.Equal(25, averageEggs);
            }
        }
    }
}
