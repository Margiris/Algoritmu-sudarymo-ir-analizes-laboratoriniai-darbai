using System;

namespace Lab1
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class RadixSort : Sort
    {
        public const int GroupLength = 16;
        private const int BitLength = 64;

        private const long Groups = BitLength / GroupLength;
        private const long Mask = (1 << GroupLength) - 1;

        private static long _negatives, _positives;

        public static void Sort(Array items, ArrayLong a, ArrayLong t, ArrayLong count, ArrayLong pref)
        {
            var length = items.Length;

            for (var i = 0; i < length; i++)
            {
                a[i] = BitConverter.ToInt64(BitConverter.GetBytes(items[i]), 0);
            }

            for (int c = 0, shift = 0; c < Groups; c++, shift += GroupLength)
            {
                // reset count items 
                for (var j = 0; j < count.Length; j++)
                    count[j] = 0;

                // counting elements of the c-th group 
                for (var index = 0; index < a.Length; index++)
                {
                    var i = a[index];
                    count[(int) ((i >> shift) & Mask)]++;

                    // additionally count all negative 
                    // values in first round
                    if (c == 0 && i < 0)
                        _negatives++;
                }

                if (c == 0) _positives = length - _negatives;

                // calculating prefixes
                pref[0] = 0;
                for (var i = 1; i < count.Length; i++)
                    pref[i] = pref[i - 1] + count[i - 1];

                // from a[] to t[] elements ordered by c-th group 
                for (var index1 = 0; index1 < a.Length; index1++)
                {
                    var i = a[index1];
                    // Get the right index to sort the number in
                    var index = pref[(int) ((i >> shift) & Mask)]++;

                    if (c == Groups - 1)
                    {
                        // We're in the last (most significant) group, if the
                        // number is negative, order them inversely in front
                        // of the items, pushing positive ones back.
                        if (i < 0)
                            index = _positives - (index - _negatives) - 1;
                        else
                            index += _negatives;
                    }

                    t[(int) index] = i;
                }

                // a[]=t[] and start again until the last group 
                t.CopyTo(a, 0);
            }

            // Convert back the longs to the double items
            for (var i = 0; i < length; i++)
            {
//                DrawTextProgressBar(i + 1, length);
                items[i] = BitConverter.ToDouble(BitConverter.GetBytes(a[i]), 0);
            }
        }

        public static void Sort(LinkedList items, ArrayLong a, ArrayLong t, ArrayLong count, ArrayLong pref)
        {
            var length = items.Count;
            var current = items.GetFirstNode();

            // temporary items and the items of converted doubles to longs
            //var t = new long[length];
            //var a = new long[length];

            for (var i = 0; i < items.Count; i++)
            {
                a[i] = BitConverter.ToInt64(BitConverter.GetBytes(current.Value), 0);
                current = items.NextOf(current);
            }

            for (int c = 0, shift = 0; c < Groups; c++, shift += GroupLength)
            {
                // reset count items 
                for (var j = 0; j < count.Length; j++)
                    count[j] = 0;

                // counting elements of the c-th group 
                for (var index = 0; index < a.Length; index++)
                {
                    var i = a[index];
                    count[(int) ((i >> shift) & Mask)]++;

                    // additionally count all negative 
                    // values in first round
                    if (c == 0 && i < 0)
                        _negatives++;
                }

                if (c == 0) _positives = length - _negatives;

                // calculating prefixes
                pref[0] = 0;
                for (var i = 1; i < count.Length; i++)
                    pref[i] = pref[i - 1] + count[i - 1];

                // from a[] to t[] elements ordered by c-th group 
                for (var index1 = 0; index1 < a.Length; index1++)
                {
                    var i = a[index1];
                    // Get the right index to sort the number in
                    var index = pref[(int) ((i >> shift) & Mask)]++;

                    if (c == Groups - 1)
                    {
                        // We're in the last (most significant) group, if the
                        // number is negative, order them inversely in front
                        // of the items, pushing positive ones back.
                        if (i < 0)
                            index = _positives - (index - _negatives) - 1;
                        else
                            index += _negatives;
                    }

                    t[(int) index] = i;
                }

                // a[]=t[] and start again until the last group 
                t.CopyTo(a, 0);
            }

            current = items.GetFirstNode();
            // Convert back the longs to the double items
            for (var i = 0; i < length; i++)
            {
//                DrawTextProgressBar(o + 1, length);
                items.SetValue(current, BitConverter.ToDouble(BitConverter.GetBytes(a[i]), 0));
                current = items.NextOf(current);
            }
        }
    }
}