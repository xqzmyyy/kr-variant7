using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using farm.Data;
using farm.Models;

namespace farm.Repositories
{
    public static class EmployeeRepository
    {
        // Get all employees
        public static List<Employee> GetAllEmployees()
        {
            using (var context = new AppDbContext())
            {
                return context.Employees.Include(e => e.Cages).ToList();
            }
        }

        // Get the count of chickens per employee
        public static Dictionary<Employee, int> GetEmployeeChickenCount()
        {
            using (var context = new AppDbContext())
            {
                return context.Employees
                    .Select(e => new
                    {
                        Employee = e,
                        ChickenCount = e.Cages.Count(c => c.ChickenId != null)
                    })
                    .ToDictionary(x => x.Employee, x => x.ChickenCount);
            }
        }

        // Get the count of eggs collected by each employee
        public static Dictionary<Employee, int> GetEggsCollectedByEmployees()
        {
            using (var context = new AppDbContext())
            {
                return context.Employees
                    .Select(e => new
                    {
                        Employee = e,
                        EggsCollected = e.Cages.Count(c => c.IsEggLaid)
                    })
                    .ToDictionary(x => x.Employee, x => x.EggsCollected);
            }
        }
    }
}
