using System.Diagnostics.CodeAnalysis;

namespace Lab1
{
    internal class SelectionSort : Sort
    {
        public static void Sort(Array items)
        {
            for (var i = 0; i < items.Length - 1; i++)
            {
                var indexOfMin = i;

                for (var j = i + 1; j < items.Length; j++)
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
                    items.Swap(i, indexOfMin);
                }

                DrawTextProgressBar(i + 2, items.Length);
            }
        }

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        public static void Sort(LinkedListDisk items)
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

                DrawTextProgressBar(i + 2, length);
            }
        }
    }
}