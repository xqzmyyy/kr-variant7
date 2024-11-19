using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using krv7.Data;
using krv7.Models;

namespace krv7.Repositories
{
    public static class EmployeeRepository
    {
        // get all employees
        public static List<Employee> GetAllEmployees()
        {
            using (var context = new AppDbContext())
            {
                return context.Employees.Include(e => e.Cages).ToList();
            }
        }

        // get count of chicken per employee
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
    }
}
