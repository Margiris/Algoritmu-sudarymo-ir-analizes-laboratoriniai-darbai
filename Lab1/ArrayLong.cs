﻿using System;

namespace Lab1
{
    public abstract class ArrayLong
    {
        public int Length;
        public abstract long this[int index] { get; set; }

        public void CopyTo(ArrayLong array, int index)
        {
            if (array.Length >= Length + index)
                for (var i = index; i < Length; i++)
                    array[i] = this[i];
            else
                throw new NotImplementedException();
        }
    }
}