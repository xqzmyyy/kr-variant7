using System;
using System.Linq;
using farm.Data;
using farm.Models;

class Program 
{
    static void Main(string[] args) 
    {
        using (var context = new AppDbContext()) 
        {
            // init new database
            context.SeedData();
        }

        while (true) {
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

    static void ListAllChickens() 
    {
        using (var context = new AppDbContext()) 
        {
            PrintHeader("=== Список всіх курей ===", ConsoleColor.Red);
            var chickens = context.Chickens.ToList();
            foreach (var chicken in chickens) {
                PrintMessage($"ID: {chicken.Id}, Вага: {chicken.Weight}, Вік: {chicken.Age}, Яйця в місяць: {chicken.EggsPerMonth}, Клітка ID: {chicken.CageId}", ConsoleColor.White);
            }
        }
    }

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

    static void ListAllCages() 
    {
        using (var context = new AppDbContext()) 
        {
            PrintHeader("=== Список всіх кліток ===", ConsoleColor.Red);
            var cages = context.Cages.ToList();
            foreach (var cage in cages) 
            {
                PrintMessage($"ID: {cage.Id}, Курица ID: {(cage.ChickenId.HasValue ? cage.ChickenId.Value.ToString() : "Порожня")}, Працівник ID: {cage.EmployeeId}, Дата: {cage.Date.ToShortDateString()}, Яйце знесено: {cage.IsEggLaid}", ConsoleColor.White);
            }
        }
    }

    static void AverageEggsForWeightAndAge() 
    {
        Console.Write("Введіть вагу курки: ");
        if (!double.TryParse(Console.ReadLine(), out var weight)) 
        {
            PrintMessage("❌ Неправильний формат ваги!", ConsoleColor.Red);
            return;
        }

        Console.Write("Введіть вік курки: ");
        if (!int.TryParse(Console.ReadLine(), out var age)) 
        {
            PrintMessage("❌ Неправильний формат віку!", ConsoleColor.Red);
            return;
        }

        using (var context = new AppDbContext()) 
        {
            var chickens = context.Chickens.Where(c => c.Weight == weight && c.Age == age).ToList();
            if (!chickens.Any()) 
            {
                PrintMessage("Курей з такими параметрами не знайдено.", ConsoleColor.Yellow);
                return;
            }
            var average = chickens.Average(c => c.EggsPerMonth);
            PrintMessage($"Середня кількість яєць для курей з вагою {weight} та віком {age}: {average}", ConsoleColor.White);
        }
    }

    static void TotalEggsForDateRange() 
    {
        const double eggPrice = 10;

        Console.Write("Введіть загальну кількість днів у діапазоні: ");
        if (!int.TryParse(Console.ReadLine(), out var days)) 
        {
            PrintMessage("❌ Неправильний формат кількості днів!", ConsoleColor.Red);
            return;
        }

        using (var context = new AppDbContext()) 
        {
            var totalEggs = context.Cages
                .Where(c => c.Date >= DateTime.Today.AddDays(-days) && c.IsEggLaid)
                .Count();
            var totalValue = totalEggs * eggPrice;

            PrintMessage($"Загальна кількість яєць: {totalEggs}, Загальна вартість: {totalValue} грн", ConsoleColor.White);
        }
    }

    static void ChickenWithMostEggs() 
    {
        using (var context = new AppDbContext()) 
        {
            var chicken = context.Chickens
                .OrderByDescending(c => c.EggsPerMonth)
                .FirstOrDefault();

            if (chicken == null) 
            {
                PrintMessage("Немає даних про курей.", ConsoleColor.Red);
                return;
            }

            PrintMessage($"Курка з найбільшою кількістю яєць - ID: {chicken.Id}, Яйця в місяць: {chicken.EggsPerMonth}", ConsoleColor.White);
        }
    }

    static void ChickensPerEmployee() 
    {
        using (var context = new AppDbContext()) 
        {
            var employees = context.Employees
                .Select(e => new {
                    e.Name,
                    ChickenCount = e.Cages.Count(c => c.ChickenId != null)
                }).ToList();

            PrintHeader("=== Кількість курей у кожного працівника ===", ConsoleColor.Red);
            foreach (var employee in employees) 
            {
                PrintMessage($"Працівник: {employee.Name}, Кількість курей: {employee.ChickenCount}", ConsoleColor.White);
            }
        }
    }

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

    static void AddChicken() 
    {
        Console.Write("Введіть вагу курки: ");
        if (!double.TryParse(Console.ReadLine(), out var weight)) 
        {
            PrintMessage("❌ Неправильний формат ваги!", ConsoleColor.Red);
            return;
        }

        Console.Write("Введіть вік курки: ");
        if (!int.TryParse(Console.ReadLine(), out var age)) 
        {
            PrintMessage("❌ Неправильний формат віку!", ConsoleColor.Red);
            return;
        }

        Console.Write("Введіть кількість яєць на місяць: ");
        if (!int.TryParse(Console.ReadLine(), out var eggs)) 
        {
            PrintMessage("❌ Неправильний формат кількості яєць!", ConsoleColor.Red);
            return;
        }

        Console.Write("Введіть ID клітки: ");
        if (!int.TryParse(Console.ReadLine(), out var cageId)) 
        {
            PrintMessage("❌ Неправильний формат ID клітки!", ConsoleColor.Red);
            return;
        }

        using (var context = new AppDbContext()) 
        {
            context.Chickens.Add(new Chicken {
                Weight = weight,
                Age = age,
                EggsPerMonth = eggs,
                CageId = cageId
            });
            context.SaveChanges();
            PrintMessage("✅ Курка додана.", ConsoleColor.Green);
        }
    }

    static void DeleteChicken() 
    {
        Console.Write("Введіть ID курки для видалення: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) 
        {
            PrintMessage("❌ Неправильний формат ID!", ConsoleColor.Red);
            return;
        }

        using (var context = new AppDbContext()) 
        {
            var chicken = context.Chickens.Find(id);
            if (chicken == null) 
            {
                PrintMessage("Курка з таким ID не знайдена.", ConsoleColor.Yellow);
                return;
            }
            context.Chickens.Remove(chicken);
            context.SaveChanges();
            PrintMessage("🚫 Курка видалена.", ConsoleColor.Green);
        }
    }

    static void PrintHeader(string text, ConsoleColor color) 
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    static void PrintMenuOption(string number, string text) 
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write($"[{number}] ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    static void PrintMessage(string message, ConsoleColor color) 
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
