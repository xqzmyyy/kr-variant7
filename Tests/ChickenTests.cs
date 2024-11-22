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
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            return new AppDbContext(options);
        }

        // clear database
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

                // create cage without chicken
                var cage = new Cage { EmployeeId = 1 };
                context.Cages.Add(cage);
                context.SaveChanges();

                // add chicken and relate to cage
                var chicken = new Chicken { Weight = 2.5, Age = 12, EggsPerMonth = 20 };
                context.Chickens.Add(chicken);
                context.SaveChanges();

                // upd relation cage to chicken
                cage.ChickenId = chicken.Id;
                cage.Chicken = chicken;
                context.Cages.Update(cage);
                context.SaveChanges();

                // check if it was success
                Assert.Single(context.Chickens);
                Assert.Equal(2.5, context.Chickens.First().Weight);
            }
        }

        [Fact]
        public void CanDeleteChicken() 
        {
            using (var context = GetInMemoryContext()) 
            {
                ClearDatabase(context);

                // create cage and chicken
                var cage = new Cage { EmployeeId = 1 };
                context.Cages.Add(cage);
                context.SaveChanges();

                var chicken = new Chicken { Weight = 2.5, Age = 12, EggsPerMonth = 20 };
                context.Chickens.Add(chicken);
                context.SaveChanges();

                cage.ChickenId = chicken.Id;
                context.Cages.Update(cage);
                context.SaveChanges();

                // delete chicken
                context.Chickens.Remove(chicken);
                context.SaveChanges();

                Assert.Empty(context.Chickens);
            }
        }

        [Fact]
        public void CanCalculateAverageEggs() 
        {
            using (var context = GetInMemoryContext()) {
                ClearDatabase(context);

                // add chicken
                context.Chickens.AddRange(
                    new Chicken { Weight = 2.5, Age = 12, EggsPerMonth = 20 },
                    new Chicken { Weight = 3.0, Age = 15, EggsPerMonth = 30 }
                );
                context.SaveChanges();

                // calculate avarage eggs
                var averageEggs = context.Chickens.Average(c => c.EggsPerMonth);

                Assert.Equal(25, averageEggs);
            }
        }

        [Fact]
        public void CanFindChickenWithMostEggs() 
        {
            using (var context = GetInMemoryContext()) 
            {
                ClearDatabase(context);

                // add chicken
                context.Chickens.AddRange(
                    new Chicken { Weight = 2.5, Age = 12, EggsPerMonth = 20 },
                    new Chicken { Weight = 3.0, Age = 15, EggsPerMonth = 30 }
                );
                context.SaveChanges();

                // find chicken with the most eggs
                var topChicken = context.Chickens
                    .OrderByDescending(c => c.EggsPerMonth)
                    .FirstOrDefault();

                Assert.NotNull(topChicken);
                Assert.Equal(30, topChicken.EggsPerMonth);
            }
        }
    }
}
