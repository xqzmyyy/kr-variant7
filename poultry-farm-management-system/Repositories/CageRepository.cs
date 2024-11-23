using Microsoft.EntityFrameworkCore;
using farm.Data;
using farm.Models;

namespace farm.Repositories
{
    public static class CageRepository
    {
        // Get the cage with the chicken that lays the most eggs
        public static Cage? GetCageWithMostEggs()
        {
            using (var context = new AppDbContext())
            {
                return context.Cages
                    .Include(c => c.Chicken)
                    .OrderByDescending(c => c.Chicken != null ? c.Chicken.EggsPerMonth : 0)
                    .FirstOrDefault();
            }
        }

        // Get all cages
        public static List<Cage> GetAllCages()
        {
            using (var context = new AppDbContext())
            {
                return context.Cages.Include(c => c.Employee).Include(c => c.Chicken).ToList();
            }
        }
    }
}
