using System;
using System.IO;

namespace Lab1
{
    public class ArrayLongDisk : ArrayLong
    {
        public ArrayLongDisk(string fileName, int count)
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
            new ArrayLongRAM(count).CopyTo(this, 0);
        }

        ~ArrayLongDisk()
        {
            Util.CloseFileStream(FileStream);
        }

        public FileStream FileStream { get; set; }

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