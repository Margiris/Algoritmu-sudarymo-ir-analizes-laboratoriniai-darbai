using System;

namespace Lab1
{
    internal abstract class Array
    {
        public int Length;
        public abstract double this[int intex] { get; set; }

        public void Print()
        {
            for (var i = 0; i < Length; i++)
            {
                Console.Write(" {0:F5} ", this[i]);
            }

            Console.WriteLine();
        }
    }
}