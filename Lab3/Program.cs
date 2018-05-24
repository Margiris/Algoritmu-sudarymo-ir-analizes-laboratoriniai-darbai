using System;
using System.Threading;

namespace Lab3
{
    internal static class Program
    {
        public static void Main()
        {
//            var firstTask = new Thread(FirstTask.Run, 1000000000);
//            firstTask.Start();
//            firstTask.Join();

            var secondTask = new Thread(SecondTask.Run, 2000000000);
            secondTask.Start();
            secondTask.Join();
        }
    }
}