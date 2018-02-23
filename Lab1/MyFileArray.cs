using System;
using System.IO;

namespace Lab1
{
    class MyFileArray : DataArray
    {
        private double a;
        public MyFileArray(string filename, int n, int seed, FileStream fs)
        {
            Fs = fs;
            var data = new double[n];
            Length = n;
            var rand = new Random(seed);
            for (var i = 0; i < Length; i++)
            {
                data[i] = rand.NextDouble();
            }
            if (File.Exists(filename)) File.Delete(filename);
            try
            {
                using (var writer = new BinaryWriter(File.Open(filename,
                FileMode.Create)))
                {
                    for (var j = 0; j < Length; j++)
                        writer.Write(data[j]);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public FileStream Fs { get; set; }
        public override double this[int index]
        {
            get
            {
                var data = new byte[8];
                Fs.Seek(8 * index, SeekOrigin.Begin);
                Fs.Read(data, 0, 8);
                var result = BitConverter.ToDouble(data, 0);
                return result;
            }
            set => a = value;
        }
        public override void Swap(int j, double a, double b)
        {
            var data = new byte[16];
            BitConverter.GetBytes(a).CopyTo(data, 0);
            BitConverter.GetBytes(b).CopyTo(data, 8);
            Fs.Seek(8 * (j - 1), SeekOrigin.Begin);
            Fs.Write(data, 0, 16);
        }

        public override void Swap(int a, int b)
        {
            throw new NotImplementedException();
        }
    }
}