namespace Lab1
{
    internal class ArrayLongRAM : ArrayLong
    {
        public readonly long[] Data;

        public ArrayLongRAM(int count)
        {
            Data = new long[count];
            Length = count;
        }

        public override long this[int index]
        {
            get => Data[index];
            set => Data[index] = value;
        }
    }
}