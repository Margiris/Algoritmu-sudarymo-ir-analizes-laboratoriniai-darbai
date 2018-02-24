using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab1
{
    internal class Sort
    {
        private static Stopwatch _stopwatch;

        protected static long ComparisonCount;
        protected static long SwapCount;
        private const int RunCount = 8;

        private static double[] CreateArray(int count, int seed)
        {
            var array = new double[count];
            var rand = new Random(seed);

            for (var i = 0; i < count; i++)
            {
                array[i] = rand.NextDouble();
            }

            return array;
        }

        private static LinkedList<double> CreateList(int count, int seed)
        {
            var list = new LinkedList<double>();
            var rand = new Random(seed);

            for (var i = 0; i < count; i++)
            {
                list.AddLast(rand.NextDouble());
            }

            return list;
        }

        /// <summary>
        /// Draws progress bar in current console line.
        /// Author - smr5 @ Stack Overflow.
        /// Edited by me to display comparisons & swaps.
        /// </summary>
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

        public static void TestArray(int count, int step, int seed, Action<double[]> algorithm)
        {
            Console.WriteLine(new String('_', 119));
            Console.WriteLine("{0,-34}{1,13}{2,14}{3,10}{4,16}{5,10}{6,22}",
                // ReSharper disable once PossibleNullReferenceException
                " " + algorithm.Method.DeclaringType.Name + ": Array", " Progress",
                "Current", "Total", "Comparisons", "Swaps", "Elapsed time (ms)");

            for (var i = 0; i < RunCount; i++)
            {
                ComparisonCount = 0;
                SwapCount = 0;
                var myArray = CreateArray(count, seed);

                _stopwatch = Stopwatch.StartNew();
                algorithm(myArray);
                _stopwatch.Stop();

                DrawTextProgressBar(count, count);
                Console.WriteLine();

                count *= step;
            }
        }

        public static void TestList(int count, int step, int seed, Action<LinkedList<double>> algorithm)
        {
            Console.WriteLine(new string('_', 119));
            Console.WriteLine("{0,-34}{1,13}{2,14}{3,10}{4,16}{5,10}{6,22}",
                // ReSharper disable once PossibleNullReferenceException
                " " + algorithm.Method.DeclaringType.Name + ": List", " Progress",
                "Current", "Total", "Comparisons", "Swaps", "Elapsed time (ms)");

            for (var i = 0; i < RunCount; i++)
            {
                ComparisonCount = 0;
                SwapCount = 0;
                var myList = CreateList(count, seed);

                _stopwatch = Stopwatch.StartNew();
                algorithm(myList);
                _stopwatch.Stop();

                DrawTextProgressBar(count, count);
                Console.WriteLine();

                count *= step;
            }
        }

        public static void DebugArray(int seed)
        {
            var myDataArray = CreateArray(10, seed);

            Console.WriteLine(myDataArray.ToString());

            RadixSort.Sort(myDataArray);

            Console.WriteLine(myDataArray.ToString());
        }

        public static void DebugList(int seed)
        {
            var myDataList = CreateList(10, seed);

            Console.WriteLine(myDataList.ToString());

            RadixSort.Sort(myDataList);

            Console.WriteLine(myDataList.ToString());
        }
    }
}