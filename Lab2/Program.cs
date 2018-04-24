using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

// ReSharper disable RedundantAssignment
// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable InconsistentNaming

// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable TailRecursiveCall

namespace Lab2
{
    internal static class Program
    {
        private const string ResultsLogFilename = @"results.log";
        private static Stopwatch _stopwatch;
        private static int actionCount;

        public static void Main()
        {
            _stopwatch = new Stopwatch();

            Console.WriteLine("Task 1");
            var task1 = new Thread(Task1, 1000000000);
            task1.Start();

            while (task1.IsAlive)
            {
                Thread.Sleep(500);
                if (!_stopwatch.IsRunning) continue;

                Console.Write("Actions performed - " + actionCount);
                Console.CursorLeft = 0;
            }

            Console.WriteLine("\nTask 2");
            var task2 = new Thread(Task2, 1000000000);
            task2.Start();

            while (task2.IsAlive)
            {
                Thread.Sleep(500);
                if (!_stopwatch.IsRunning) continue;

                Console.Write("Actions performed - " + actionCount);
                Console.CursorLeft = 0;
            }

            Console.WriteLine("\nTask 2 from 2 to the input number increasing by * 5");
            var task2Automatic = new Thread(Task2Automatic, 1000000000);
            task2Automatic.Start();

            while (task2Automatic.IsAlive)
            {
                Thread.Sleep(500);
                if (!_stopwatch.IsRunning) continue;

                Console.Write("Actions performed - " + actionCount);
                Console.CursorLeft = 0;
            }
        }

        /// <summary>
        /// Method for the first task.
        /// </summary>
        private static void Task1()
        {
            int number;

            while ((number = GetInteger()) > 0)
            {
                var intermediateResults = LongArrayWithSingleValue(-1, number);

                actionCount = 0;
                _stopwatch = Stopwatch.StartNew();
                var result = Fr(number);
                _stopwatch.Stop();
                var actionsPerformed1 = actionCount;

                actionCount = 0;
                _stopwatch = Stopwatch.StartNew();
                result = Fd(number, intermediateResults);
                _stopwatch.Stop();
                var actionsPerformed2 = actionCount;

                LogResults(number, actionsPerformed1, actionsPerformed2, result, null);
            }
        }

        /// <summary>
        /// Method for the second task.
        /// </summary>
        private static void Task2()
        {
            double number;

            while ((number = GetNumber()) > 0)
            {
                _stopwatch = Stopwatch.StartNew();
                var actions = CalculateActionsRecursively(number, new List<int>());
                _stopwatch.Stop();

                LogResults(number, 0, 0, actions.Count, actions);
            }
        }

        /// <summary>
        /// Method for the second task that takes a number from input and calculates the result
        /// for all numbers from 2 to it increasing by * 2.
        /// </summary>
        private static void Task2Automatic()
        {
            double initialNumber;

            while ((initialNumber = GetNumber()) > 0)
            {
                for (double number = 2; number < initialNumber; number *= 5)
                {
                    _stopwatch = Stopwatch.StartNew();
                    var actions = CalculateActionsRecursively(number, new List<int>());
                    _stopwatch.Stop();
                    var elapsedTime1 = _stopwatch.ElapsedMilliseconds;

                    actions.Clear();

                    _stopwatch = Stopwatch.StartNew();
                    actions = CalculateActions(number);
                    _stopwatch.Stop();
                    var elapsedTime2 = _stopwatch.ElapsedMilliseconds;

                    LogResults(number, elapsedTime1, elapsedTime2, actions.Count, actions);
                }
            }
        }

        /// <summary>
        /// Prints the length of the given list of numbers in one line and all the numbers in the other one to the console.
        /// Logs the required amount of actions of each method to file.
        /// </summary>
        /// <param name="number">The given number</param>
        /// <param name="result">The result calculated by the algorithm</param>
        /// <param name="actions">Actions performed (if any) in the second task</param>
        /// <param name="actionsPerformed1">Amount of actions performed to finish the recursive algorithm</param>
        /// <param name="actionsPerformed2">Amount of actions performed to finish the dynamic algorithm</param>
        private static void LogResults(double number, long actionsPerformed1, long actionsPerformed2, long result,
            List<int> actions)
        {
            using (var resultStreamWriter = new StreamWriter(ResultsLogFilename, true))
            {
                if (actions == null)
                {
                    Console.WriteLine("Total actions performed recursively - {0}, dynamically - {1}",
                        actionsPerformed1, actionsPerformed2);

                    resultStreamWriter.WriteLine("{0,7}{1,15}{2,15}{3,30}",
                        number, actionsPerformed1, actionsPerformed2, result);
                }
                else
                {
                    var actionsString = "";

                    foreach (var action in actions)
                        actionsString += action;

                    Console.WriteLine("Number of actions required - " + actions.Count);
                    Console.WriteLine("Actions: " + actionsString);

                    resultStreamWriter.WriteLine("{0,30}{1,4}  {2}",
                        number, result, actionsString);
                }
            }

            Console.WriteLine();
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
        /// Checks if input number is bigger than maximum int value:
        ///     if it is - prints a warning and asks got input again;
        ///     if it's not - returns input converted to int.
        /// </summary>
        /// <returns>Input number converted to int</returns>
        private static int GetInteger()
        {
            var number = GetNumber();

            while (number > int.MaxValue)
            {
                Console.WriteLine("This number is too big for the current method, please try again.");
                number = GetNumber();
            }

            return Convert.ToInt32(number);
        }

        /// <summary>
        /// Asks for input,
        /// catches invalid characters and displays a message,
        /// returns input if greater than 1
        /// or asks again otherwise.
        /// </summary>
        /// <returns>A number greater than 1</returns>
        private static double GetNumber()
        {
            double a = 0;

            while (a <= 1)
            {
                Console.Write("Provide an integer greater than 1 or type x to exit: ");

                var line = Console.ReadLine();

                if (line == "x")
                {
                    return 0;
                }

                try
                {
                    a = Convert.ToDouble(line);
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            }

            return a;
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
            actionCount++;

            if (n <= 1)
                return 2;

            var Fr1 = Fr(n - 2);
            var Fr2 = Fr(n / 5);
            var Fr3 = Fr(n / 6);

            return Fr1 + 6 * Fr2 * Fr2 + 3 * Fr3 * Fr3 + n * n / 5;
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
            actionCount++;

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
        private static List<int> CalculateActions(double number)
        {
            var actions = new List<int>();

            while (number > 1)
            {
                if (number % 3 == 0)
                {
                    number /= 3;
                    actions.Add(3);
                }
                else if (number % 2 == 0)
                {
                    number /= 2;
                    actions.Add(2);
                }
                else
                {
                    number -= 1;
                    actions.Add(1);
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
        private static List<int> CalculateActionsRecursively(double number, List<int> actions)
        {
            if (number <= 1)
            {
                return actions;
            }

            if (IsDividableBy(3, number))
            {
                actions.Add(3);
                return CalculateActionsRecursively(number / 3, actions);
            }

            if (IsDividableBy(2, number))
            {
                actions.Add(2);
                return CalculateActionsRecursively(number / 2, actions);
            }

            actions.Add(1);
            return CalculateActionsRecursively(number - 1, actions);
        }

        private static bool IsDividableBy(int divider, double number)
        {
            if (divider == 1 || divider == number)
            {
                return true;
            }
            
            var digits = doubleToIntArray(number);

            switch (divider)
            {
                case 2:
                    return digits[digits.Length - 1] % 2 == 0;
                case 3:
                    return digits.Sum() % 3 == 0;
                default:
                    throw new NotImplementedException();
            }
        }

        private static int[] doubleToIntArray(double number)
        {
            var array = new LinkedList<int>();

            while (number >= 1)
            {
                array.AddFirst((int) (number % 10));
                number /= 10;
            }

            return array.ToArray();
        }
    }
}