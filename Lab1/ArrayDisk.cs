using System;
using System.Diagnostics;
using System.IO;

namespace Lab1
{
    public class ArrayDisk : Array, IDisposable
    {
        public ArrayDisk(string fileName, int count, int seed)
        {
            try
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);

                FileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Length = count;
            new ArrayRAM(count, seed).CopyTo(this, 0);
        }

        public void Dispose()
        {
            FileStream.Flush();
            FileStream.Close();
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