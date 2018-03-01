using System;
using System.Diagnostics;
using System.IO;

namespace Lab1
{
    internal class Sort
    {
        private static Stopwatch _stopwatch;

        protected static long ComparisonCount;
        protected static long SwapCount;
        private const int RunCount = 6;

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

        public static void TestArrayRAM(int count, int step, int seed, Action<Array> algorithm)
        {
            Console.WriteLine(new string('_', 119));
            Console.WriteLine("{0,-34}{1,13}{2,14}{3,10}{4,16}{5,10}{6,22}",
                // ReSharper disable once PossibleNullReferenceException
                " " + algorithm.Method.DeclaringType.Name + ": Array in RAM", " Progress",
                "Current", "Total", "Comparisons", "Swaps", "Elapsed time (ms)");

            for (var i = 0; i < RunCount; i++)
            {
                ComparisonCount = 0;
                SwapCount = 0;
                var sample = new ArrayRAM(count, seed);

                _stopwatch = Stopwatch.StartNew();
                algorithm(sample);
                _stopwatch.Stop();

                DrawTextProgressBar(count, count);
                Console.WriteLine();

//                sample.Print();


                count *= step;
            }
        }

        public static void TestListRAM(int count, int step, int seed, Action<LinkedList> algorithm)
        {
            Console.WriteLine(new string('_', 119));
            Console.WriteLine("{0,-34}{1,13}{2,14}{3,10}{4,16}{5,10}{6,22}",
                // ReSharper disable once PossibleNullReferenceException
                " " + algorithm.Method.DeclaringType.Name + ": List in RAM", " Progress",
                "Current", "Total", "Comparisons", "Swaps", "Elapsed time (ms)");

            for (var i = 0; i < RunCount; i++)
            {
                ComparisonCount = 0;
                SwapCount = 0;
                var sample = new LinkedListRAM(count, seed);

                _stopwatch = Stopwatch.StartNew();
                algorithm(sample);
                _stopwatch.Stop();

                DrawTextProgressBar(count, count);
                Console.WriteLine();

//                sample.Print();

                count *= step;
            }
        }

        public static void TestArrayDisk(int count, int step, int seed, Action<Array> algorithm)
        {
            Console.WriteLine(new string('_', 119));
            Console.WriteLine("{0,-34}{1,13}{2,14}{3,10}{4,16}{5,10}{6,22}",
                // ReSharper disable once PossibleNullReferenceException
                " " + algorithm.Method.DeclaringType.Name + ": Array in disk", " Progress",
                "Current", "Total", "Comparisons", "Swaps", "Elapsed time (ms)");

            for (var i = 0; i < RunCount; i++)
            {
                const string filename = @"file.dat";
                ComparisonCount = 0;
                SwapCount = 0;
                var sample = new ArrayDisk(filename, count, seed);

                using (sample.FileStream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
                {
                    _stopwatch = Stopwatch.StartNew();
                    algorithm(sample);
                    _stopwatch.Stop();

                    DrawTextProgressBar(count, count);
                    Console.WriteLine();

//                    sample.Print();
                }

                count *= step;
            }
        }

        public static void TestListDisk(int count, int step, int seed, Action<LinkedListDisk> algorithm)
        {
            Console.WriteLine(new string('_', 119));
            Console.WriteLine("{0,-34}{1,13}{2,14}{3,10}{4,16}{5,10}{6,22}",
                // ReSharper disable once PossibleNullReferenceException
                " " + algorithm.Method.DeclaringType.Name + ": Linked list in disk", " Progress",
                "Current", "Total", "Comparisons", "Swaps", "Elapsed time (ms)");

            for (var i = 0; i < RunCount; i++)
            {
                const string filename = @"file.dat";
                ComparisonCount = 0;
                SwapCount = 0;
                var sample = new LinkedListDisk(filename, count, seed);

                using (sample.FileStream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
                {
                    _stopwatch = Stopwatch.StartNew();
                    algorithm(sample);
                    _stopwatch.Stop();

                    DrawTextProgressBar(count, count);
                    Console.WriteLine();

//                    sample.Print();
                }

                count *= step;
            }
        }
    }
}