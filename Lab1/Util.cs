using System;
using System.IO;

namespace Lab1
{
    public static class Util
    {
        public static double[] DoublesArrayWithRandomValues(int length)
        {
            return DoublesArrayWithRandomValues(length, (int) DateTime.Now.Ticks);
        }

        public static double[] DoublesArrayWithRandomValues(int length, int seed)
        {
            var arr = new double[length];
            var rand = new Random(seed);

            for (var i = 0; i < length; i++)
            {
                arr[i] = rand.NextDouble() <= 0.5 ? rand.NextDouble() : 0 - rand.NextDouble();
            }

            return arr;
        }

        public static long[] LongsArrayWithRandomValues(int length)
        {
            var temp = DoublesArrayWithRandomValues(length);
            var arr = new long[length];

            for (var i = 0; i < length; i++)
                arr[i] = (long) temp[i] * 10000;

            return arr;
        }

        public static int FindArraysDifferenceIndex(Array array1, Array array2)
        {
            for (var i = 0; i < array1.Length; i++)
                if (Math.Abs(array1[i] - array2[i]) > 0.00001)
                    return i;

            return -1;
        }

        public static int FindArraysDifferenceIndex(double[] array1, Array array2)
        {
            for (var i = 0; i < array1.Length; i++)
                if (Math.Abs(array1[i] - array2[i]) > 0.00001)
                    return i;

            return -1;
        }

        public static int FindArraysDifferenceIndex(ArrayLong array1, ArrayLong array2)
        {
            for (var i = 0; i < array1.Length; i++)
                if (array1[i] != array2[i])
                    return i;

            return -1;
        }

        public static int FindArraysDifferenceIndex(long[] array1, ArrayLong array2)
        {
            for (var i = 0; i < array1.Length; i++)
                if (array1[i] != array2[i])
                    return i;

            return -1;
        }

        public static int FindListsDifferenceIndex(LinkedList list1, LinkedList list2)
        {
            var current1 = list1.GetFirstNode();
            var current2 = list2.GetFirstNode();

            for (var i = 0; i < list1.Count; i++)
            {
                if (Math.Abs(current1.Value - current2.Value) > 0.00001)
                    return i;
                current1 = list1.NextOf(current1);
                current2 = list2.NextOf(current2);
            }

            return -1;
        }

        public static bool CloseFileStream(FileStream stream)
        {
            if (stream == null) return false;

            if (stream.CanWrite)
                stream.Flush();

            try
            {
                stream.Close();
                return true;
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}