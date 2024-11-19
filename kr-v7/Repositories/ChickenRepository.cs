using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore; 
using krv7.Data; 
using krv7.Models;

namespace krv7.Repositories
{
    public static class ChickenRepository
    {
        // get all chickens
        public static List<Chicken> GetAllChickens()
        {
            using (var context = new AppDbContext())
            {
                return context.Chickens.ToList();
            }
        }

        // get weight and age
        public static List<Chicken> GetChickensByWeightAndAge(double weight, int age)
        {
            using (var context = new AppDbContext())
            {
                return context.Chickens
                    .Where(c => c.Weight == weight && c.Age == age)
                    .ToList();
            }
        }

        // get chicken with the most count of eggs
        public static Chicken GetChickenWithMostEggs()
        {
            using (var context = new AppDbContext())
            {
                return context.Chickens
                    .OrderByDescending(c => c.EggsPerMonth)
                    .FirstOrDefault() ?? new Chicken { Id = 0, Weight = 0, Age = 0, EggsPerMonth = 0 }; // return init value
            }
        }
    }
}
