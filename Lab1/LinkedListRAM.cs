using System;

namespace Lab1
{
    public class LinkedListRAM : LinkedList
    {
        public LinkedListRAM(int count, int seed)
        {
            var temp = Util.DoublesArrayWithRandomValues(count, seed);

            Count = count;

            for (var i = 0; i < count; i++)
                AddLast(temp[i]);
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
                    Value = data
                };

                Last.Next = toAdd;
                Last = NextOf(Last);
            }
        }

        public override LinkedListNode GetFirstNode()
        {
            return First;
        }

        public override LinkedListNode NextOf(LinkedListNode node)
        {
            return node.Next;
        }

        public override void SetValue(LinkedListNode node, double value)
        {
            node.Value = value;
        }

        public override void Swap(LinkedListNode node1, LinkedListNode node2)
        {
            var temp = node1.Value;
            node1.Value = node2.Value;
            node2.Value = temp;
        }

//        public virtual LinkedListNode GetNode(int index)
//        {
//            Current = First;
//            
//            for (var i = 0; i < index; i++)
//            {
//                Current = NextOf(Current);
//            }
//
//            return Current;
//        }
    }
}