using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab1;

// ReSharper disable InconsistentNaming

namespace SortingTest
{
    [TestClass]
    public class RadixSortTest
    {
        private const string Filename1 = @"testArray1.dat";
        private const string Filename2 = @"testArray2.dat";
        private FileStream _fs1;
        private FileStream _fs2;

        [TestInitialize]
        public void Initialize()
        {
            _fs1 = new FileStream(Filename1, FileMode.Create, FileAccess.ReadWrite);
            _fs2 = new FileStream(Filename2, FileMode.Create, FileAccess.ReadWrite);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _fs1.Dispose();
            _fs2.Dispose();
            if (File.Exists(Filename1))
                File.Delete(Filename1);
            if (File.Exists(Filename2))
                File.Delete(Filename2);
        }

        [TestMethod]
        public void TestArraySort(int length)
        {
            var arrSource1 = Util.DoublesArrayWithRandomValues(length);
            var arrSource2 = new ArrayRAM(length, 0);
            var arrSource3 = new ArrayDisk(Filename1, length, 0) {FileStream = _fs1};

            for (var i = 0; i < length; i++)
            {
                arrSource2[i] = arrSource1[i];
                arrSource3[i] = arrSource1[i];
            }

            if (Util.ArraysAreEqual(arrSource1, arrSource2) != -1 ||
                Util.ArraysAreEqual(arrSource1, arrSource3) != -1) return;

            System.Array.Sort(arrSource1);
            RadixSort.Sort(arrSource2, new ArrayLongRAM(length), new ArrayLongRAM(length),
                new ArrayLongRAM(1 << RadixSort.GroupLength), new ArrayLongRAM(1 << RadixSort.GroupLength));
            RadixSort.Sort(arrSource3, new ArrayLongRAM(length), new ArrayLongRAM(length),
                new ArrayLongRAM(1 << RadixSort.GroupLength), new ArrayLongRAM(1 << RadixSort.GroupLength));

            Assert.AreEqual(-1, Util.ArraysAreEqual(arrSource1, arrSource2));
            Assert.AreEqual(-1, Util.ArraysAreEqual(arrSource1, arrSource3));
        }

        [TestMethod]
        public void TestLinkedListSort(int length)
        {
            var listSource1 = new LinkedList<double>();
            var listSource2 = new LinkedListRAM(length, 0);
            var listSource3 = new LinkedListDisk(Filename1, length, 0) {FileStream = _fs1};
            
            var current = listSource2.First;

            while (current)
            {
                
            }

            for (var i = 0; i < length; i++)
            {
                arrSource2[i] = arrSource1[i];
                arrSource3[i] = arrSource1[i];
            }

            if (Util.ArraysAreEqual(arrSource1, arrSource2) != -1 ||
                Util.ArraysAreEqual(arrSource1, arrSource3) != -1) return;

            System.Array.Sort(arrSource1);
            RadixSort.Sort(arrSource2, new ArrayLongRAM(length), new ArrayLongRAM(length),
                new ArrayLongRAM(1 << RadixSort.GroupLength), new ArrayLongRAM(1 << RadixSort.GroupLength));
            RadixSort.Sort(arrSource3, new ArrayLongRAM(length), new ArrayLongRAM(length),
                new ArrayLongRAM(1 << RadixSort.GroupLength), new ArrayLongRAM(1 << RadixSort.GroupLength));

            Assert.AreEqual(-1, Util.ArraysAreEqual(arrSource1, arrSource2));
            Assert.AreEqual(-1, Util.ArraysAreEqual(arrSource1, arrSource3));
        }
    }
}