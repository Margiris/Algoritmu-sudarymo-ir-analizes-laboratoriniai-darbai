using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable InconsistentlySynchronizedField
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ConvertToConstant.Local
// ReSharper disable InconsistentNaming

namespace Lab3
{
    public static class SecondTask
    {
        private const string ResultsLogFilename = @"results.log";
        private static Stopwatch _stopwatch;
        private static int threadCount = 1; // Main runs on the first thread.
        private static readonly object setThreadCount = new object();

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

                LogResults(number, actionsPerformed1, 0, result1);

                _stopwatch = Stopwatch.StartNew();
                var result2 = FtN(number);
                _stopwatch.Stop();
                var actionsPerformed2 = _stopwatch.ElapsedMilliseconds;

                Console.WriteLine("{0}, {1}", result1, result2);

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

            long Ft1 = 0;
            long Ft2 = 0;
            long Ft3 = 0;

            if (threadCount <= Environment.ProcessorCount - 4)
            {
                var t1 = new Thread(() => { Ft1 = Ft(n - 2); });
                var t2 = new Thread(() => { Ft2 = Ft(n / 5); });
                var t3 = new Thread(() => { Ft3 = Ft(n / 6); });

                #region Start Threads

                lock (setThreadCount)
                {
                    t1.Start();
                    threadCount++;
                }

                lock (setThreadCount)
                {
                    t2.Start();
                    threadCount++;
                }

                lock (setThreadCount)
                {
                    t3.Start();
                    threadCount++;
                }

                Console.WriteLine("Number of threads after starting - {0}", threadCount);

                #endregion

                #region Join Threads

                t1.Join();
                lock (setThreadCount)
                {
                    threadCount--;
                }

                t2.Join();
                lock (setThreadCount)
                {
                    threadCount--;
                }

                t3.Join();
                lock (setThreadCount)
                {
                    threadCount--;
                }

                Console.WriteLine("Number of threads after joinning - {0}", threadCount);

                #endregion
            }
            else
            {
                Ft1 = Ft(n - 2);
                Ft2 = Ft(n / 5);
                Ft3 = Ft(n / 6);
            }

            return Ft1 + 6 * Ft2 * Ft2 + 3 * Ft3 * Ft3 + n * n / 5;
        }

        /// <summary>
        /// Calculates F(n)    = F(n - 2) + 6 * F(n / 5) ^ 2 + 3 * F(n / 6) ^ 2 + n ^ 2 / 5; if n > 1
        ///                    = 2; otherwise
        /// recursively using Task class for parallelisation.
        /// </summary>
        /// <param name="n">Argument of the function</param>
        /// <returns>Result of the function</returns>
        private static long FtN(long n)
        {
            if (n <= 1)
                return 2;

            var FtN1 = Task<long>.Factory.StartNew(() => FtN(n - 2));
            var FtN2 = Task<long>.Factory.StartNew(() => FtN(n / 5));
            var FtN3 = Task<long>.Factory.StartNew(() => FtN(n / 6));
            
            return FtN1.Result + 6 * FtN2.Result * FtN2.Result + 3 * FtN3.Result * FtN3.Result + n * n / 5;
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
                Console.WriteLine("Total running time recursively in a single thread - {0}, multithreaded - {1}",
                    actionsPerformed1, actionsPerformed2);

                resultStreamWriter.WriteLine("{0,7}{1,15}{2,15}{3,30}",
                    number, actionsPerformed1, actionsPerformed2, result);
            }

            Console.WriteLine();
        }
    }
}