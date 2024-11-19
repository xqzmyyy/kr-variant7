using Xunit;
using krv7.Data;
using krv7.Models;
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

                var chicken = new Chicken { Weight = 2.5, Age = 12, EggsPerMonth = 20, CageId = 1 };
                context.Chickens.Add(chicken);
                context.SaveChanges();

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

                var chicken = new Chicken { Weight = 2.5, Age = 12, EggsPerMonth = 20, CageId = 1 };
                context.Chickens.Add(chicken);
                context.SaveChanges();

                context.Chickens.Remove(chicken);
                context.SaveChanges();

                Assert.Empty(context.Chickens);
            }
        }


        [Fact]
        public void CanCalculateAverageEggs()
        {
            using (var context = GetInMemoryContext())
            {
                ClearDatabase(context);

                context.Chickens.AddRange(
                    new Chicken { Weight = 2.5, Age = 12, EggsPerMonth = 20 },
                    new Chicken { Weight = 3.0, Age = 15, EggsPerMonth = 30 }
                );
                context.SaveChanges();

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

                context.Chickens.AddRange(
                    new Chicken { Weight = 2.5, Age = 12, EggsPerMonth = 20 },
                    new Chicken { Weight = 3.0, Age = 15, EggsPerMonth = 30 }
                );
                context.SaveChanges();

                var topChicken = context.Chickens
                    .OrderByDescending(c => c.EggsPerMonth)
                    .FirstOrDefault();

                Assert.NotNull(topChicken);
                Assert.Equal(30, topChicken.EggsPerMonth);
            }
        }
    }
}
