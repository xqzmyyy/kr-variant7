using System;
using System.Linq;
using krv7.Data;
using krv7.Models;
using krv7.Repositories;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new AppDbContext())
        {
            // init new database
            context.SeedData();
        }

        while (true)
        {
            // start menu options
            Console.Clear();
            PrintHeader("=== Меню ===", ConsoleColor.Red);
            PrintMenuOption("1", "📋 Список всіх курей");
            PrintMenuOption("2", "👷 Список всіх працівників");
            PrintMenuOption("3", "🛖 Список всіх кліток");
            PrintMenuOption("4", "🥚 Середня кількість яєць для курей заданої ваги та віку");
            PrintMenuOption("5", "📅 Загальна кількість яєць за діапазон днів та їхня вартість");
            PrintMenuOption("6", "🏆 Курица з найбільшою кількістю яєць");
            PrintMenuOption("7", "📊 Кількість курей, закріплених за кожним працівником");
            PrintMenuOption("8", "⚠️ Куриці з несучістю нижче середнього рівня по фабриці");
            PrintMenuOption("9", "➕ Додати нову курку");
            PrintMenuOption("10", "❌ Видалити курку");
            PrintMenuOption("11", "🚪 Вийти");

            Console.Write("\nВаш вибір: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            var choice = Console.ReadLine();
            Console.ResetColor();

            Console.Clear();

            // choices
            switch (choice)
            {
                case "1":
                    ListAllChickens();
                    break;
                case "2":
                    ListAllEmployees();
                    break;
                case "3":
                    ListAllCages();
                    break;
                case "4":
                    AverageEggsForWeightAndAge();
                    break;
                case "5":
                    TotalEggsForDateRange();
                    break;
                case "6":
                    ChickenWithMostEggs();
                    break;
                case "7":
                    ChickensPerEmployee();
                    break;
                case "8":
                    ChickensBelowAverage();
                    break;
                case "9":
                    AddChicken();
                    break;
                case "10":
                    DeleteChicken();
                    break;
                case "11":
                    PrintMessage("До побачення! 🚪", ConsoleColor.Cyan);
                    return;
                default:
                    PrintMessage("Неправильний вибір. Спробуйте ще раз. ❌", ConsoleColor.Red);
                    break;
            }

            PrintMessage("\nНатисніть будь-яку кнопку, щоб повернутися до меню...", ConsoleColor.Yellow);
            Console.ReadKey();
        }
    }

    // list of all chickens
    static void ListAllChickens()
    {
        using (var context = new AppDbContext())
        {
            PrintHeader("=== Список всіх курей ===", ConsoleColor.Red);
            var chickens = context.Chickens.ToList();
            foreach (var chicken in chickens)
            {
                PrintMessage($"ID: {chicken.Id}, Вага: {chicken.Weight}, Вік: {chicken.Age}, Яйця в місяць: {chicken.EggsPerMonth}, Клітка ID: {chicken.CageId}", ConsoleColor.White);
            }
        }
    }

    // list of all employees
    static void ListAllEmployees()
    {
        using (var context = new AppDbContext())
        {
            PrintHeader("=== Список всіх працівників ===", ConsoleColor.Red);
            var employees = context.Employees.ToList();
            foreach (var employee in employees)
            {
                PrintMessage($"ID: {employee.Id}, Ім'я: {employee.Name}, Зарплата: {employee.Salary}", ConsoleColor.White);
            }
        }
    }

    // list of all cages
    static void ListAllCages()
    {
        using (var context = new AppDbContext())
        {
            PrintHeader("=== Список всіх кліток ===", ConsoleColor.Red);
            var cages = context.Cages.ToList();
            foreach (var cage in cages)
            {
                PrintMessage($"ID: {cage.Id}, Курица ID: {(cage.ChickenId.HasValue ? cage.ChickenId.Value.ToString() : "Порожня")}, Працівник ID: {cage.EmployeeId}", ConsoleColor.White);
            }
        }
    }

    // average count of eggs (age and weight)
    static void AverageEggsForWeightAndAge()
    {
        Console.Write("Введіть вагу курки: ");
        var weight = double.Parse(Console.ReadLine()!);

        Console.Write("Введіть вік курки: ");
        var age = int.Parse(Console.ReadLine()!);

        using (var context = new AppDbContext())
        {
            var average = context.Chickens
                .Where(c => c.Weight == weight && c.Age == age)
                .Average(c => c.EggsPerMonth);

            PrintMessage($"Середня кількість яєць для курей з вагою {weight} та віком {age}: {average}", ConsoleColor.White);
        }
    }

    // count of eggs and their price
    static void TotalEggsForDateRange()
    {
        const double eggPrice = 10; // price of one egg

        Console.Write("Введіть загальну кількість днів у діапазоні: ");
        var days = int.Parse(Console.ReadLine()!);

        using (var context = new AppDbContext())
        {
            var totalEggs = context.Chickens.Sum(c => c.EggsPerMonth);
            var totalValue = totalEggs * eggPrice;

            PrintMessage($"Загальна кількість яєць: {totalEggs}, Загальна вартість: {totalValue} грн", ConsoleColor.White);
        }
    }

    // chicken with the most eggs
    static void ChickenWithMostEggs()
    {
        using (var context = new AppDbContext())
        {
            var chicken = context.Chickens
                .OrderByDescending(c => c.EggsPerMonth)
                .FirstOrDefault();

            if (chicken != null)
            {
                PrintMessage($"Курка з найбільшою кількістю яєць - ID: {chicken.Id}, Яйця в місяць: {chicken.EggsPerMonth}", ConsoleColor.White);
            }
            else
            {
                PrintMessage("Немає даних про курей.", ConsoleColor.Red);
            }
        }
    }

    // count of chickens per every employee
    static void ChickensPerEmployee()
    {
        using (var context = new AppDbContext())
        {
            var employees = context.Employees
                .Select(e => new
                {
                    e.Name,
                    ChickenCount = e.Cages.Count(c => c.ChickenId != null)
                })
                .ToList();

            PrintHeader("=== Кількість курей у кожного працівника ===", ConsoleColor.Red);
            foreach (var employee in employees)
            {
                PrintMessage($"Працівник: {employee.Name}, Кількість курей: {employee.ChickenCount}", ConsoleColor.White);
            }
        }
    }

    // below average chicken
    static void ChickensBelowAverage()
    {
        using (var context = new AppDbContext())
        {
            var averageEggs = context.Chickens.Average(c => c.EggsPerMonth);
            var chickens = context.Chickens
                .Where(c => c.EggsPerMonth < averageEggs)
                .ToList();

            PrintHeader("=== Куриці з несучістю нижче середньої ===", ConsoleColor.Red);
            foreach (var chicken in chickens)
            {
                PrintMessage($"ID: {chicken.Id}, Яйця в місяць: {chicken.EggsPerMonth}", ConsoleColor.White);
            }
        }
    }

    // add chicken
    static void AddChicken()
    {
        Console.Write("Введіть вагу курки: ");
        var weight = double.Parse(Console.ReadLine()!);

        Console.Write("Введіть вік курки: ");
        var age = int.Parse(Console.ReadLine()!);

        Console.Write("Введіть кількість яєць на місяць: ");
        var eggs = int.Parse(Console.ReadLine()!);

        Console.Write("Введіть ID клітки: ");
        var cageId = int.Parse(Console.ReadLine()!);

        using (var context = new AppDbContext())
        {
            context.Chickens.Add(new Chicken
            {
                Weight = weight,
                Age = age,
                EggsPerMonth = eggs,
                CageId = cageId
            });
            context.SaveChanges();
            PrintMessage("✅ Курка додана.", ConsoleColor.Green);
        }
    }

    // delete chicken
    static void DeleteChicken()
    {
        Console.Write("Введіть ID курки для видалення: ");
        var id = int.Parse(Console.ReadLine()!);

        using (var context = new AppDbContext())
        {
            var chicken = context.Chickens.Find(id);
            if (chicken != null)
            {
                context.Chickens.Remove(chicken);
                context.SaveChanges();
                PrintMessage("🚫 Курка видалена.", ConsoleColor.Green);
            }
            else
            {
                PrintMessage("Курка з таким ID не знайдена.", ConsoleColor.Red);
            }
        }
    }

    // print header of menu
    static void PrintHeader(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    // create menu option
    static void PrintMenuOption(string number, string text)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($"[{number}] ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    // print message
    static void PrintMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
