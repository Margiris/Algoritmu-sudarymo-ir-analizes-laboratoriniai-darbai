using System;
using System.IO;

namespace Lab1
{
    class MyFileList : DataList
    {
        int _prevNode;
        int _currentNode;
        int _nextNode;
        public MyFileList(string filename, int n, int seed, FileStream fs)
        {
            Fs = fs;
            Length = n;
            var rand = new Random(seed);
            if (File.Exists(filename)) File.Delete(filename);
            try
            {
                using (var writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    writer.Write(4);
                    for (var j = 0; j < Length; j++)
                    {
                        writer.Write(rand.NextDouble());
                        writer.Write((j + 1) * 12 + 4);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        
        public FileStream Fs { get; set; }
        public double Head()
        {
            var data = new Byte[12];
            Fs.Seek(0, SeekOrigin.Begin);
            Fs.Read(data, 0, 4);
            _currentNode = BitConverter.ToInt32(data, 0);
            _prevNode = -1;
            Fs.Seek(_currentNode, SeekOrigin.Begin);
            Fs.Read(data, 0, 12);
            var result = BitConverter.ToDouble(data, 0);
            _nextNode = BitConverter.ToInt32(data, 8);
            return result;
        }
        
        public double Next()
        {
            var data = new Byte[12];
            Fs.Seek(_nextNode, SeekOrigin.Begin);
            Fs.Read(data, 0, 12);
            _prevNode = _currentNode;
            _currentNode = _nextNode;
            var result = BitConverter.ToDouble(data, 0);
            _nextNode = BitConverter.ToInt32(data, 8);
            return result;
        }
        public void Swap(double a, double b)
        {
            Byte[] data;
            Fs.Seek(_prevNode, SeekOrigin.Begin);
            data = BitConverter.GetBytes(a);
            Fs.Write(data, 0, 8);
            Fs.Seek(_currentNode, SeekOrigin.Begin);
            data = BitConverter.GetBytes(b);
            Fs.Write(data, 0, 8);
        }
    }
}

