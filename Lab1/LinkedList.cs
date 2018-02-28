using System;

namespace Lab1
{
    internal abstract class LinkedList
    {
        public class LinkedListNode
        {
            public LinkedListNode Previous;
            public LinkedListNode Next;
            public double Value;
        }

        public int Count;
        public LinkedListNode First { get; protected set; }
        public LinkedListNode Last { get; set; }

        public abstract void AddLast(double data);
        
        public void Print()
        {
            var current = First;

            while (current != null)
            {
                Console.Write("{0:F5} ", current.Value);
                current = (LinkedListNode)current.Next;
            }

            Console.WriteLine();
        }
    }
}