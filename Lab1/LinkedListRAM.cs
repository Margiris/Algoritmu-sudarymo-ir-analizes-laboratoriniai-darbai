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

        public void AddLast(double data)
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
                };

                Last.AddNext(toAdd);
                Last = NextOf(Last);
            }
        }

        public override LinkedListNode GetFirstNode()
        {
            return First;
        }

        public override LinkedListNode GetNode(int index)
        {
            Current = First;
            
            for (var i = 0; i < index; i++)
            {
                Current = NextOf(Current);
            }

            return Current;
        }

        public override LinkedListNode NextOf(LinkedListNode node)
        {
            return node.Next;
        }
    }
}