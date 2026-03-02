using System;
using System.Linq;

namespace Lab1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write array size:");
            string sizeArray = Console.ReadLine();

            int[] array = sizeArray.Split(' ').Select(int.Parse).ToArray();

            int nRows = array[0];
            int nCols = array[1];

            Person[] oneDimensional = new Person[nRows * nCols];
            Person[,] twoDimensional = new Person[nRows, nCols];
            Person[][] jaggedArray = new Person[nRows][];
            Person[][] jaggedDifferent = JaggedArray(nRows * nCols);

            for (int i = 0; i < oneDimensional.Length; i++)
                oneDimensional[i] = new Person();

            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nCols; j++)
                    twoDimensional[i, j] = new Person();

            for (int i = 0; i < nRows; i++)
            {
                jaggedArray[i] = new Person[nCols];
                for (int j = 0; j < nCols; j++)
                    jaggedArray[i][j] = new Person();
            }

            Console.WriteLine("Succesfull initialized ");
            Console.WriteLine($"nRows = {nRows}, nCols = {nCols}");

            int startOne = Environment.TickCount;
            for (int i = 0; i < oneDimensional.Length; i++)
                oneDimensional[i].Year = 2026;
            Console.WriteLine($"One-dimensional: {Environment.TickCount - startOne} ms");

            int startTwo = Environment.TickCount;
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nCols; j++)
                    twoDimensional[i, j].Year = 2026;
            Console.WriteLine($"Two-dimensional: {Environment.TickCount - startTwo} ms");

            int startJagged = Environment.TickCount;
            for (int i = 0; i < nRows; i++)
                for (int j = 0; j < nCols; j++)
                    jaggedArray[i][j].Year = 2026;
            Console.WriteLine($"Jagged equal: {Environment.TickCount - startJagged} ms");

            int startJaggedDiff = Environment.TickCount;
            for (int i = 0; i < jaggedDifferent.Length; i++)
                for (int j = 0; j < jaggedDifferent[i].Length; j++)
                    jaggedDifferent[i][j].Year = 2026;
            Console.WriteLine($"Jagged different: {Environment.TickCount - startJaggedDiff} ms");

            // MAGAZINE 
            Console.WriteLine("\n1. Magazine - ToShortString");
            Magazine mag = new Magazine();
            Console.WriteLine(mag.ToShortString());

            Console.WriteLine("\n2. Indexer:");
            Console.WriteLine(mag[Frequency.Weekly]);
            Console.WriteLine(mag[Frequency.Monthly]);
            Console.WriteLine(mag[Frequency.Yearly]);

            Console.WriteLine("\n3. Set properties:");
            mag.Title = "Science Today";
            mag.Frequency = Frequency.Weekly;
            mag.PublicationDate = new DateTime(2026, 2, 23);
            mag.Circulation = 5000;
            Console.WriteLine(mag);

            Console.WriteLine("\n4. Add articles:");
            Article a1 = new Article(new Person("Alice", "Smith", new DateTime(1985, 5, 10)), "Quantum Physics", 9.0);
            Article a2 = new Article(new Person("Bob", "Jones", new DateTime(1990, 3, 22)), "AI in Medicine", 8.5);
            Article a3 = new Article(new Person("Carol", "White", new DateTime(1978, 11, 5)), "Space Exploration", 9.5);

            mag.AddArticles(a1, a2, a3);
            Console.WriteLine(mag);

            // ARTICLE TEST

            Console.WriteLine("\n5. Article arrays test");

            int artRows = 500;
            int artCols = 500;
            int total = artRows * artCols;

            Article[] artOne = new Article[total];
            Article[,] artTwo = new Article[artRows, artCols];
            Article[][] artJagged = new Article[artRows][];
            Article[][] artJaggedDiff = CreateJaggedArticles(total);

            for (int i = 0; i < total; i++)
                artOne[i] = new Article();

            for (int i = 0; i < artRows; i++)
                for (int j = 0; j < artCols; j++)
                    artTwo[i, j] = new Article();

            for (int i = 0; i < artRows; i++)
            {
                artJagged[i] = new Article[artCols];
                for (int j = 0; j < artCols; j++)
                    artJagged[i][j] = new Article();
            }

            int tStart = Environment.TickCount;
            for (int i = 0; i < total; i++)
                artOne[i].Rating = 5;
            Console.WriteLine($"1D: {Environment.TickCount - tStart} ms");

            tStart = Environment.TickCount;
            for (int i = 0; i < artRows; i++)
                for (int j = 0; j < artCols; j++)
                    artTwo[i, j].Rating = 5;
            Console.WriteLine($"2D: {Environment.TickCount - tStart} ms");

            tStart = Environment.TickCount;
            for (int i = 0; i < artRows; i++)
                for (int j = 0; j < artCols; j++)
                    artJagged[i][j].Rating = 5;
            Console.WriteLine($"Jagged equal: {Environment.TickCount - tStart} ms");

            tStart = Environment.TickCount;
            for (int i = 0; i < artJaggedDiff.Length; i++)
                for (int j = 0; j < artJaggedDiff[i].Length; j++)
                    artJaggedDiff[i][j].Rating = 5;
            Console.WriteLine($"Jagged diff: {Environment.TickCount - tStart} ms");
        }

        static Person[][] JaggedArray(int totalElements)
        {
            int current = 0;
            int rows = 0;

            while (current < totalElements)
            {
                rows++;
                current += rows;
            }

            Person[][] array = new Person[rows][];
            current = 0;

            for (int i = 0; i < rows; i++)
            {
                int length = Math.Min(i + 1, totalElements - current);
                array[i] = new Person[length];

                for (int j = 0; j < length; j++)
                {
                    array[i][j] = new Person();
                    current++;
                }

                if (current >= totalElements)
                    break;
            }

            return array;
        }
        static Article[][] CreateJaggedArticles(int totalElements)
        {
            int current = 0;
            int rows = 0;

            while (current < totalElements)
            {
                rows++;
                current += rows;
            }

            Article[][] array = new Article[rows][];
            current = 0;

            for (int i = 0; i < rows; i++)
            {
                int length = Math.Min(i + 1, totalElements - current);
                array[i] = new Article[length];

                for (int j = 0; j < length; j++)
                {
                    array[i][j] = new Article();
                    current++;
                }

                if (current >= totalElements)
                    break;
            }

            return array;
        }
    }
}