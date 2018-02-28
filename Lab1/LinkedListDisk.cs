using System;
using System.IO;

namespace Lab1
{
    internal class LinkedListDisk : LinkedList
    {
        public new class LinkedListNode
        {
            public int CurrentIndex;
            public int NextIndex;
            public double Value;

            public LinkedListNode Next
            {
                get
                {
                    var node = new LinkedListNode();
                    var bytes = new byte[12];

                    fileStream.Seek(0, SeekOrigin.Begin);
                    fileStream.Read(bytes, 0, 4);

                    node.CurrentIndex = BitConverter.ToInt32(bytes, 0);

                    fileStream.Seek(node.CurrentIndex, SeekOrigin.Begin);
                    fileStream.Read(bytes, 0, 12);

                    node.Value = BitConverter.ToDouble(bytes, 0);
                    node.NextIndex = BitConverter.ToInt32(bytes, 8);

                    return node;
                }
            }
        }

        public FileStream fileStream { get; set; }

        public LinkedListDisk(string fileName, int count, int seed)
        {
            Count = count;

            var data = new ArrayRAM(count, seed).Data;

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            try
            {
                using (var writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
                {
                    for (var i = 0; i < count; i++)
                    {
                        writer.Write(i * 12 + 4);
                        writer.Write(data[i]);
                    }

                    writer.Write(count * 12 + 4);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public new LinkedListNode First
        {
            get
            {
                var node = new LinkedListNode();
                var bytes = new byte[12];

                fileStream.Seek(0, SeekOrigin.Begin);
                fileStream.Read(bytes, 0, 4);

                node.CurrentIndex = BitConverter.ToInt32(bytes, 0);

                fileStream.Seek(node.CurrentIndex, SeekOrigin.Begin);
                fileStream.Read(bytes, 0, 12);

                node.Value = BitConverter.ToDouble(bytes, 0);
                node.NextIndex = BitConverter.ToInt32(bytes, 8);

                return node;
            }
        }

        public LinkedListNode GetNode(int index)
        {
            return null;
        }

        public override void AddLast(double data)
        {
            throw new NotImplementedException();
        }
    }
}