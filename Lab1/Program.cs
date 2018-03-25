using System;

namespace Lab1
{
    internal static class Program
    {
        private static void Main()
        {
            const int radixSortCount = 8192;
            const int selectionSortCount = 1024;
            const int step = 2;
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
                        Sort.TestArrayRAM(selectionSortCount, step, seed, SelectionSort.Sort);
                        break;
                    case "2":
                        Sort.TestListRAM(selectionSortCount, step, seed, SelectionSort.Sort);
                        break;
                    case "3":
                        Sort.TestArrayRAM(radixSortCount, step, seed, RadixSort.Sort);
                        break;
                    case "4":
                        Sort.TestListRAM(radixSortCount, step, seed, RadixSort.Sort);
                        break;
                    case "6":
                        Sort.TestArrayDisk(selectionSortCount, step, seed, SelectionSort.Sort);
                        break;
                    case "7":
                        Sort.TestListDisk(selectionSortCount, step, seed, SelectionSort.Sort);
                        break;
                    case "8":
                        Sort.TestArrayDisk(radixSortCount, step, seed, RadixSort.Sort);
                        break;
                    case "9":
                        Sort.TestListDisk(radixSortCount, step, seed, RadixSort.Sort);
                        break;
                    case "11":
                        Sort.TestArrayRAM(selectionSortCount, step, seed, SelectionSort.Sort);
                        Sort.TestListRAM(selectionSortCount, step, seed, SelectionSort.Sort);
                        Sort.TestArrayRAM(selectionSortCount, step, seed, RadixSort.Sort);
                        Sort.TestListRAM(radixSortCount, step, seed, RadixSort.Sort);
                        Sort.TestArrayDisk(selectionSortCount, step, seed, SelectionSort.Sort);
                        Sort.TestListDisk(selectionSortCount, step, seed, SelectionSort.Sort);
                        Sort.TestArrayDisk(radixSortCount, step, seed, RadixSort.Sort);
                        Sort.TestListDisk(radixSortCount, step, seed, RadixSort.Sort);
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