using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab1;

// ReSharper disable InconsistentNaming

namespace TestDataStructures
{
    [TestClass]
    public class ArrayTest
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
            GC.Collect();
            Util.CloseFileStream(_fileHandle1);
            Util.CloseFileStream(_fileHandle2);

            if (File.Exists(Filename1))
                File.Delete(Filename1);
            if (File.Exists(Filename2))
                File.Delete(Filename2);
        }

        /// <summary>
        /// System.Array, ArrayRAM and ArrayDisk objects should be of equal length.
        /// </summary>
        [TestMethod]
        [DataRow(1)]
        [DataRow(int.MaxValue / 40)]
        [DataRow(13601111)]
        [DataRow(69420)]
        public void TestLength(int length)
        {
            var array1 = new double[length];
            var array2 = new ArrayRAM(length, 0);
            var array3 = new ArrayDisk(Filename1, length, 0);
            _fileHandle1 = array3.FileStream;

            Assert.AreEqual(length, array2.Length,
                $"Array should be of length {length}, now it's {array2.Length}");
            Assert.AreEqual(array1.Length, array2.Length,
                $"System.Array and ArrayRAM should have same length, now it's {array1.Length} and {array2.Length} respectively");
            Assert.AreEqual(array1.Length, array3.Length,
                $"System.Array and ArrayDisk should have same length, now it's {array1.Length} and {array3.Length} respectively");
        }

        /// <summary>
        /// When accessed, number value put in array at given index should be same as the original number.
        /// </summary>
        /// <param name="length">Length of the array to be created</param>
        /// <param name="index">Index of array to insert originalNumber to and read value from</param>
        /// <param name="originalNumber">Value to put in array</param>
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
            var arrayRAM = new ArrayRAM(length, 0);
            var arrayDisk = new ArrayDisk(Filename1, length, 0);
            _fileHandle1 = arrayDisk.FileStream;

            arrayRAM[index] = originalNumber;
            arrayDisk[index] = originalNumber;

            Assert.AreEqual(originalNumber, arrayRAM[index],
                "Number value in arrayRAM is different from the original number");
            Assert.AreEqual(originalNumber, arrayDisk[index],
                "Number value in arrayDisk is different from the original number");
        }

        [TestMethod]
        [DataRow(6, 0)]
        [DataRow(6, 4)]
        [DataRow(1, 0)]
        [DataRow(68453, 0)]
        [DataRow(68453, 9456)]
        [DataRow(int.MaxValue / 400, 3843215)]
        public void TestCopyToSameType(int length, int index)
        {
            var arrSource1 = Util.DoublesArrayWithRandomValues(length);
            var arrSource2 = new ArrayRAM(length, 0);
            var arrSource3 = new ArrayDisk(Filename1, length, 0);
            _fileHandle1 = arrSource3.FileStream;

            var arrDestination1 = Util.DoublesArrayWithRandomValues(length + index);
            var arrDestination2 = new ArrayRAM(length + index, 0);
            var arrDestination3 = new ArrayDisk(Filename2, length + index, 0);
            _fileHandle2 = arrDestination3.FileStream;

            for (var i = 0; i < length; i++)
            {
                arrSource2[i] = arrSource1[i];
                arrSource3[i] = arrSource1[i];
                arrDestination2[i] = arrDestination1[i];
                arrDestination3[i] = arrDestination1[i];
            }

            arrSource1.CopyTo(arrDestination1, index);
            arrSource2.CopyTo(arrDestination2, index);
            arrSource3.CopyTo(arrDestination3, index);

            var diffIndex1 = Util.FindArraysDifferenceIndex(arrDestination1, arrDestination2);
            var diffIndex2 = Util.FindArraysDifferenceIndex(arrDestination1, arrDestination3);
            var diffIndex3 = Util.FindArraysDifferenceIndex(arrDestination2, arrDestination3);

            Assert.AreEqual(-1, diffIndex1,
                $"System.Array and ArrayRAM should be the same but differ at index {diffIndex1}");
            Assert.AreEqual(-1, diffIndex2,
                $"System.Array and ArrayDisk should be the same but differ at index {diffIndex2}");
            Assert.AreEqual(-1, diffIndex3,
                $"ArrayRAM and ArrayDisk should be the same but differ at index {diffIndex3}");
        }

        [TestMethod]
        public void TestCopyToTooSmallDestinationArray()
        {
            var arrSource = new ArrayRAM(20, 0);
            var arrDestination = new ArrayRAM(10, 0);

            Assert.ThrowsException<NotImplementedException>(() => arrSource.CopyTo(arrDestination, 0));
        }

        [TestMethod]
        [DataRow(6, 0)]
        [DataRow(6, 4)]
        [DataRow(1, 0)]
        [DataRow(68453, 0)]
        [DataRow(68453, 9456)]
        [DataRow(int.MaxValue / 400, 3843215)]
        public void TestCopyToDifferentType(int length, int index)
        {
            var arrSource1 = Util.DoublesArrayWithRandomValues(length);
            var arrSource2 = new ArrayRAM(length, 0);
            var arrSource3 = new ArrayDisk(Filename1, length, 0);
            _fileHandle1 = arrSource3.FileStream;

            var arrDestination1 = Util.DoublesArrayWithRandomValues(length + index);
            var arrDestination2 = new ArrayRAM(length + index, 0);
            var arrDestination3 = new ArrayDisk(Filename2, length + index, 0);
            _fileHandle2 = arrDestination3.FileStream;

            for (var i = 0; i < length; i++)
            {
                arrSource2[i] = arrSource1[i];
                arrSource3[i] = arrSource1[i];
                arrDestination2[i] = arrDestination1[i];
                arrDestination3[i] = arrDestination1[i];
            }

            arrSource2.CopyTo(arrDestination3, index);
            arrSource3.CopyTo(arrDestination2, index);

            var diffIndex = Util.FindArraysDifferenceIndex(arrDestination2, arrDestination3);

            Assert.AreEqual(-1, diffIndex,
                $"ArrayRAM and ArrayDisk objects should be the same but differ at index {diffIndex}");
        }

        [TestMethod]
        [DataRow(1, 0, 0)]
        [DataRow(163484, 6341, 46451)]
        [DataRow(int.MaxValue / 40, int.MaxValue / 1354, 153)]
        public void TestSwapRAM(int length, int index1, int index2)
        {
            var arrayRAM = new ArrayRAM(length, 0);

            TestSwap(arrayRAM, index1, index2);
        }

        [TestMethod]
        [DataRow(1, 0, 0)]
        [DataRow(163484, 6341, 46451)]
        [DataRow(int.MaxValue / 40, int.MaxValue / 1354, 153)]
        public void TestSwapDisk(int length, int index1, int index2)
        {
            var arrayDisk = new ArrayDisk(Filename1, length, 0);
            _fileHandle1 = arrayDisk.FileStream;

            TestSwap(arrayDisk, index1, index2);
        }

        public void TestSwap(Lab1.Array array, int index1, int index2)
        {
            var value1 = array[index1];
            var value2 = array[index2];

            array.Swap(index1, index2);

            Assert.AreEqual(value1, array[index2]);
            Assert.AreEqual(value2, array[index1]);
        }

        /// <summary>
        /// Two arrays created with same seed should be equal
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
        [DataRow(135787, int.MaxValue)]
        [DataRow(96485, int.MinValue)]
        public void TestRandomnessWithSameSeed(int length, int seed)
        {
            var arr1 = new ArrayRAM(length, seed);
            var arr2 = new ArrayRAM(length, seed);

            var diffIndex = Util.FindArraysDifferenceIndex(arr1, arr2);
            Assert.AreEqual(-1, diffIndex,
                $"Two arrays created with same seed should be equal number-by-number, failed at index {diffIndex}");
        }

        /// <summary>
        /// Two arrays created with different positive seeds
        /// should not be equal item-by-item more than 0.1 %.
        ///
        /// By System.Random implementation absolute value of
        /// seed is used, so testing negative numbers is redundant.
        /// </summary>
        [TestMethod]
        [DataRow(1654, 0, 1)]
        [DataRow(96384, 164231, 64135)]
        [DataRow(999999, 1650, 41635)]
        [DataRow(int.MaxValue / 40, 1, int.MaxValue)]
        [DataRow(2, int.MaxValue, int.MaxValue - 1)]
        public void TestRandomnessWithDifferentSeeds(int length, int seed1, int seed2)
        {
            var arr1 = new ArrayRAM(length, seed1);
            var arr2 = new ArrayRAM(length, seed2);

            var equalityCoefficient = 0.0;

            for (var i = 0; i < length; i++)
                if (Math.Abs(arr1[i] - arr2[i]) < 0.00001)
                    equalityCoefficient += 1.0 / length;

            Assert.IsTrue(equalityCoefficient < 0.001,
                $"Two arrays with different seeds should not be equal item-by-item more than 0.1 %, currently it's {equalityCoefficient} %");
        }
    }
}