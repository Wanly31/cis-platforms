namespace Lab1
{
    using System;
    using System.Diagnostics;

    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Оберіть, що тестувати:");
            Console.WriteLine("1 - Події магазинів (Lab Event-driven)");
            Console.WriteLine("2 - Продуктивність колекцій (Standard, Immutable, Sorted)");
            Console.WriteLine("3 - Серіалізація та файли (Save, Load, AddFromConsole)");
            Console.Write("Ваш вибір: ");
            
            var input = Console.ReadLine();
            if (input == "1")
            {
                TestEvents();
            }
            else if (input == "2")
            {
                TestCollectionPerformance();
            }
            else if (input == "3")
            {
                TestFiles();
            }
            else
            {
                Console.WriteLine("Невірний вибір. Виконую тест роботи з файлами за замовчуванням.");
                TestFiles();
            }
        }

        static void TestFiles()
        {
            Console.WriteLine("\nТЕСТУВАННЯ РОБОТИ З ФАЙЛАМИ ТА СЕРІАЛІЗАЦІЄЮ");

            Magazine originalMagazine = new Magazine("Супер Журнал", DateTime.Now, 5000, Frequency.Weekly);
            Person author1 = new Person("Іван", "Франко", new DateTime(1856, 8, 27));
            originalMagazine.AddArticles(new Article(author1, "Перша стаття", 9.5));

            Console.WriteLine("\nОригінальний об'єкт (до збереження)");
            Console.WriteLine(originalMagazine);

            string file1 = "mag_instance.json";
            string file2 = "mag_static.json";

            // Тест 1: Instance Save
            Console.WriteLine("\n 1. Тест Instance Save");
            bool isSaved1 = originalMagazine.Save(file1);
            Console.WriteLine($"Результат збереження у {file1}: {(isSaved1 ? "Успіх" : "Неуспіх")}");

            // Тест 2: Static Save
            Console.WriteLine("\n 2. Тест Static Save ");
            bool isSaved2 = Magazine.Save(file2, originalMagazine);
            Console.WriteLine($"Результат статичного збереження у {file2}: {(isSaved2 ? "Успіх" : "Неуспіх")}");

            // Тест 3: Instance Load
            Console.WriteLine("\n 3. Тест Instance Load");
            Magazine loadedMag1 = new Magazine();
            bool isLoaded1 = loadedMag1.Load(file1);
            Console.WriteLine($"Результат завантаження з {file1}: {(isLoaded1 ? "Успіх" : "Неуспіх")}");
            if (isLoaded1) Console.WriteLine(loadedMag1);

            // Тест 4: Static Load
            Console.WriteLine("\n4. Тест Static Load");
            Magazine loadedMag2 = new Magazine();
            bool isLoaded2 = Magazine.Load(file2, loadedMag2);
            Console.WriteLine($"Результат статичного завантаження з {file2}: {(isLoaded2 ? "Успіх" : "Неуспіх")}");
            if (isLoaded2) Console.WriteLine(loadedMag2);

            // Тест 5: AddFromConsole
            Console.WriteLine("\n5. Тест AddFromConsole");
            Console.WriteLine("Будь ласка, введіть дані для нової статті (Формат: Назва|Ім'я|Прізвище|1990-01-01|9.9) або введіть будь-що неправильне для пропуску:");
            bool isAddedFromConsole = loadedMag1.AddFromConsole();
            
            Console.WriteLine($"\nРезультат додавання з консолі: {(isAddedFromConsole ? "Успіх" : "Неуспіх")}");
            if (isAddedFromConsole)
            {
                Console.WriteLine("\nЖурнал після додавання нової статті");
                Console.WriteLine(loadedMag1);
            }
        }

        static void TestCollectionPerformance()
        {
            Console.WriteLine("\nТЕСТУВАННЯ ПРОДУКТИВНОСТІ КОЛЕКЦІЙ");
            
            int count = 100000;
            Console.WriteLine($"Ініціалізація колекцій з {count} елементів...\n");
            
            TestCollections testCollections = new TestCollections(count);

            Magazine firstMag = TestCollections.GenerateMagazine(1);
            Magazine middleMag = TestCollections.GenerateMagazine(count / 2);
            Magazine lastMag = TestCollections.GenerateMagazine(count - 1);
            Magazine notInMag = TestCollections.GenerateMagazine(count + 10);

            Edition firstKey = firstMag.EditionData;
            Edition middleKey = middleMag.EditionData;
            Edition lastKey = lastMag.EditionData;
            Edition notInKey = notInMag.EditionData;

            string firstStr = firstKey.ToString();
            string middleStr = middleKey.ToString();
            string lastStr = lastKey.ToString();
            string notInStr = notInKey.ToString();

            // Element to search: 0 - First, 1 - Middle, 2 - Last, 3 - Not in collection
            var testCases = new (string Name, Edition Key, string Str, Magazine Val)[]
            {
                ("Перший елемент", firstKey, firstStr, firstMag),
                ("Центральний елемент", middleKey, middleStr, middleMag),
                ("Останній елемент", lastKey, lastStr, lastMag),
                ("Елемент, якого немає", notInKey, notInStr, notInMag)
            };

            foreach (var testCase in testCases)
            {
                Console.WriteLine($"\n--- Пошук: {testCase.Name} ---");

                // List<Edition>
                MeasureTime("List<Edition> (Keys)", () => testCollections.FindInListKeys(testCase.Key));
                
                // List<string>
                MeasureTime("List<string> (Strings)", () => testCollections.FindInListStrings(testCase.Str));
                
                // Dictionary<Edition, Magazine>
                MeasureTime("Dictionary by Key (Edition)", () => testCollections.FindInDictByKey(testCase.Key));
                MeasureTime("Dictionary by String Key", () => testCollections.FindInDictByStringKey(testCase.Str));
                MeasureTime("Dictionary by Value (Magazine)", () => testCollections.FindInDictByValue(testCase.Val));

                // ImmutableList<Edition>
                MeasureTime("ImmutableList<Edition> (Keys)", () => testCollections.FindInImmutableListKeys(testCase.Key));

                // ImmutableList<string>
                MeasureTime("ImmutableList<string> (Strings)", () => testCollections.FindInImmutableListStrings(testCase.Str));

                // ImmutableDictionary
                MeasureTime("ImmutableDictionary by Key", () => testCollections.FindInImmutableKeyValue(testCase.Key));
                MeasureTime("ImmutableDictionary by Value", () => testCollections.FindInImmutableStringValue(testCase.Val));

                // SortedList
                MeasureTime("SortedList by Key", () => testCollections.FindInSortedListKeys(testCase.Key));
                MeasureTime("SortedList by String Key", () => testCollections.FindInSortedListStrings(testCase.Str));

                // SortedDictionary
                MeasureTime("SortedDictionary by Key", () => testCollections.FindInSortedDictKeyValue(testCase.Key));
                MeasureTime("SortedDictionary by Value", () => testCollections.FindInSortedDictStringValue(testCase.Val));
            }
        }

        static void MeasureTime(string operationName, Action action)
        {
            var stopwatch = Stopwatch.StartNew();
            action();
            stopwatch.Stop();
            Console.WriteLine($"{operationName,-35}: {stopwatch.ElapsedTicks} ticks");
        }

        static void TestEvents()
        {
            Console.WriteLine("\nТЕСТУВАННЯ ПОДІЙ МАГАЗИНІВ");

            // 1. Створюємо дві колекції
            MagazineCollection coll1 = new MagazineCollection { Name = "Перша Супер Колекція" };
            MagazineCollection coll2 = new MagazineCollection { Name = "Друга Mega Колекція" };

            // 2. Створюємо два об'єкти Listener
            Listener listener1 = new Listener();
            Listener listener2 = new Listener();

            // 3. Підписуємо Listener 1 тільки на Колекцію 1
            coll1.MagazineAdded += listener1.HandleMagazineEvent;
            coll1.MagazineReplaced += listener1.HandleMagazineEvent;

            // Підписуємо Listener 2 на ОБИДВІ колекції
            coll1.MagazineAdded += listener2.HandleMagazineEvent;
            coll1.MagazineReplaced += listener2.HandleMagazineEvent;
            coll2.MagazineAdded += listener2.HandleMagazineEvent;
            coll2.MagazineReplaced += listener2.HandleMagazineEvent;

            // 4. Генеруємо події
            Console.WriteLine("Вносимо зміни у колекції...");
            coll1.AddDefaults();
            coll2.AddMagazines(new Magazine("Новий журнал для другої колекції", DateTime.Now, 100, Frequency.Monthly));

            // Змінюємо 1-м шляхом (використовуючи метод Replace)
            coll1.Replace(0, new Magazine("Замінений через Replace", DateTime.Now, 200, Frequency.Weekly));

            // Змінюємо 2-м шляхом (використовуючи індексатор)
            coll2[0] = new Magazine("Замінений через Індексатор", DateTime.Now, 300, Frequency.Yearly);

            // 5. Виводимо накопичені логи з обох Listener'ів
            Console.WriteLine("\nДані з об'єкта Listener 1 (слухає тільки 1-шу колекцію) ");
            Console.WriteLine(listener1.ToString());

            Console.WriteLine("Дані з об'єкта Listener 2 (слухає обидві колекції)");
            Console.WriteLine(listener2.ToString());
        }
    }
}