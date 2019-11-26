using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab1;

// ReSharper disable InconsistentNaming

namespace TestDataStructures
{
    [TestClass]
    public class LinkedListTest
    {
        private const string Filename1 = @"testList1.dat";
        private const string Filename2 = @"testList2.dat";
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
            GC.Collect();
            Util.CloseFileStream(_fileHandle1);
            Util.CloseFileStream(_fileHandle2);

            if (File.Exists(Filename1))
                File.Delete(Filename1);
            if (File.Exists(Filename2))
                File.Delete(Filename2);
        }

        /// <summary>
        /// System.Array, LinkedListRAM and LinkedListDisk objects should be of equal length.
        /// </summary>
        [TestMethod]
        [DataRow(1)]
        [DataRow(int.MaxValue / 40)]
        [DataRow(13601111)]
        [DataRow(69420)]
        public void TestLength(int length)
        {
            var list1 = new double[length];
            var list2 = new LinkedListRAM(length, 0);
            var list3 = new LinkedListDisk(Filename1, length, 0);
            _fileHandle1 = list3.FileStream;

            Assert.AreEqual(length, list2.Count,
                $"List should be of length {length}, now it's {list2.Count}");
            Assert.AreEqual(list1.Length, list2.Count,
                $"System.Array and LinkedListRAM should have same length, now it's {list1.Length} and {list2.Count} respectively");
            Assert.AreEqual(list1.Length, list3.Count,
                $"System.Array and LinkedListDisk should have same length, now it's {list1.Length} and {list3.Count} respectively");
        }

        /// <summary>
        /// When accessed, number value put in list at given index should be same as the original number.
        /// </summary>
        /// <param name="length">Length of the list to be created</param>
        /// <param name="index">Index of list to insert originalNumber to and read value from</param>
        /// <param name="originalNumber">Value to put in list</param>
        [TestMethod]
        [DataRow(1651, 0, 0.54)]
        [DataRow(int.MaxValue / 40, int.MaxValue / 40 - 1, 568432.051684134)]
        [DataRow(1651, 0, -4146465.00000001)]
        [DataRow(int.MaxValue / 40, 354, 0.54)]
        [DataRow(888, 886, 568432.051684134)]
        [DataRow(13532154, 13532154 - 1, double.MinValue)]
        [DataRow(1, 0, 0.54)]
        [DataRow(54, 1, double.MaxValue)]
        [DataRow(54, 22, -4146465.00000001)]
        public void TestGetSetCorrectValues(int length, int index, double originalNumber)
        {
            var listRAM = new LinkedListRAM(length, 0);
            var listDisk = new LinkedListDisk(Filename1, length, 0);
            _fileHandle1 = listDisk.FileStream;

            var current1 = listRAM.GetFirstNode();
            var current2 = listDisk.GetFirstNode();

            for (var i = 0; i < index; i++)
            {
                current1 = listRAM.NextOf(current1);
                current2 = listDisk.NextOf(current2);
            }

            listRAM.SetValue(current1, originalNumber);
            listDisk.SetValue(current2, originalNumber);

            Assert.AreEqual(originalNumber, current1.Value,
                "Number value in listRAM is different from the original number");
            Assert.AreEqual(originalNumber, current2.Value,
                "Number value in listDisk is different from the original number");
        }
        
        [TestMethod]
        [DataRow(1, 0, 0)]
        [DataRow(163484, 6341, 46451)]
        [DataRow(int.MaxValue / 40, int.MaxValue / 1354, 153)]
        public void TestSwap(int length, int index1, int index2)
        {
            var listRAM = new LinkedListRAM(length, 0);
            var listDisk = new LinkedListDisk(Filename1, length, 0);
            _fileHandle1 = listDisk.FileStream;

            TestSwap(listRAM, index1, index2);
            TestSwap(listDisk, index1, index2);
        }

        public void TestSwap(LinkedList list, int index1, int index2)
        {
            var node1 = list.GetFirstNode();

            for (var i = 0; i < index1; i++)
                node1 = list.NextOf(node1);
            
            var node2 = list.GetFirstNode();

            for (var i = 0; i < index2; i++)
                node2 = list.NextOf(node2);

            var value1 = node1.Value;
            var value2 = node2.Value;

            list.Swap(node1, node2);

            Assert.AreEqual(value1, node2.Value);
            Assert.AreEqual(value2, node1.Value);
        }

        /// <summary>
        /// Two lists created with same seed should be equal
        /// number-by-number.
        /// 
        /// Given same seed, System.Random provides same random
        /// number sequence.
        /// </summary>
        [TestMethod]
        [DataRow(1654, 0)]
        [DataRow(96384, 1)]
        [DataRow(999999, 167483521)]
        [DataRow(int.MaxValue / 40, int.MaxValue)]
        [DataRow(int.MaxValue / 40, 48)]
        [DataRow(2, int.MaxValue)]
        [DataRow(135787, int.MaxValue)]
        [DataRow(96485, int.MinValue)]
        public void TestRandomnessWithSameSeed(int length, int seed)
        {
            var arr1 = new LinkedListRAM(length, seed);
            var arr2 = new LinkedListRAM(length, seed);

            var diffIndex = Util.FindListsDifferenceIndex(arr1, arr2);
            Assert.AreEqual(-1, diffIndex,
                $"Two lists created with same seed should be equal number-by-number, failed at index {diffIndex}");
        }

        /// <summary>
        /// Two lists created with different positive seeds
        /// should not be equal item-by-item more than 0.1 %.
        ///
        /// By System.Random implementation absolute value of
        /// seed is used, so testing negative numbers is redundant.
        /// </summary>
        [TestMethod]
        [DataRow(1654, 0, 1)]
        [DataRow(96384, 164231, 64135)]
        [DataRow(999999, 1650, 41635)]
        [DataRow(int.MaxValue / 40, 46213, 0)]
        [DataRow(int.MaxValue / 40, 1, int.MaxValue)]
        [DataRow(2, int.MaxValue, int.MaxValue - 1)]
        [DataRow(135787, int.MaxValue - 1, int.MaxValue)]
        public void TestRandomnessWithDifferentSeeds(int length, int seed1, int seed2)
        {
            var list1 = new LinkedListRAM(length, seed1);
            var list2 = new LinkedListRAM(length, seed2);

            var equalityCoefficient = 0.0;
            
            var current1 = list1.GetFirstNode();
            var current2 = list2.GetFirstNode();

            for (var i = 0; i < list1.Count; i++)
            {
                if (Math.Abs(current1.Value - current2.Value) > 0.00001)
                    equalityCoefficient += 1.0 / length;
                current1 = list1.NextOf(current1);
                current2 = list2.NextOf(current2);
            }

            Assert.IsTrue(equalityCoefficient < 0.001,
                $"Two lists with different seeds should not be equal item-by-item more than 0.1 %, currently it's {equalityCoefficient} %");
        }
    }
}