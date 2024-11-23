using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using farm.Data;
using farm.Models;

namespace farm.Repositories
{
    public static class ChickenRepository
    {
        // Get all chickens
        public static List<Chicken> GetAllChickens()
        {
            using (var context = new AppDbContext())
            {
                return context.Chickens.Include(c => c.Cage).ToList();
            }
        }

        // Get chickens by weight and age
        public static List<Chicken> GetChickensByWeightAndAge(double weight, int age)
        {
            using (var context = new AppDbContext())
            {
                return context.Chickens
                    .Where(c => c.Weight == weight && c.Age == age)
                    .ToList();
            }
        }

        // Get the chicken with the most eggs
        public static Chicken? GetChickenWithMostEggs()
        {
            using (var context = new AppDbContext())
            {
                return context.Chickens
                    .OrderByDescending(c => c.EggsPerMonth)
                    .FirstOrDefault();
            }
        }

        // Get chickens below the average egg production
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

        // Add a chicken
        public static void AddChicken(Chicken chicken)
        {
            using (var context = new AppDbContext())
            {
                context.Chickens.Add(chicken);
                context.SaveChanges();
            }
        }

        // Delete a chicken by ID
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
