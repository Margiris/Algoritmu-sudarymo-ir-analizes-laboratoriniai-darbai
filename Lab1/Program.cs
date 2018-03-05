using System;

namespace Lab1
{
    internal static class Program
    {
        private static void Main()
        {
            const int count = 4;
            const int step = 4;
            var seed = (int) DateTime.Now.Ticks;
            var isRunning = true;

            PrintMenu();

            while (isRunning)
            {
                Console.Write("Enter operation number: ");

                switch (Console.ReadLine())
                {
                    case "help":
                        PrintMenu();
                        break;
                    case "1":
                        Sort.TestArrayRAM(count, step, seed, SelectionSort.Sort);
                        break;
                    case "2":
                        Sort.TestListRAM(count, step, seed, SelectionSort.Sort);
                        break;
                    case "3":
                        Sort.TestArrayRAM(count, step, seed, RadixSort.Sort);
                        break;
                    case "4":
                        Sort.TestListRAM(count, step, seed, RadixSort.Sort);
                        break;
                    case "6":
                        Sort.TestArrayDisk(count, step, seed, SelectionSort.Sort);
                        break;
                    case "7":
                        Sort.TestListDisk(count, step, seed, SelectionSort.Sort);
                        break;
                    case "8":
                        Sort.TestArrayDisk(count, step, seed, RadixSort.Sort);
                        break;
                    case "9":
                        Sort.TestListDisk(count, step, seed, RadixSort.Sort);
                        break;
                    case "11":
                        Sort.TestArrayRAM(count, step, seed, SelectionSort.Sort);
                        Sort.TestListRAM(count, step, seed, SelectionSort.Sort);
                        Sort.TestArrayRAM(count, step, seed, RadixSort.Sort);
                        Sort.TestListRAM(count, step, seed, RadixSort.Sort);
                        Sort.TestArrayDisk(count, step, seed, SelectionSort.Sort);
                        Sort.TestListDisk(count, step, seed, SelectionSort.Sort);
                        Sort.TestArrayDisk(count, step, seed, RadixSort.Sort);
                        Sort.TestListDisk(count, step, seed, RadixSort.Sort);
                        break;
                    case "x":
                        isRunning = false;
                        break;
                    default:
                        Console.Write("Invalid selection. ");
                        break;
                }
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("Print this menu              help");
            Console.WriteLine("Tests from RAM:");
            Console.WriteLine("Selection sort of an array      1");
            Console.WriteLine("Selection sort of a list        2");
            Console.WriteLine("    Radix sort of an array      3");
            Console.WriteLine("    Radix sort of a list        4");
            Console.WriteLine("Search in red-black tree        5");
            Console.WriteLine("Tests from disk:");
            Console.WriteLine("Selection sort of an array      6");
            Console.WriteLine("Selection sort of a list        7");
            Console.WriteLine("    Radix sort of an array      8");
            Console.WriteLine("    Radix sort of a list        9");
            Console.WriteLine("Search in red-black tree       10");
            Console.WriteLine();
            Console.WriteLine("All of the above               11");
            Console.WriteLine("Exit                            x");
            Console.WriteLine();
        }
    }
}