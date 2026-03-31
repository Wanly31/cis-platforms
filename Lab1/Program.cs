namespace Lab1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

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