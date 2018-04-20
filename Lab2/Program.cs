using System;
using System.Collections.Generic;

namespace Lab2
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var number = GetInteger();
            var actions = new LinkedList<int>();
            
            Console.WriteLine("Result 2 (long) - " + F(number));

            Calculate(number, actions);

            PrintResults(actions);
        }
        
        /// <summary>
        /// Calculates F(n)    = F(n - 2) + 6 * F(n / 5) ^ 2 + 3 * F(n / 6) ^ 2 + n ^ 2 / 5; if n > 1
        ///                    = 2; otherwise
        /// recursively.
        /// </summary>
        /// <param name="n">Argument of the function</param>
        /// <returns>Result of the function</returns>
        private static long F(long n)
        {
            if (n <= 1)
            {
                return 2;
            }

            return F(n - 2) +
                   6 * Convert.ToInt64(Math.Pow(F(n / 5), 2)) +
                   3 * Convert.ToInt64(Math.Pow(F(n / 6), 2)) +
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
        /// <param name="actions">List of actions performed</param>
        private static void Calculate(int number, LinkedList<int> actions)
        {
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