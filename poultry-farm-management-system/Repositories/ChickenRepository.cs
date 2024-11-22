using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore; 
using farm.Data; 
using farm.Models;

namespace farm.Repositories 
{
    public static class ChickenRepository {
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
            using (var context = new AppDbContext()) {
                return context.Chickens
                    .Where(c => c.Weight == weight && c.Age == age)
                    .ToList();
            }
        }

        // get chicken with the most count of eggs
        public static Chicken GetChickenWithMostEggs() 
        {
            using (var context = new AppDbContext()) {
                return context.Chickens
                    .OrderByDescending(c => c.EggsPerMonth)
                    .FirstOrDefault() ?? new Chicken { Id = 0, Weight = 0, Age = 0, EggsPerMonth = 0 }; // return init value
            }
        }

        // get chickens below average
        public static List<Chicken> GetChickensBelowAverage() 
        {
            using (var context = new AppDbContext())
            {
                var averageEggs = context.Chickens.Average(c => c.EggsPerMonth);
                return context.Chickens
                    .Where(c => c.EggsPerMonth < averageEggs)
                    .ToList();
            }
        }

        // add chicken
        public static void AddChicken(Chicken chicken) 
        {
            using (var context = new AppDbContext()) {
                context.Chickens.Add(chicken);
                context.SaveChanges();
            }
        }

        // delete chicken
        public static bool DeleteChicken(int id) 
        {
            using (var context = new AppDbContext()) 
            {
                var chicken = context.Chickens.Find(id);
                if (chicken == null) return false;

                context.Chickens.Remove(chicken);
                context.SaveChanges();
                return true;
            }
        }

    }
}
