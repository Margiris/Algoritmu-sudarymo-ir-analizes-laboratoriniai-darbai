using System;

namespace Lab1
{
    internal class LinkedListRAM : LinkedList
    {
        public LinkedListRAM(int count, int seed)
        {
            Count = count;
            var rand = new Random(seed);

            for (var i = 0; i < count; i++)
            {
                AddLast(rand.NextDouble());
            }
        }

        public sealed override void AddLast(double data)
        {
            if (Last == null)
            {
                First = new LinkedListNode
                {
                    Value = data
                };

                Last = First;
            }
            else
            {
                var toAdd = new LinkedListNode
                {
                    Value = data,
                    Previous = Last
                };

                Last.Next = toAdd;
                Last = toAdd;
            }
        }
    }
}