﻿using System;
using System.IO;

namespace Lab1
{
    public class LinkedListDisk : LinkedList
    {
        public FileStream FileStream { get; set; }

        public LinkedListDisk(string fileName, int count, int seed)
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

            Count = count;
//            new ArrayRAM(count, seed).CopyTo(this, 0);
        }

        ~LinkedListDisk()
        {
            Util.CloseFileStream(FileStream);
        }

        public override LinkedListNode GetFirstNode()
        {
            var bytes = new byte[4];

            FileStream.Seek(0, SeekOrigin.Begin);
            FileStream.Read(bytes, 0, 4);

            var index = BitConverter.ToInt32(bytes, 0);

            return GetNode(index);
        }

        public LinkedListNode GetNode(int index)
        {
            var bytes = new byte[12];
            var node = new LinkedListNode
            {
                CurrentIndex = index
            };

            FileStream.Seek(node.CurrentIndex, SeekOrigin.Begin);
            FileStream.Read(bytes, 0, 12);

            node.Value = BitConverter.ToDouble(bytes, 0);
            node.NextIndex = BitConverter.ToInt32(bytes, 8);

            return node;
        }

        public override LinkedListNode NextOf(LinkedListNode node)
        {
            return GetNode(node.NextIndex);
        }

        public override void SetValue(LinkedListNode node, double value)
        {
            var nodeBytes = BitConverter.GetBytes(value);

            FileStream.Seek(node.CurrentIndex, SeekOrigin.Begin);
            FileStream.Write(nodeBytes, 0, 8);
        }

        public override void Swap(LinkedListNode node1, LinkedListNode node2)
        {
            var node1Bytes = BitConverter.GetBytes(node1.Value);
            var node2Bytes = BitConverter.GetBytes(node2.Value);

            FileStream.Seek(node1.CurrentIndex, SeekOrigin.Begin);
            FileStream.Write(node2Bytes, 0, 8);

            FileStream.Seek(node2.CurrentIndex, SeekOrigin.Begin);
            FileStream.Write(node1Bytes, 0, 8);
        }
    }
}