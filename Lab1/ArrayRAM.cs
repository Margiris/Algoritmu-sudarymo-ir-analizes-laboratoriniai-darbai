using System;

namespace Lab1
{
    internal class ArrayRAM : Array
    {
        public readonly double[] Data;

        public ArrayRAM(int count, int seed)
        {
            Data = new double[count];
            Length = count;
            var rand = new Random(seed);

            for (var i = 0; i < count; i++)
            {
                Data[i] = rand.NextDouble();
            }
        }

        public override double this[int index]
        {
            get => Data[index];
            set => Data[index] = value;
        }
    }
}