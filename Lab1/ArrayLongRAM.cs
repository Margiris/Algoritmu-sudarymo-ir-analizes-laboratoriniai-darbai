namespace Lab1
{
    public class ArrayLongRAM : ArrayLong
    {
        public long[] data;

        public ArrayLongRAM(int count)
        {
            data = Util.LongsArrayWithRandomValues(count);
            Length = count;
        }

        public override long this[int index]
        {
            get => data[index];
            set => data[index] = value;
        }
    }
}