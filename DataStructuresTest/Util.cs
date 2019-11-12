using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresTest
{
    static class Util
    {
        public static double[] DoublesArrayWIthRandomValues(int length)
        {
            var arr = new double[length];
            var rand = new Random((int) DateTime.Now.Ticks);

            for (var i = 0; i < length; i++)
            {
                arr[i] = rand.NextDouble();
            }

            return arr;
        }
    }
}
