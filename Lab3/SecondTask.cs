using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ConvertToConstant.Local
// ReSharper disable InconsistentNaming

namespace Lab3
{
    public static class SecondTask
    {
        private const string ResultsLogFilename = @"results.log";
        private static Stopwatch _stopwatch;

        /// <summary>
        /// Main thread for the task.
        /// </summary>
        public static void Run()
        {
            int number;

            while ((number = GetInteger()) > 0)
            {
                _stopwatch = Stopwatch.StartNew();
                var result1 = Fr(number);
                _stopwatch.Stop();
                var actionsPerformed1 = _stopwatch.ElapsedMilliseconds;

                _stopwatch = Stopwatch.StartNew();
                var result2 = Ft(number);
                _stopwatch.Stop();
                var actionsPerformed2 = _stopwatch.ElapsedMilliseconds;

                if (result1 != result2)
                {
                    Console.WriteLine("Calculation error, exiting...");
                    return;
                }

                LogResults(number, actionsPerformed1, actionsPerformed2, result1);
            }
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

            var Fr1 = Fr(n - 2);
            var Fr2 = Fr(n / 5);
            var Fr3 = Fr(n / 6);

            return Fr1 + 6 * Fr2 * Fr2 + 3 * Fr3 * Fr3 + n * n / 5;
        }
        
        /// <summary>
        /// Calculates F(n)    = F(n - 2) + 6 * F(n / 5) ^ 2 + 3 * F(n / 6) ^ 2 + n ^ 2 / 5; if n > 1
        ///                    = 2; otherwise
        /// recursively using a new thread for every recursive call.
        /// </summary>
        /// <param name="n">Argument of the function</param>
        /// <returns>Result of the function</returns>
        private static long Ft(long n)
        {
            if (n <= 1)
                return 2;

            long Fr1 = 0;
            long Fr2 = 0;
            long Fr3 = 0;
            
            var t1 = new Thread(() => { Fr1 = Ft(n - 2); });
            var t2 = new Thread(() => { Fr2 = Ft(n / 5); });
            var t3 = new Thread(() => { Fr3 = Ft(n / 6); });
            
            t1.Start();
            t2.Start();
            t3.Start();
            
            t1.Join();
            t2.Join();
            t3.Join();

            return Fr1 + 6 * Fr2 * Fr2 + 3 * Fr3 * Fr3 + n * n / 5;
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
                Console.WriteLine("This number is too big for the current task, please try again.");
                number = GetNumber();
            }

            return Convert.ToInt32(number.ToString());
        }
        
        /// <summary>
        /// Asks for input,
        /// catches invalid characters and displays a message,
        /// returns input if greater than 1
        /// or asks again otherwise.
        /// </summary>
        /// <returns>A number greater than 1</returns>
        private static BigInteger GetNumber()
        {
            BigInteger a = 0;

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
                    a = BigInteger.Parse(line);
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            }

            return a;
        }
        
        /// <summary>
        /// Prints the length of the given list of numbers in one line and all the numbers in the other one to the console.
        /// Logs the required amount of actions of each method to file.
        /// </summary>
        /// <param name="number">The given number</param>
        /// <param name="result">The result calculated by the algorithm</param>
        /// <param name="actionsPerformed1">Amount of actions performed to finish the recursive algorithm</param>
        /// <param name="actionsPerformed2">Amount of actions performed to finish the dynamic algorithm</param>
        private static void LogResults(BigInteger number, BigInteger actionsPerformed1, BigInteger actionsPerformed2,
            long result)
        {
            using (var resultStreamWriter = new StreamWriter(ResultsLogFilename, true))
            {
                    Console.WriteLine("Total actions performed recursively in a single thread - {0}, multithreaded - {1}",
                        actionsPerformed1, actionsPerformed2);

                    resultStreamWriter.WriteLine("{0,7}{1,15}{2,15}{3,30}",
                        number, actionsPerformed1, actionsPerformed2, result);
            }

            Console.WriteLine();
        }
    }
}