using System;

namespace Lab1
{
    public class ArrayRAM : Array
    {
        public readonly double[] data;

        public ArrayRAM(int count, int seed)
        {
            data = new double[count];
            Length = count;
            var rand = new Random(seed);

            for (var i = 0; i < count; i++)
            {
                data[i] = rand.NextDouble();
            }
        }

        public override double this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }
    }
}