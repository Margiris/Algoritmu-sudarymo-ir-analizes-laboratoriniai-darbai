namespace Lab1
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class SelectionSort : Sort
    {
        public static void Sort(Array items, Array a, Array t)
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

//                DrawTextProgressBar(i + 2, items.Length);
            }
        }

        public static void Sort(LinkedList items, Array a, Array t)
        {
            var length = items.Count;
            var currentOuter = items.GetFirstNode();

            for (var i = 0; i < length - 1; i++)
            {
                var minimum = currentOuter;
                var currentInner = items.NextOf(currentOuter);

                for (var j = i + 1; j < length; j++)
                {
                    ComparisonCount++;

                    if (currentInner.Value < minimum.Value)
                    {
                        minimum = currentInner;
                    }

                    currentInner = items.NextOf(currentInner);
                }

                if (!ReferenceEquals(minimum, currentOuter))
                {
                    SwapCount++;
                    items.Swap(minimum, currentOuter);
                }

                currentOuter = items.NextOf(currentOuter);

//                DrawTextProgressBar(i + 2, length);
            }
        }
    }
}