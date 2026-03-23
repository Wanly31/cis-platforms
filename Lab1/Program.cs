namespace Lab1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("1. Тестування MagazineCollection");
            MagazineCollection collection = new MagazineCollection();
            collection.AddDefaults();
            
            Magazine customMag = new Magazine("Custom Magazine", new DateTime(2025, 1, 1), 750, Frequency.Weekly);
            customMag.AddArticles(new Article(new Person("Super", "Author", DateTime.Today), "Cool article", 5.0));
            collection.AddMagazines(customMag);

            Console.WriteLine("Початкова колекція:");
            Console.WriteLine(collection.ToString());

            Console.WriteLine("\nСортування за назвою (Title):");
            collection.SortByTitle();
            Console.WriteLine(collection.ToString());

            Console.WriteLine("\nСортування за датою виходу (ReleaseDate):");
            collection.SortByDate();
            Console.WriteLine(collection.ToString());

            Console.WriteLine("\nСортування за тиражем (Circulation):");
            collection.SortByCirculation();
            Console.WriteLine(collection.ToString());

            Console.WriteLine("\n2. Перевірка LINQ методів");
            Console.WriteLine($"Максимальний середній рейтинг: {collection.MaxAverageRating}");

            Console.WriteLine("\nЖурнали, що виходять щомісяця (Monthly):");
            foreach (var m in collection.MonthlyMagazines)
            {
                Console.WriteLine(m.ToShortString());
            }

            Console.WriteLine("\nЖурнали з рейтингом >= 4.0:");
            var grouped = collection.RatingGroup(4.0);
            foreach (var m in grouped)
            {
                Console.WriteLine(m.ToShortString());
            }

            Console.WriteLine("\n3. Тестування часу у TestCollections ");
            Console.WriteLine("Напишіть кількість елементів для генерації:");

            int number;
            while (!int.TryParse(Console.ReadLine(), out number) || number <= 0)
            {
                Console.WriteLine("Помилка! Введіть додатнє ціле число:");
            }

            Console.WriteLine($"Створюємо об'єкт TestCollections на {number} елементів");
            var testCollections = new TestCollections(number);
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

            Magazine firstMag = TestCollections.GenerateMagazine(0);
            Magazine centerMag = TestCollections.GenerateMagazine(number / 2);
            Magazine lastMag = TestCollections.GenerateMagazine(number - 1);
            Magazine notExistingMag = TestCollections.GenerateMagazine(number + 100);

            Magazine[] magsToTest = { firstMag, centerMag, lastMag, notExistingMag };
            string[] names = { "ПЕРШИЙ", "ЦЕНТРАЛЬНИЙ", "ОСТАННІЙ", "НЕ ІСНУЄ" };

            for (int i = 0; i < 4; i++)
            {
                Magazine currentMag = magsToTest[i];
                Edition currentKey = currentMag.EditionData;
                string currentStrKey = currentKey.ToString();

                Console.WriteLine($"\nШукаємо елемент '{names[i]}' ");

                // --- Стандартні колекції ---
                watch.Restart();
                testCollections.FindInListKeys(currentKey);
                watch.Stop();
                Console.WriteLine($"List<Edition>: {watch.ElapsedTicks} тіків");

                watch.Restart();
                testCollections.FindInListStrings(currentStrKey);
                watch.Stop();
                Console.WriteLine($"List<string>: {watch.ElapsedTicks} тіків");

                watch.Restart();
                testCollections.FindInDictByKey(currentKey);
                watch.Stop();
                Console.WriteLine($"Dictionary<Edition, Magazine> (key): {watch.ElapsedTicks} тіків");

                watch.Restart();
                testCollections.FindInDictByStringKey(currentStrKey);
                watch.Stop();
                Console.WriteLine($"Dictionary<string, Magazine> (key): {watch.ElapsedTicks} тіків");

                watch.Restart();
                testCollections.FindInDictByValue(currentMag);
                watch.Stop();
                Console.WriteLine($"Dictionary (value): {watch.ElapsedTicks} тіків");

                //Immutable колекції
                watch.Restart();
                testCollections.FindInImmutableListKeys(currentKey);
                watch.Stop();
                Console.WriteLine($"ImmutableList<Edition>: {watch.ElapsedTicks} тіків");

                watch.Restart();
                testCollections.FindInImmutableListStrings(currentStrKey);
                watch.Stop();
                Console.WriteLine($"ImmutableList<string>: {watch.ElapsedTicks} тіків");

                watch.Restart();
                testCollections.FindInImmutableKeyValue(currentKey);
                watch.Stop();
                Console.WriteLine($"ImmutableDictionary<Edition, Magazine> (key): {watch.ElapsedTicks} тіків");

                watch.Restart();
                testCollections.FindInImmutableStringValue(currentMag);
                watch.Stop();
                Console.WriteLine($"ImmutableDictionary (value): {watch.ElapsedTicks} тіків");

                //Sorted колекції
                watch.Restart();
                testCollections.FindInSortedListKeys(currentKey);
                watch.Stop();
                Console.WriteLine($"SortedList<Edition, Magazine> (key): {watch.ElapsedTicks} тіків");

                watch.Restart();
                testCollections.FindInSortedListStrings(currentStrKey);
                watch.Stop();
                Console.WriteLine($"SortedList<string, Magazine> (key): {watch.ElapsedTicks} тіків");

                watch.Restart();
                testCollections.FindInSortedDictKeyValue(currentKey);
                watch.Stop();
                Console.WriteLine($"SortedDictionary<Edition, Magazine> (key): {watch.ElapsedTicks} тіків");

                watch.Restart();
                testCollections.FindInSortedDictStringValue(currentMag);
                watch.Stop();
                Console.WriteLine($"SortedDictionary (value): {watch.ElapsedTicks} тіків");
            }

        }
    }
}