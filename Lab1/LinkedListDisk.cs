using System;
using System.IO;

namespace Lab1
{
    internal class LinkedListDisk : LinkedList
    {
        // ReSharper disable once InconsistentNaming
        public FileStream fileStream { private get; set; }

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

        public override LinkedListNode GetFirstNode()
        {
            var bytes = new byte[4];

            fileStream.Seek(0, SeekOrigin.Begin);
            fileStream.Read(bytes, 0, 4);

            var index = BitConverter.ToInt32(bytes, 0);

            return GetNode(index);
        }

        public override LinkedListNode GetNode(int index)
        {
            var bytes = new byte[12];
            var node = new LinkedListNode
            {
                CurrentIndex = index
            };

            fileStream.Seek(node.CurrentIndex, SeekOrigin.Begin);
            fileStream.Read(bytes, 0, 12);

            node.Value = BitConverter.ToDouble(bytes, 0);
            node.NextIndex = BitConverter.ToInt32(bytes, 8);

            return node;
        }

        public override LinkedListNode NextOf(LinkedListNode node)
        {
            return GetNode(node.NextIndex);
        }
    }
}