using Lab7;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
{
    Console.WriteLine("Консоль не працює по неділях.");
    return;
}

var rnd = new Random();
var coupleService = new CoupleService();

string[] maleNames = { "Олександр", "Іван", "Максим", "Сергій", "Артем", "Дмитро" };
string[] femaleNames = { "Анна", "Марія", "Олена", "Дарія", "Юлія", "Катерина" };

Human GetRandomPerson()
{
    int type = rnd.Next(5);
    Human h;
    if (type < 2)
    {
        h = type == 0 ? new Student() : new Botan();
        h.Name = maleNames[rnd.Next(maleNames.Length)];
    }
    else
    {
        h = type == 2 ? new Girl() : (type == 3 ? new PrettyGirl() : new SmartGirl());
        h.Name = femaleNames[rnd.Next(femaleNames.Length)];
    }
    return h;
}

Console.WriteLine("Натисніть Enter для генерації 2 пар людей, Q або F10 для виходу.");

while (true)
{
    var key = Console.ReadKey(true);
    if (key.Key == ConsoleKey.Q || key.Key == ConsoleKey.F10)
        break;
    
    if (key.Key != ConsoleKey.Enter)
        continue;

    Console.WriteLine("\n=============================");
    
    ProcessPair(1);
    ProcessPair(2);
}

void ProcessPair(int pairNumber)
{
    Console.WriteLine($">>> Пара {pairNumber}:");
    Human h1 = GetRandomPerson();
    Human h2 = GetRandomPerson();
    
    Console.WriteLine($"{h1.Name} ({h1.GetType().Name}) та {h2.Name} ({h2.GetType().Name})");

    try
    {
        // Виклик методу Couple
        IHasName? result = coupleService.Couple(h1, h2);
        
        // Виведення результату поза функцією Couple
        if (result != null)
        {
            string childType = result.GetType().Name;
            
            string patronymic = "";
            var pProp = result.GetType().GetProperty("Patronymic");
            if (pProp != null)
            {
                patronymic = (string?)pProp.GetValue(result) ?? "";
            }
            
            string fullName = string.IsNullOrWhiteSpace(patronymic) ? result.Name : $"{result.Name} {patronymic}";
            Console.WriteLine($"Успіх! Створено новий об'єкт");
            Console.WriteLine($"Тип: {childType}");
            Console.WriteLine($"Ім'я: {fullName}");
        }
        else
        {
            Console.WriteLine("Пара розійшлася (немає взаємної симпатії)");
        }
    }
    catch (SameGenderException ex)
    {
        // Перехоплення винятку поза функцією Couple та виведення на консоль
        Console.WriteLine($"Виняток! {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($" Помилка: {ex.Message}");
    }
    Console.WriteLine();
}
