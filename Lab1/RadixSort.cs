using System;
using System.Collections.Generic;
// ReSharper disable LoopVariableIsNeverChangedInsideLoop

namespace Lab1
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class RadixSort : Sort
    {
        public static void Sort(double[] items)
        {
            var length = items.Length;
            // temporary items and the items of converted doubles to longs
            var t = new long[length];
            var a = new long[length];
            
            for (var i = 0; i < length; i++)
            {
                a[i] = BitConverter.ToInt64(BitConverter.GetBytes(items[i]), 0);
            }

            const int groupLength = 4;
            const int bitLength = 64;

            // counting and prefix items
            // (dimension is 2^r, the number of possible values of a r-bit number) 
            var count = new long[1 << groupLength];
            var pref = new long[1 << groupLength];
            const long groups = bitLength / groupLength;
            const long mask = (1 << groupLength) - 1;
            long negatives = 0, positives = 0;

            for (int c = 0, shift = 0; c < groups; c++, shift += groupLength)
            {
                // reset count items 
                for (var j = 0; j < count.Length; j++)
                    count[j] = 0;

                // counting elements of the c-th group 
                foreach (var i in a)
                {
                    count[(i >> shift) & mask]++;

                    // additionally count all negative 
                    // values in first round
                    if (c == 0 && i < 0)
                        negatives++;
                }

                if (c == 0) positives = length - negatives;

                // calculating prefixes
                pref[0] = 0;
                for (var i = 1; i < count.Length; i++)
                    pref[i] = pref[i - 1] + count[i - 1];

                // from a[] to t[] elements ordered by c-th group 
                foreach (var i in a)
                {
                    // Get the right index to sort the number in
                    var index = pref[(i >> shift) & mask]++;

                    if (c == groups - 1)
                    {
                        // We're in the last (most significant) group, if the
                        // number is negative, order them inversely in front
                        // of the items, pushing positive ones back.
                        if (i < 0)
                            index = positives - (index - negatives) - 1;
                        else
                            index += negatives;
                    }

                    t[index] = i;
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
            
            DrawTextProgressBar(length, length);
        }

        public static void Sort(LinkedList<double> items)
        {
            var length = items.Count;

            // temporary items and the items of converted doubles to longs
            var t = new long[length];
            var a = new long[length];

            var o = 0;
            
            foreach (var item in items)
            {
                a[o++] = BitConverter.ToInt64(BitConverter.GetBytes(item), 0);   
            }

            const int groupLength = 4;
            const int bitLength = 64;

            // counting and prefix items
            // (dimension is 2^r, the number of possible values of a r-bit number) 
            var count = new long[1 << groupLength];
            var pref = new long[1 << groupLength];
            const long groups = bitLength / groupLength;
            const long mask = (1 << groupLength) - 1;
            long negatives = 0, positives = 0;

            for (int c = 0, shift = 0; c < groups; c++, shift += groupLength)
            {
                // reset count items 
                for (var j = 0; j < count.Length; j++)
                    count[j] = 0;

                // counting elements of the c-th group 
                foreach (var i in a)
                {
                    count[(i >> shift) & mask]++;

                    // additionally count all negative 
                    // values in first round
                    if (c == 0 && i < 0)
                        negatives++;
                }

                if (c == 0) positives = length - negatives;

                // calculating prefixes
                pref[0] = 0;
                for (var i = 1; i < count.Length; i++)
                    pref[i] = pref[i - 1] + count[i - 1];

                // from a[] to t[] elements ordered by c-th group 
                foreach (var i in a)
                {
                    // Get the right index to sort the number in
                    var index = pref[(i >> shift) & mask]++;

                    if (c == groups - 1)
                    {
                        // We're in the last (most significant) group, if the
                        // number is negative, order them inversely in front
                        // of the items, pushing positive ones back.
                        if (i < 0)
                            index = positives - (index - negatives) - 1;
                        else
                            index += negatives;
                    }

                    t[index] = i;
                }

                // a[]=t[] and start again until the last group 
                t.CopyTo(a, 0);
            }

            o = 0;
            // Convert back the longs to the double items
            for (var current = items.First; current != null; current = current.Next)
            {
//                DrawTextProgressBar(i + 1, length);4
                current.Value = BitConverter.ToDouble(BitConverter.GetBytes(a[o++]), 0);
            }
            
            DrawTextProgressBar(length, length);
        }
        
//        public static void Sort(DataList items)
//        {
//            var isFinished = false;
//            var digitPosition = 0;
//
//            var buckets = new List<Queue<double>>();
//            InitializeBuckets(buckets);
//
//            while (!isFinished)
//            {
//                isFinished = true;
//
//                for (var current = items.First; current != null; current = current.NextNode)
//                {
//                    var bucketNumber = GetBucketNumber(current.Data, digitPosition);
//                    
//                    if (bucketNumber > 0)
//                    {
//                        isFinished = false;
//                    }
//
//                    buckets[bucketNumber].Enqueue(current.Data);
//                }
//
//                var walk = items.First;
//                foreach (var bucket in buckets)
//                {
//                    while (bucket.Count > 0 && walk != null)
//                    {
//                        walk.Data = bucket.Dequeue();
//                        walk = walk.Next();
//                    }
//                }
//
//                digitPosition++;
//            }
//        }
//
//        private static int GetBucketNumber(double value, int digitPosition)
//        {
//            var bucketNumber = (value / (int)Math.Pow(10, digitPosition)) % 10;
//            return bucketNumber;
//        }
//
//        private static void InitializeBuckets(ICollection<Queue<double>> buckets)
//        {
//            for (var i = 0; i < 10; i++)
//            {
//                var q = new Queue<double>();
//                buckets.Add(q);
//            }
//        }
    }
}