using System;
using System.Linq;
using System.Threading;
// ReSharper disable ParameterTypeCanBeEnumerable.Local
// ReSharper disable SuggestBaseTypeForParameter

namespace Lab3
{

    public static class FirstTask
    {
        private const int MaxValue = 20;
        private const int ArrayCount = 20;
        private const int ItemsCount = 10;

        private static readonly string[] Results = new string[ArrayCount];
        
        public static void Run()
        {
            for (var i = 0; i < ArrayCount; i++)
            {
                var i1 = i;
                var t = new Thread(() => Sort(i1));
                t.Start();
            }

            Print(Results);
        }

        private static void Sort(int indexInResults)
        {
            var seed = (int) DateTime.Now.Ticks + indexInResults * indexInResults;

            var items = NewRandomArray(ItemsCount, seed);

            Results[indexInResults] = ToLine(items);

            CountingSort(items);

            Results[indexInResults] += " sorted: " + ToLine(items);
        }

        private static void CountingSort(int[] items)
        {
            var count = new int[items.Max() + 1];

            for (var i = 0; i < items.Length; i++)
            {
                count[items[i]]++;
            }

            var a = 0;

            for (var i = 0; i < count.Length; i++)
            {
                for (var j = 0; j < count[i]; j++)
                {
                    items[a++] = i;
                }
            }
        }

        private static string ToLine(int[] items)
        {
            return items.Aggregate("", (current, item) => current + $"{item,3} ");
        }

        private static int[] NewRandomArray(int count, int seed)
        {
            var randomArray = new int[count];
            var rand = new Random(seed);

            for (var i = 0; i < count; i++)
            {
                randomArray[i] = rand.Next(MaxValue);
            }

            return randomArray;
        }

        private static void Print(string[] results)
        {
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }
        }
    }
}