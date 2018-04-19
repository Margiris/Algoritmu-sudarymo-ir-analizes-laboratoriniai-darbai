using System;

namespace Lab2
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var a = GetInteger();

            Console.WriteLine(a);
        }

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
    }
}