using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable InconsistentNaming

namespace Lab1
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class SelectionSort : Sort
    {
        public static void SortRAM(Array items)
        {
            var length = items.Length;

            for (var i = 0; i < length - 1; i++)
            {
                var indexOfMin = i;

                for (var j = i + 1; j < length; j++)
                {
                    ComparisonCount++;

                    if (items[j] < items[indexOfMin])
                    {
                        indexOfMin = j;
                    }
                }

                if (indexOfMin != i)
                {
                    SwapCount++;
                    var temp = items[i];
                    items[i] = items[indexOfMin];
                    items[indexOfMin] = temp;
                }

//                DrawTextProgressBar(i + 2, length);
            }
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static void SortRAM(LinkedList<double> items)
        {
            var length = items.Count;
            var currentOuter = items.First;

            for (var i = 0; i < length - 1; i++)
            {
                var minimum = currentOuter;

                for (var current = currentOuter.Next; current != null; current = current.Next)
                {
                    ComparisonCount++;

                    if (current.Value < minimum.Value)
                    {
                        minimum = current;
                    }
                }

                if (!ReferenceEquals(minimum, currentOuter))
                {
                    SwapCount++;
                    var temp = currentOuter.Value;
                    currentOuter.Value = minimum.Value;
                    minimum.Value = temp;
                }

                currentOuter = currentOuter.Next;

//                DrawTextProgressBar(i + 2, length);
            }
        }
    }
}
