using Microsoft.EntityFrameworkCore;
using krv7.Data;
using krv7.Models;

namespace krv7.Repositories
{
    public static class CageRepository
    {
        // get most count of eggs
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
    }
}
