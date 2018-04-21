﻿using System;
using System.Collections.Generic;
using System.Threading;

// ReSharper disable TailRecursiveCall

namespace Lab2
{
    internal static class Program
    {
        public static void Main()
        {
            var t = new Thread(Task1, 500000000);
            t.Start();
        }

        private static void Task1()
        {
            var number = GetInteger();
            var intermediateResults = LongArrayWithSingleValue(-1, number);

            Console.WriteLine("Recursively:");

            var result = Fr(number);
            var actions = CalculateActionsRecursively(number, new LinkedList<int>());
            PrintResults(actions, result);

            actions.Clear();

            Console.WriteLine("Dynamically:");

            result = Fd(number, intermediateResults);
            actions = CalculateActions(number);
            PrintResults(actions, result);
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
                   6 * Fr(n / 5) * Fr(n / 5) +
                   3 * Fr(n / 6) * Fr(n / 6) +
                   n * n / 5;
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

            results[n - 2] = (results[n - 2] > -1) ? results[n - 2] : Fd(n - 2, results);
            results[n / 5] = (results[n / 5] > -1) ? results[n / 5] : Fd(n / 5, results);
            results[n / 6] = (results[n / 6] > -1) ? results[n / 6] : Fd(n / 6, results);

            return results[n - 2] +
                   6 * results[n / 5] * results[n / 5] +
                   3 * results[n / 6] * results[n / 6] +
                   n * n / 5;
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
        /// Time complexity:
        ///     best - log3(n);
        ///     worst - n - 1
        /// for all natural numbers n > 1.
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
        /// Same as CalculateActions but instead of while loop it uses recursion.
        /// </summary>
        /// <param name="number">The number to work with</param>
        /// <param name="actions">List of actions performed</param>
        /// <returns>List of actions performed</returns>
        private static LinkedList<int> CalculateActionsRecursively(int number, LinkedList<int> actions)
        {
            if (number <= 1)
            {
                return actions;
            }

            if (number % 3 == 0)
            {
                actions.AddLast(3);
                return CalculateActionsRecursively(number / 3, actions);
            }

            if (number % 2 == 0)
            {
                actions.AddLast(2);
                return CalculateActionsRecursively(number / 2, actions);
            }

            actions.AddLast(1);
            return CalculateActionsRecursively(number - 1, actions);
        }

        /// <summary>
        /// Prints the length of the given list of numbers in one line and all the numbers in the other one.
        /// </summary>
        /// <param name="actions">List of numbers to print</param>
        /// <param name="result">Result of the first task</param>
        /// <exception cref="ArgumentNullException">Throws if the given list is null</exception>
        private static void PrintResults(LinkedList<int> actions, long result)
        {
            if (actions == null) throw new ArgumentNullException(nameof(actions));

            Console.WriteLine("Result of the first task - " + result);
            Console.WriteLine("Number of actions required - " + actions.Count);
            Console.Write("Actions: ");

            foreach (var action in actions)
                Console.Write(action);

            Console.WriteLine();
        }
    }
}