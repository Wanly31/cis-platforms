using System;
using System.Linq;

namespace Lab1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. Edition: Equals, references, hash codes");
            Edition ed1 = new Edition("Science Weekly", new DateTime(2026, 1, 15), 5000);
            Edition ed2 = new Edition("Science Weekly", new DateTime(2026, 1, 15), 5000);

            Console.WriteLine($"ReferenceEquals: {ReferenceEquals(ed1, ed2)}");
            Console.WriteLine($"Equals: {ed1.Equals(ed2)}");
            Console.WriteLine($"==: {ed1 == ed2}");
            Console.WriteLine($"Hash ed1: {ed1.GetHashCode()}");
            Console.WriteLine($"Hash ed2: {ed2.GetHashCode()}");

            Console.WriteLine("\n2. Edition: invalid Circulation ");
            try
            {
                Edition bad = new Edition("Bad Edition", DateTime.Today, -10);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\n3. Magazine with articles and editors ");
            Magazine mag = new Magazine("Tech Today", new DateTime(2026, 3, 1), 10000, Frequency.Monthly);

            mag.AddEditors(
                new Person("Ivan", "Petrov", new DateTime(1980, 5, 12)),
                new Person("Olena", "Koval", new DateTime(1990, 8, 25))
            );

            mag.AddArticles(
                new Article(new Person("Alice", "Smith", new DateTime(1985, 5, 10)), "Quantum Computing Advances", 9.2),
                new Article(new Person("Bob", "Jones", new DateTime(1990, 3, 22)), "AI in Medicine", 7.5),
                new Article(new Person("Carol", "White", new DateTime(1978, 11, 5)), "Space Exploration Today", 8.8),
                new Article(new Person("Dan", "Brown", new DateTime(1995, 1, 30)), "Computing in Education", 6.0)
            );

            Console.WriteLine(mag.ToString());

            Console.WriteLine(" 4. EditionData property ");
            Console.WriteLine(mag.EditionData);

            Console.WriteLine("\n5. DeepCopy test ");
            Magazine copy = (Magazine)mag.DeepCopy();

            mag.Frequency = Frequency.Weekly;
            mag.AddArticles(new Article(new Person("New", "Author", DateTime.Today), "New Article", 10.0));
            mag.AddEditors(new Person("Extra", "Editor", DateTime.Today));

            Console.WriteLine(" Original (after changes) ");
            Console.WriteLine(mag.ToString());
            Console.WriteLine("Copy (should be unchanged) ");
            Console.WriteLine(copy.ToString());

            Console.WriteLine("6. Articles with rating > 8.0 ");
            foreach (Article a in copy.ArticlesWithRatingAbove(8.0))
            {
                Console.WriteLine(a);
            }

            Console.WriteLine("\n7. Articles with 'Computing' in title ");
            foreach (Article a in copy.ArticlesWithTitleContaining("Computing"))
            {
                Console.WriteLine(a);
            }
        }
    }
}