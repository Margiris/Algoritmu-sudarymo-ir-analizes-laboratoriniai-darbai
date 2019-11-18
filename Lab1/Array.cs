using System;
using System.Diagnostics;

namespace Lab1
{
    public abstract class Array
    {
        public int Length;
        public abstract double this[int index] { get; set; }

        public void CopyTo(Array array, int index)
        {
            if (array.Length >= Length + index)
                for (var i = 0; i < Length; i++)
                    array[index + i] = this[i];
            else
                throw new NotImplementedException();
        }

        public void Swap(int index1, int index2)
        {
            var temp = this[index1];
            this[index1] = this[index2];
            this[index2] = temp;
        }

        public void Print()
        {
            for (var i = 0; i < Length; i++)
            {
                Console.Write("{0:F5} ", this[i]);
            }

            Console.WriteLine();
        }
    }
}