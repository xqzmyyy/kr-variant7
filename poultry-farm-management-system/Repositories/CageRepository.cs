using Microsoft.EntityFrameworkCore;
using farm.Data;
using farm.Models;

namespace farm.Repositories {
    public static class CageRepository {
        // get most count of eggs
        public static Cage? GetCageWithMostEggs() {
            using (var context = new AppDbContext()) {
                return context.Cages
                    .Include(c => c.Chicken)
                    .OrderByDescending(c => c.Chicken != null ? c.Chicken.EggsPerMonth : 0)
                    .FirstOrDefault();
            }
        }
    }
}
