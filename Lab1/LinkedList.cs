using System;

namespace Lab1
{
    public abstract class LinkedList
    {
        public class LinkedListNode
        {
            public LinkedListNode Next;
            public double Value;

            public int CurrentIndex;
            public int NextIndex;

            public void AddNext(LinkedListNode node)
            {
                Next = node;
            }
        }
        
        public int Count;
        public LinkedListNode First { get; protected set; }
        protected LinkedListNode Current { get; set; }
        protected LinkedListNode Last { get; set; }

        public abstract LinkedListNode GetFirstNode();
        public abstract LinkedListNode GetNode(int index);
        public abstract LinkedListNode NextOf(LinkedListNode node);

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