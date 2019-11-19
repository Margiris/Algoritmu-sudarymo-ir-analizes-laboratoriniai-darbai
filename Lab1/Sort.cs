using System;
using System.Diagnostics;
using System.IO;

// ReSharper disable InconsistentNaming

namespace Lab1
{
    public class Sort
    {
        private static Stopwatch _stopwatch;

        protected static long ComparisonCount;
        protected static long SwapCount;
        private const int RunCount = 8;

        private const string Filename = @"file.dat";
        private const string a_Filename = @"a.dat";
        private const string t_Filename = @"t.dat";
        private const string count_Filename = @"count.dat";
        private const string pref_Filename = @"pref.dat";
        private const string results_Filename = @"results.log";

        /// <summary>
        /// Draws progress bar in current console line.
        /// Author - smr5 @ Stack Overflow.
        /// Edited by me to display comparisons & swaps.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected static void DrawTextProgressBar(int progress, int total)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 48;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            var onechunk = 46.0f / total;

            //draw filled part
            var position = 1;

            for (var i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (var i = position; i <= 47; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 51;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("{0,10}{1,10}{2,16}{3,10}{4,22}",
                progress, total, ComparisonCount, SwapCount, _stopwatch.ElapsedMilliseconds);
        }

        private static void LogResults(string title, int amount)
        {
            var groups = "";
            if (title.Contains("Radix"))
                groups = RadixSort.GroupLength.ToString();

            using (var resultStreamWriter = new StreamWriter(results_Filename, true))
            {
                resultStreamWriter.WriteLine("{0,-36}{1,10}{2,16}{3,10}{4,22}{5,3}",
                    title, amount, ComparisonCount, SwapCount, _stopwatch.ElapsedMilliseconds, groups);
            }
        }

        public static void TestArrayRAM(int amount, int step, int seed,
            Action<Array, ArrayLong, ArrayLong, ArrayLong, ArrayLong> algorithm)
        {
            // ReSharper disable once PossibleNullReferenceException
            var title = " " + algorithm.Method.DeclaringType.Name + ": Array in RAM";

            Console.WriteLine(new string('_', 119));
            Console.WriteLine("{0,-34}{1,13}{2,14}{3,10}{4,16}{5,10}{6,22}",
                title, " Progress", "Current", "Total", "Comparisons", "Swaps", "Elapsed time (ms)");

            for (var i = 0; i < RunCount; i++)
            {
                ComparisonCount = 0;
                SwapCount = 0;

                var sample = new ArrayRAM(amount, seed);
                var a = new ArrayLongRAM(amount);
                var t = new ArrayLongRAM(amount);
                var count = new ArrayLongRAM(1 << RadixSort.GroupLength);
                var pref = new ArrayLongRAM(1 << RadixSort.GroupLength);

                _stopwatch = Stopwatch.StartNew();
                algorithm(sample, a, t, count, pref);
                _stopwatch.Stop();

                DrawTextProgressBar(amount, amount);
                Console.WriteLine();
                LogResults(title, amount);

                //sample.Print();


                amount *= step;
            }
        }

        public static void TestListRAM(int amount, int step, int seed,
            Action<LinkedList, ArrayLong, ArrayLong, ArrayLong, ArrayLong> algorithm)
        {
            // ReSharper disable once PossibleNullReferenceException
            var title = " " + algorithm.Method.DeclaringType.Name + ": Linked list in RAM";

            Console.WriteLine(new string('_', 119));
            Console.WriteLine("{0,-34}{1,13}{2,14}{3,10}{4,16}{5,10}{6,22}",
                title, " Progress", "Current", "Total", "Comparisons", "Swaps", "Elapsed time (ms)");

            for (var i = 0; i < RunCount; i++)
            {
                ComparisonCount = 0;
                SwapCount = 0;

                var sample = new LinkedListRAM(amount, seed);
                var a = new ArrayLongRAM(amount);
                var t = new ArrayLongRAM(amount);
                var count = new ArrayLongRAM(1 << RadixSort.GroupLength);
                var pref = new ArrayLongRAM(1 << RadixSort.GroupLength);

                _stopwatch = Stopwatch.StartNew();
                algorithm(sample, a, t, count, pref);
                _stopwatch.Stop();

                DrawTextProgressBar(amount, amount);
                Console.WriteLine();
                LogResults(title, amount);

                //sample.Print();

                amount *= step;
            }
        }

        public static void TestArrayDisk(int amount, int step, int seed,
            Action<Array, ArrayLong, ArrayLong, ArrayLong, ArrayLong> algorithm)
        {
            // ReSharper disable once PossibleNullReferenceException
            var title = " " + algorithm.Method.DeclaringType.Name + ": Array in disk";

            Console.WriteLine(new string('_', 119));
            Console.WriteLine("{0,-34}{1,13}{2,14}{3,10}{4,16}{5,10}{6,22}",
                title, " Progress", "Current", "Total", "Comparisons", "Swaps", "Elapsed time (ms)");

            for (var i = 0; i < RunCount; i++)
            {
                ComparisonCount = 0;
                SwapCount = 0;

                var sample = new ArrayDisk(Filename, amount, seed);
                var a = new ArrayLongDisk(a_Filename, amount);
                var t = new ArrayLongDisk(t_Filename, amount);
                var count = new ArrayLongDisk(count_Filename, 1 << RadixSort.GroupLength);
                var pref = new ArrayLongDisk(pref_Filename, 1 << RadixSort.GroupLength);

                _stopwatch = Stopwatch.StartNew();
                algorithm(sample, a, t, count, pref);
                _stopwatch.Stop();

                DrawTextProgressBar(amount, amount);
                Console.WriteLine();
                LogResults(title, amount);

                //sample.Print();

                Util.CloseFileStream(sample.FileStream);
                Util.CloseFileStream(a.FileStream);
                Util.CloseFileStream(t.FileStream);
                Util.CloseFileStream(count.FileStream);
                Util.CloseFileStream(pref.FileStream);

                amount *= step;
            }
        }

        public static void TestListDisk(int amount, int step, int seed,
            Action<LinkedList, ArrayLong, ArrayLong, ArrayLong, ArrayLong> algorithm)
        {
            // ReSharper disable once PossibleNullReferenceException
            var title = " " + algorithm.Method.DeclaringType.Name + ": Linked list in disk";

            Console.WriteLine(new string('_', 119));
            Console.WriteLine("{0,-34}{1,13}{2,14}{3,10}{4,16}{5,10}{6,22}",
                title, " Progress", "Current", "Total", "Comparisons", "Swaps", "Elapsed time (ms)");

            for (var i = 0; i < RunCount; i++)
            {
                ComparisonCount = 0;
                SwapCount = 0;

                var sample = new LinkedListDisk(Filename, amount, seed);
                var a = new ArrayLongDisk(a_Filename, amount);
                var t = new ArrayLongDisk(t_Filename, amount);
                var count = new ArrayLongDisk(count_Filename, 1 << RadixSort.GroupLength);
                var pref = new ArrayLongDisk(pref_Filename, 1 << RadixSort.GroupLength);

                using (sample.FileStream = new FileStream(Filename, FileMode.Open, FileAccess.ReadWrite))
                using (a.FileStream = new FileStream(a_Filename, FileMode.Open, FileAccess.ReadWrite))
                using (t.FileStream = new FileStream(t_Filename, FileMode.Open, FileAccess.ReadWrite))
                using (count.FileStream = new FileStream(count_Filename, FileMode.Open, FileAccess.ReadWrite))
                using (pref.FileStream = new FileStream(pref_Filename, FileMode.Open, FileAccess.ReadWrite))
                {
                    _stopwatch = Stopwatch.StartNew();
                    algorithm(sample, a, t, count, pref);
                    _stopwatch.Stop();

                    DrawTextProgressBar(amount, amount);
                    Console.WriteLine();
                    LogResults(title, amount);

                    //sample.Print();
                }

                amount *= step;
            }
        }
    }
}