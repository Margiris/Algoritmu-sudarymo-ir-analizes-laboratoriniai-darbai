using System;
using System.Collections.Generic;

namespace Lab1
{
    public class LinkedListRAM : LinkedList<double>
    {
        public LinkedListRAM(int count, int seed)
        {
            var rand = new Random(seed);

            for (var i = 0; i < count; i++)
            {
                AddLast(rand.NextDouble());
            }
        }

        public void Print()
        {
            foreach (var number in this)
            {
                Console.Write(" {0:F5} ", number);
            }
            
            Console.WriteLine();
        }
    }
}