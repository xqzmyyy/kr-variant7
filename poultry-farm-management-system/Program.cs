using System;
using System.Linq;
using farm.Repositories;
using farm.Models;
using farm.Data;

class Program
{
    static void Main(string[] args)
    {

        using (var context = new AppDbContext())
        {
            context.Database.EnsureCreated();
            context.SeedData();
        }

        var menuOptions = new (string Description, Action Method)[]
        {
            ("📋 Список всіх курей", ListAllChickens),
            ("📊 Сводна інформація про птахофабрику", FarmSummary),
            ("👷 Список всіх працівників", ListAllEmployees),
            ("🛖 Список всіх кліток", ListAllCages),
            ("🥚 Середня кількість яєць для курей заданої ваги та віку", AverageEggsForWeightAndAge),
            ("📅 Загальна кількість яєць за діапазон днів та їхня вартість", TotalEggsForDateRange),
            ("🏆 Клітка з куркою, яка знесла найбільше яєць", CageWithTopChicken),
            ("👨‍🌾 Кількість яєць, зібраних кожним працівником", EggsCollectedByEmployees),
            ("⚠️ Куриці з несучістю нижче середнього рівня по фабриці", ChickensBelowAverage),
            ("➕ Додати нову курку", AddChicken),
            ("❌ Видалити курку", DeleteChicken),
            ("🚪 Вийти", () => { PrintMessage("До побачення! 🚪", ConsoleColor.Cyan); Environment.Exit(0); })
        };

        while (true)
        {
            Console.Clear();
            PrintHeader("=== Меню ===", ConsoleColor.Red);

            for (int i = 0; i < menuOptions.Length; i++)
            {
                PrintMenuOption((i + 1).ToString(), menuOptions[i].Description);
            }

            Console.WriteLine();
            PrintChickenArt();

            Console.Write("\nВаш вибір: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            var input = Console.ReadLine();
            Console.ResetColor();

            Console.Clear();

            if (int.TryParse(input, out var choice) && choice >= 1 && choice <= menuOptions.Length)
            {
                menuOptions[choice - 1].Method.Invoke();
            }
            else
            {
                PrintMessage("Неправильний вибір. Спробуйте ще раз. ❌", ConsoleColor.Red);
            }

            PrintMessage("\nНатисніть будь-яку кнопку, щоб повернутися до меню...", ConsoleColor.Yellow);
            Console.ReadKey();
        }
    }


    // get list of all chickens
    static void ListAllChickens()
    {
        var chickens = ChickenRepository.GetAllChickens();
        PrintHeader("=== Список всіх курей ===", ConsoleColor.Red);

        foreach (var chicken in chickens)
        {
            var cageInfo = chicken.Cage != null
                ? $"ID Клітки: {chicken.Cage.Id}, Закріплено за працівником ID: {chicken.Cage.EmployeeId}, Яйце знесено: {chicken.Cage.IsEggLaid}"
                : "Клітка: Немає інформації";

            PrintMessage(
                $"ID: {chicken.Id}, Вага: {chicken.Weight}, Вік: {chicken.Age}, Несучість (яєць/міс): {chicken.EggsPerMonth}, Порода: {chicken.Breed}, {cageInfo}",
                ConsoleColor.White
            );
        }
    }

    // farm info
    static void FarmSummary()
    {
        var chickens = ChickenRepository.GetAllChickens();
        var employees = EmployeeRepository.GetAllEmployees();
        var cages = CageRepository.GetAllCages();

        var totalChickens = chickens.Count;
        var totalEmployees = employees.Count;
        var totalCages = cages.Count;
        var averageEggs = chickens.Any() ? chickens.Average(c => c.EggsPerMonth) : 0;

        PrintHeader("=== Сводна інформація про птахофабрику ===", ConsoleColor.Red);
        PrintMessage($"Загальна кількість курей: {totalChickens}", ConsoleColor.White);
        PrintMessage($"Загальна кількість працівників: {totalEmployees}", ConsoleColor.White);
        PrintMessage($"Загальна кількість кліток: {totalCages}", ConsoleColor.White);
        PrintMessage($"Середня кількість яєць на курку: {averageEggs:F2}", ConsoleColor.White);
    }

    // get employees
    static void ListAllEmployees()
    {
        var employees = EmployeeRepository.GetAllEmployees();
        PrintHeader("=== Список всіх працівників ===", ConsoleColor.Red);
        foreach (var employee in employees)
        {
            var cageCount = employee.Cages?.Count ?? 0;
            PrintMessage($"ID: {employee.Id}, Ім'я: {employee.Name}, Зарплата: {employee.Salary}, Кількість кліток: {cageCount}", ConsoleColor.White);
        }
    }

    // get cages
    static void ListAllCages()
    {
        var cages = CageRepository.GetAllCages();
        PrintHeader("=== Список всіх кліток ===", ConsoleColor.Red);
        foreach (var cage in cages)
        {
            PrintMessage($"ID: {cage.Id}, Курица ID: {(cage.ChickenId.HasValue ? cage.ChickenId.Value.ToString() : "Порожня")}, Працівник ID: {cage.EmployeeId}, Дата: {cage.Date.ToShortDateString()}, Яйце знесено: {cage.IsEggLaid}", ConsoleColor.White);
        }
    }

    // average eggs for age and weight
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

        var chickens = ChickenRepository.GetChickensByWeightAndAge(weight, age);
        if (!chickens.Any())
        {
            PrintMessage("Курей з такими параметрами не знайдено.", ConsoleColor.Yellow);
            return;
        }
        var average = chickens.Average(c => c.EggsPerMonth);
        PrintMessage($"Середня кількість яєць для курей з вагою {weight} та віком {age}: {average}", ConsoleColor.White);
    }

    // total eggs for date range
    static void TotalEggsForDateRange()
    {
        const double eggPrice = 10;

        Console.Write("Введіть загальну кількість днів у діапазоні: ");
        if (!int.TryParse(Console.ReadLine(), out var days))
        {
            PrintMessage("❌ Неправильний формат кількості днів!", ConsoleColor.Red);
            return;
        }

        var cages = CageRepository.GetAllCages()
            .Where(c => c.Date >= DateTime.Today.AddDays(-days) && c.IsEggLaid)
            .ToList();
        var totalEggs = cages.Count;
        var totalValue = totalEggs * eggPrice;

        PrintMessage($"Загальна кількість яєць: {totalEggs}, Загальна вартість: {totalValue} грн", ConsoleColor.White);
    }

    // cage with chicken with most count of eggs
    static void CageWithTopChicken()
    {
        var cage = CageRepository.GetCageWithMostEggs();
        if (cage == null)
        {
            PrintMessage("Немає даних про клітки.", ConsoleColor.Red);
            return;
        }

        PrintHeader("=== Клітка з найпродуктивнішою куркою ===", ConsoleColor.Red);
        PrintMessage($"Клітка ID: {cage.Id}, Кількість яєць в місяць: {cage.Chicken?.EggsPerMonth ?? 0}", ConsoleColor.White);
    }

    // count of eggs per every employee
    static void EggsCollectedByEmployees()
    {
        var employeeData = EmployeeRepository.GetEggsCollectedByEmployees();
        PrintHeader("=== Кількість яєць, зібраних кожним працівником ===", ConsoleColor.Red);
        foreach (var entry in employeeData)
        {
            PrintMessage($"Працівник: {entry.Key.Name}, Яєць зібрано: {entry.Value}", ConsoleColor.White);
        }
    }

    // chickens with lowest eggs
    static void ChickensBelowAverage()
    {
        var chickens = ChickenRepository.GetChickensBelowAverage();
        PrintHeader("=== Куриці з несучістю нижче середньої ===", ConsoleColor.Red);
        foreach (var chicken in chickens)
        {
            PrintMessage($"ID: {chicken.Id}, Яйця в місяць: {chicken.EggsPerMonth}", ConsoleColor.White);
        }
    }

    // add new chicken
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

        Console.Write("Введіть породу курки: ");
        var breed = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(breed))
        {
            breed = "Невідома порода"; // default value
        }

        Console.Write("Введіть ID клітки: ");
        if (!int.TryParse(Console.ReadLine(), out var cageId))
        {
            PrintMessage("❌ Неправильний формат ID клітки!", ConsoleColor.Red);
            return;
        }

        var chicken = new Chicken
        {
            Weight = weight,
            Age = age,
            EggsPerMonth = eggs,
            Breed = breed,
            CageId = cageId
        };

        ChickenRepository.AddChicken(chicken);
        PrintMessage("✅ Курка додана.", ConsoleColor.Green);
    }

    // delete chicken
    static void DeleteChicken()
    {
        Console.Write("Введіть ID курки для видалення: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            PrintMessage("❌ Неправильний формат ID!", ConsoleColor.Red);
            return;
        }

        var success = ChickenRepository.DeleteChicken(id);
        if (!success)
        {
            PrintMessage("Курка з таким ID не знайдена.", ConsoleColor.Yellow);
            return;
        }

        PrintMessage("🚫 Курка видалена.", ConsoleColor.Green);
    }



    // console fucntions (print)
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

    static void PrintChickenArt()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(@"
        __//
        /.__.\
        \ \/ /
    '__/    \
    \-      )
    \_____/
        ");
        Console.ResetColor();
    }
}
