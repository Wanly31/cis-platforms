using System.Threading.Tasks.Dataflow;

namespace DirectorySizeCalculator
{
    /// <summary>
    /// Результат аналізу одного каталогу
    /// </summary>
    record DirectoryResult(string Path, long Size, int FileCount, int Depth);

    class Program
    {
        // Загальні лічильники
        static long _totalBytes = 0;
        static int _totalFiles = 0;
        static int _totalDirs = 0;

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("║         ПІДРАХУНОК РОЗМІРУ КАТАЛОГУ          ║");
            Console.ResetColor();

            string rootPath;
            while (true)
            {
                Console.Write("📁 Введіть шлях до каталогу: ");
                string? input = Console.ReadLine()?.Trim().Trim('"');

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("   [ПОМИЛКА] Шлях не може бути порожнім. Спробуйте ще раз.");
                    Console.ResetColor();
                    continue;
                }

                if (!Directory.Exists(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"   [ПОМИЛКА] Каталог не існує: {input}");
                    Console.ResetColor();
                    continue;
                }

                rootPath = input;
                break;
            }

            Console.WriteLine(new string('─', 70));
            Console.WriteLine($"   Кореневий каталог : {rootPath}");
            Console.WriteLine(new string('─', 70));

            var startTime = DateTime.Now;

            int maxDop = Math.Min(Environment.ProcessorCount, 8);

            var options = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = maxDop,
                BoundedCapacity = 64
            };

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };

            // Блок 1: Отримує шлях до каталогу, розраховує його розмір у окремому потоці
            var calculateBlock = new TransformBlock<(string path, int depth), DirectoryResult>(
                input => CalculateDirectorySize(input.path, input.depth),
                options
            );

            // Блок 2: Агрегує і виводить результат на екран
            var printBlock = new ActionBlock<DirectoryResult>(
                result => PrintResult(result),
                new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 1 }
            );

            calculateBlock.LinkTo(printBlock, linkOptions);

            // Кореневий каталог
            await calculateBlock.SendAsync((rootPath, 0));

            // Рекурсивно всі підкаталоги
            await EnqueueSubdirectories(rootPath, calculateBlock, depth: 1);

            // Сигнал завершення
            calculateBlock.Complete();
            await printBlock.Completion;

            var elapsed = DateTime.Now - startTime;

            Console.WriteLine(new string('═', 70));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("📊  ПІДСУМОК");
            Console.ResetColor();
            Console.WriteLine($"   Каталогів  : {_totalDirs,8}");
            Console.WriteLine($"   Файлів     : {_totalFiles,8}");
            Console.WriteLine($"   Загальний розмір : {FormatSize(Interlocked.Read(ref _totalBytes))}");
            Console.WriteLine($"   Час виконання    : {elapsed.TotalSeconds:F2} сек  (потоків: {maxDop})");
            Console.WriteLine(new string('═', 70));

            Console.Write("\n🔄 Проаналізувати інший каталог? (y/n): ");
            if (Console.ReadLine()?.Trim().ToLower() == "y")
            {
                _totalBytes = 0;
                _totalFiles = 0;
                _totalDirs = 0;
                Console.WriteLine();
                await Main(args);
            }
        }

        //Рекурсивне додавання підкаталогів у блок
        static async Task EnqueueSubdirectories(
            string path,
            ITargetBlock<(string, int)> block,
            int depth)
        {
            string[] subDirs;
            try { subDirs = Directory.GetDirectories(path); }
            catch (UnauthorizedAccessException) { return; }
            catch (IOException) { return; }

            foreach (var dir in subDirs)
            {
                await block.SendAsync((dir, depth));
                await EnqueueSubdirectories(dir, block, depth + 1);
            }
        }

        //Обчислення розміру каталогу
        static DirectoryResult CalculateDirectorySize(string path, int depth)
        {
            long size = 0;
            int fileCount = 0;

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  ⚙  [TID {Thread.CurrentThread.ManagedThreadId,2}] Обробка: {TruncatePath(path, 55)}");
            Console.ResetColor();

            try
            {
                foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        var info = new FileInfo(file);
                        size += info.Length;
                        fileCount++;
                    }
                    catch { /* файл недоступний */ }
                }
            }
            catch { /* каталог недоступний */ }

            // Атомарне оновлення глобальних лічильників
            Interlocked.Add(ref _totalBytes, size);
            Interlocked.Add(ref _totalFiles, fileCount);
            Interlocked.Increment(ref _totalDirs);

            return new DirectoryResult(path, size, fileCount, depth);
        }

        // Вивід результату
        static void PrintResult(DirectoryResult r)
        {
            string indent = new string(' ', r.Depth * 2);
            string name = r.Depth == 0 ? r.Path : Path.GetFileName(r.Path);

            Console.ForegroundColor = r.Size > 100 * 1024 * 1024   // > 100 MB → червоний
                ? ConsoleColor.Red
                : r.Size > 10 * 1024 * 1024                        // > 10 MB  → жовтий
                    ? ConsoleColor.Yellow
                    : ConsoleColor.Green;

            Console.Write($"{indent}📂 {name,-45}");
            Console.ResetColor();
            Console.Write($"  {FormatSize(r.Size),10}");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"   ({r.FileCount} файл(ів))");
            Console.ResetColor();
        }

        static string FormatSize(long bytes) => bytes switch
        {
            >= 1_099_511_627_776 => $"{bytes / 1_099_511_627_776.0:F2} ТБ",
            >= 1_073_741_824 => $"{bytes / 1_073_741_824.0:F2} ГБ",
            >= 1_048_576 => $"{bytes / 1_048_576.0:F2} МБ",
            >= 1_024 => $"{bytes / 1_024.0:F2} КБ",
            _ => $"{bytes} Б"
        };

        static string TruncatePath(string path, int maxLen) =>
            path.Length <= maxLen ? path : "…" + path[^(maxLen - 1)..];
    }
}