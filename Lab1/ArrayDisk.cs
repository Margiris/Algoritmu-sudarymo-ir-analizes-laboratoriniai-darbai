using System;
using System.IO;

namespace Lab1
{
    internal class ArrayDisk : Array
    {
        public ArrayDisk(string fileName, int count, int seed)
        {
            var data = new ArrayRAM(count, seed).Data;

            Length = count;

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            try
            {
                using (var writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
                {
                    for (var i = 0; i < Length; i++)
                    {
                        writer.Write(data[i]);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public FileStream FileStream { private get; set; }

        public override double this[int index]
        {
            get
            {
                var bytes = new byte[8];
                FileStream.Seek(8 * index, SeekOrigin.Begin);
                FileStream.Read(bytes, 0, 8);
                return BitConverter.ToDouble(bytes, 0);
            }
            set
            {
                var bytes = BitConverter.GetBytes(value);
                FileStream.Seek(8 * index, SeekOrigin.Begin);
                FileStream.Write(bytes, 0, 8);
            }
        }
    }
}