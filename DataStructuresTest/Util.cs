﻿using System;

namespace DataStructuresTest
{
    public static class Util
    {
        public static double[] DoublesArrayWithRandomValues(int length)
        {
            var arr = new double[length];
            var rand = new Random((int) DateTime.Now.Ticks);

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

        public static int ArraysAreEqual(Lab1.Array array1, Lab1.Array array2)
        {
            for (var i = 0; i < array1.Length; i++)
                if (Math.Abs(array1[i] - array2[i]) > 0.00001)
                    return i;
            return -1;
        }

        public static int ArraysAreEqual(double[] array1, Lab1.Array array2)
        {
            for (var i = 0; i < array1.Length; i++)
                if (Math.Abs(array1[i] - array2[i]) > 0.00001)
                    return i;
            return -1;
        }

        public static int ArraysAreEqual(Lab1.ArrayLong array1, Lab1.ArrayLong array2)
        {
            for (var i = 0; i < array1.Length; i++)
                if (array1[i] != array2[i])
                    return i;
            return -1;
        }

        public static int ArraysAreEqual(long[] array1, Lab1.ArrayLong array2)
        {
            for (var i = 0; i < array1.Length; i++)
                if (array1[i] != array2[i])
                    return i;
            return -1;
        }
    }
}