using System;
using System.IO;

namespace Lab1
{
    internal class ArrayLongDisk : ArrayLong
    {
        public ArrayLongDisk(string fileName, int count)
        {
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
                        writer.Write(i);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public FileStream FileStream { private get; set; }

        public override long this[int index]
        {
            get
            {
                var bytes = new byte[8];
                FileStream.Seek(8 * index, SeekOrigin.Begin);
                FileStream.Read(bytes, 0, 8);
                return BitConverter.ToInt64(bytes, 0);
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