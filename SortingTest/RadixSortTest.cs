using System;
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
        private FileStream _fileHandle1;
        private FileStream _fileHandle2;

        [TestInitialize]
        public void Initialize()
        {
            _fileHandle1 = null;
            _fileHandle2 = null;
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (_fileHandle1 != null)
            {
                _fileHandle1.Flush();
                _fileHandle1.Close();
            }

            if (_fileHandle2 != null)
            {
                _fileHandle2.Flush();
                _fileHandle2.Close();
            }

            GC.Collect();

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
            var arrSource3 = new ArrayDisk(Filename1, length, 0);
            _fileHandle1 = arrSource3.FileStream;

            for (var i = 0; i < length; i++)
            {
                arrSource2[i] = arrSource1[i];
                arrSource3[i] = arrSource1[i];
            }

            if (Util.FindArraysDifferenceIndex(arrSource1, arrSource2) != -1 ||
                Util.FindArraysDifferenceIndex(arrSource1, arrSource3) != -1) return;

            System.Array.Sort(arrSource1);
            RadixSort.Sort(arrSource2, new ArrayLongRAM(length), new ArrayLongRAM(length),
                new ArrayLongRAM(1 << RadixSort.GroupLength), new ArrayLongRAM(1 << RadixSort.GroupLength));
            RadixSort.Sort(arrSource3, new ArrayLongRAM(length), new ArrayLongRAM(length),
                new ArrayLongRAM(1 << RadixSort.GroupLength), new ArrayLongRAM(1 << RadixSort.GroupLength));

            Assert.AreEqual(-1, Util.FindArraysDifferenceIndex(arrSource1, arrSource2));
            Assert.AreEqual(-1, Util.FindArraysDifferenceIndex(arrSource1, arrSource3));
        }

        [TestMethod]
        public void TestLinkedListSort(int length)
        {
            var listSource1 = new LinkedList<double>();
            var listSource2 = new LinkedListRAM(length, 0);
            var listSource3 = new LinkedListDisk(Filename1, length, 0);

            var current1 = listSource1.First;
            var current2 = listSource2.First;
            var current3 = listSource3.First;

            while (current2 != null)
            {
                current1.Value = current2.Value;
                current3.Value = current2.Value;
                current2 = listSource2.NextOf(current2);
            }
            Assert.Fail();
//            if (Util.FindArraysDifferenceIndex(arrSource1, arrSource2) != -1 ||
//                Util.FindArraysDifferenceIndex(arrSource1, arrSource3) != -1) return;
//
//            System.Array.Sort(arrSource1);
//            RadixSort.Sort(arrSource2, new ArrayLongRAM(length), new ArrayLongRAM(length),
//                new ArrayLongRAM(1 << RadixSort.GroupLength), new ArrayLongRAM(1 << RadixSort.GroupLength));
//            RadixSort.Sort(arrSource3, new ArrayLongRAM(length), new ArrayLongRAM(length),
//                new ArrayLongRAM(1 << RadixSort.GroupLength), new ArrayLongRAM(1 << RadixSort.GroupLength));
//
//            Assert.AreEqual(-1, Util.FindArraysDifferenceIndex(arrSource1, arrSource2));
//            Assert.AreEqual(-1, Util.FindArraysDifferenceIndex(arrSource1, arrSource3));
        }
    }
}