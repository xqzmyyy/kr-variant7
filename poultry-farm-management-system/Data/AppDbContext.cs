using Microsoft.EntityFrameworkCore;
using farm.Models;

namespace farm.Data 
{
    public class AppDbContext : DbContext 
    {
        // Chicken table
        public DbSet<Chicken> Chickens { get; set; }

        // Employee table
        public DbSet<Employee> Employees { get; set; }

        // Cage table
        public DbSet<Cage> Cages { get; set; }

        // Default constructor for SQLite configuration
        public AppDbContext() { }

        // Constructor for test configurations (e.g., InMemoryDatabase)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Configures the SQLite database connection
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=Data/database.db");
            }
        }

        // Configures relationships between entities
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            // A cage is assigned to one employee (many cages - one employee)
            modelBuilder.Entity<Cage>()
                .HasOne(c => c.Employee)
                .WithMany(e => e.Cages)
                .HasForeignKey(c => c.EmployeeId);

            // A cage is associated with one chicken (one-to-one relationship)
            modelBuilder.Entity<Cage>()
                .HasOne(c => c.Chicken)
                .WithOne(c => c.Cage)
                .HasForeignKey<Cage>(c => c.ChickenId);

            // Default value for IsEggLaid
            modelBuilder.Entity<Cage>()
                .Property(c => c.IsEggLaid)
                .HasDefaultValue(false); // init value (false)
        }

        // start data
        public void SeedData() 
        {
            // Ensures the database is populated only if it's empty
            if (!Chickens.Any() && !Employees.Any() && !Cages.Any()) 
            {
                // Add employees
                Employees.AddRange(new[]
                {
                    new Employee { Name = "Геннадій 1xbetович", Salary = 5000 },
                    new Employee { Name = "Тамара 1winовна", Salary = 6000 },
                    new Employee { Name = "Микола Parimatchович", Salary = 4800 },
                    new Employee { Name = "Юрій Favbetович", Salary = 5300 },
                    new Employee { Name = "Полтавський Палій", Salary = 5300 },
                });

                SaveChanges();

                // Add cages
                Cages.AddRange(new[] 
                {
                    new Cage { EmployeeId = 1, Date = DateTime.Today, IsEggLaid = true },
                    new Cage { EmployeeId = 1, Date = DateTime.Today, IsEggLaid = false },
                    new Cage { EmployeeId = 2, Date = DateTime.Today.AddDays(-1), IsEggLaid = true },
                    new Cage { EmployeeId = 3, Date = DateTime.Today.AddDays(-2), IsEggLaid = false },
                    new Cage { EmployeeId = 4, Date = DateTime.Today, IsEggLaid = true }
                });

                SaveChanges();

                // Add chickens
                Chickens.AddRange(new[] 
                {
                    new Chicken { Weight = 2.5, Age = 12, EggsPerMonth = 20, CageId = 1, Breed = "Леггорн" },
                    new Chicken { Weight = 3.0, Age = 15, EggsPerMonth = 25, CageId = 2, Breed = "Орпингтон" },
                    new Chicken { Weight = 1.8, Age = 10, EggsPerMonth = 18, CageId = 3, Breed = "Плімутрок" },
                    new Chicken { Weight = 2.0, Age = 11, EggsPerMonth = 22, CageId = 4, Breed = "Кучинська ювілейна" }
                });

                SaveChanges();
            }
        }
    }
}
