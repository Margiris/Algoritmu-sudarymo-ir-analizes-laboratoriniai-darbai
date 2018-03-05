using System;

namespace Lab1
{
    public abstract class LinkedList
    {
        public class LinkedListNode
        {
            public LinkedListNode Next;

            public double Value { get; set; }

            public int CurrentIndex;
            public int NextIndex;
        }

        public int Count;
        public LinkedListNode First { get; protected set; }
        protected LinkedListNode Last { get; set; }

        public abstract LinkedListNode GetFirstNode();
        public abstract LinkedListNode NextOf(LinkedListNode node);
        public abstract void SetValue(LinkedListNode node, double value);
        public abstract void Swap(LinkedListNode node1, LinkedListNode node2);

        public void Print()
        {
            var current = GetFirstNode();

            for (var i = 0; i < Count; i++)
            {
                Console.Write("{0:F5} ", current.Value);
                current = NextOf(current);
            }

            Console.WriteLine();
        }
    }
}