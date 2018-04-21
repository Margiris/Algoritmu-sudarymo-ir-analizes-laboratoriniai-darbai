﻿using System;
using System.Collections.Generic;

namespace Lab2
{
    internal static class Program
    {
        public static void Main()
        {
            var number = GetInteger();

            var intermediateResults = LongArrayWithSingleValue(-1, number);

            Console.WriteLine("Recursively - " + Fr(number));
            Console.WriteLine("Dinamicaly - " + Fd(number, intermediateResults));

            var actions = CalculateActions(number);
            PrintResults(actions);
        }

        /// <summary>
        /// Creates and returns an array of type long filled with spedified value.
        /// </summary>
        /// <param name="value">The value to write to the array</param>
        /// <param name="length">The length of the array</param>
        /// <returns>Newly created array filled with values</returns>
        private static long[] LongArrayWithSingleValue(long value, int length)
        {
            var array = new long[length];

            for (var i = 0; i < length; ++i)
                array[i] = value;

            return array;
        }

        /// <summary>
        /// Calculates F(n)    = F(n - 2) + 6 * F(n / 5) ^ 2 + 3 * F(n / 6) ^ 2 + n ^ 2 / 5; if n > 1
        ///                    = 2; otherwise
        /// recursively.
        /// </summary>
        /// <param name="n">Argument of the function</param>
        /// <returns>Result of the function</returns>
        private static long Fr(long n)
        {
            if (n <= 1)
                return 2;

            return Fr(n - 2) +
                   6 * Convert.ToInt64(Math.Pow(Fr(n / 5), 2)) +
                   3 * Convert.ToInt64(Math.Pow(Fr(n / 6), 2)) +
                   Convert.ToInt64(Math.Pow(n, 2) / 5);
        }

        /// <summary>
        /// Calculates the same function as Fr(long n), but uses an array to store previously calculated results
        /// in order to save time if the same result is needed.
        /// </summary>
        /// <param name="n">Argument of the function</param>
        /// <param name="results">Previously calculated values of the function</param>
        /// <returns>Result of the function</returns>
        // ReSharper disable once SuggestBaseTypeForParameter
        private static long Fd(long n, long[] results)
        {
            if (n <= 1)
                return 2;

            if (n >= 2)
                results[n - 2] = (results[n - 2] > -1) ? results[n - 2] : Fd(n - 2, results);

            results[n / 5] = (results[n / 5] > -1) ? results[n / 5] : Fd(n / 5, results);
            results[n / 6] = (results[n / 6] > -1) ? results[n / 6] : Fd(n / 6, results);

            return results[n - 2] +
                   6 * Convert.ToInt64(Math.Pow(results[n / 5], 2)) +
                   3 * Convert.ToInt64(Math.Pow(Fd(n / 6, results), 2)) +
                   Convert.ToInt64(Math.Pow(n, 2) / 5);
        }

        /// <summary>
        /// Asks for input,
        /// catches invalid characters and displays a message,
        /// returns input if greater than 1
        /// or asks again otherwise.
        /// </summary>
        /// <returns>An integer greater than 1</returns>
        private static int GetInteger()
        {
            var a = 0;

            while (a <= 1)
            {
                Console.Write("Enter an integer greater than 1: ");

                try
                {
                    a = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            }

            return a;
        }

        /// <summary>
        /// Calculates the smallest amount of actions required to get the given number to 1.
        /// Allowed actions are:
        ///     division by 3;
        ///     division by 2;
        ///     subtraction of 1.
        /// </summary>
        /// <param name="number">The number to work with</param>
        /// <returns>List of actions performed</returns>
        private static LinkedList<int> CalculateActions(int number)
        {
            var actions = new LinkedList<int>();

            while (number > 1)
            {
                if (number % 3 == 0)
                {
                    number /= 3;
                    actions.AddLast(3);
                }
                else if (number % 2 == 0)
                {
                    number /= 2;
                    actions.AddLast(2);
                }
                else
                {
                    number -= 1;
                    actions.AddLast(1);
                }
            }

            return actions;
        }

        /// <summary>
        /// Prints the length of the given list of numbers in one line and all the numbers in the other one.
        /// </summary>
        /// <param name="actions">List of numbers to print</param>
        /// <exception cref="ArgumentNullException">Throws if the given list is null</exception>
        private static void PrintResults(LinkedList<int> actions)
        {
            if (actions == null) throw new ArgumentNullException(nameof(actions));

            Console.WriteLine("Number of actions required - " + actions.Count);

            Console.Write("Actions: ");
            foreach (var action in actions)
            {
                Console.Write(action);
            }

            Console.WriteLine();
        }
    }
}