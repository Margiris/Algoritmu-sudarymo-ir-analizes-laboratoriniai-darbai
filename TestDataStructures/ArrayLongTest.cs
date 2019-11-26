using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab1;

// ReSharper disable InconsistentNaming

namespace TestDataStructures
{
    [TestClass]
    public class ArrayLongTest
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
            var array2 = new ArrayLongRAM(length);
            var array3 = new ArrayLongDisk(Filename1, length);
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
        [DataRow(1651, 0, 54)]
        [DataRow(int.MaxValue / 40, int.MaxValue / 40 - 1, 568432)]
        [DataRow(1651, 0, -4146465)]
        [DataRow(int.MaxValue / 40, 354, 54)]
        [DataRow(888, 886, 568432)]
        [DataRow(13532154, 13532154 - 1, long.MinValue)]
        [DataRow(1, 0, 54)]
        [DataRow(54, 1, long.MaxValue)]
        [DataRow(54, 22, -4146465)]
        public void TestGetSetCorrectValues(int length, int index, long originalNumber)
        {
            var arrayRAM = new ArrayLongRAM(length);
            var arrayDisk = new ArrayLongDisk(Filename1, length);
            _fileHandle1 = arrayDisk.FileStream;

            arrayRAM[index] = originalNumber;
            arrayDisk[index] = originalNumber;

            Assert.AreEqual(originalNumber, arrayRAM[index],
                "Number value in arrayRAM is different from the original number");
            Assert.AreEqual(originalNumber, arrayDisk[index],
                "Number value in arrayDisk is different from the original number");
        }
        
        /// <summary>
        /// CopyTo method should work the same as System.Array.CopyTo(), which reads:
        /// Copies all the elements of the current one-dimensional array to the specified
        /// one-dimensional array starting at the specified destination array index. The index is specified as a 32-bit integer.
        ///
        /// This test checks CopyTo correctness when copying data to structure of the same type as source.
        /// </summary>
        /// <param name="length">Length of the array to be created</param>
        /// <param name="index">Index of the array to start copying from</param>
        [TestMethod]
        [DataRow(6, 0)]
        [DataRow(6, 4)]
        [DataRow(1, 0)]
        [DataRow(68453, 0)]
        [DataRow(68453, 9456)]
        [DataRow(int.MaxValue / 400, 3843215)]
        public void TestCopyToSameType(int length, int index)
        {
            var arrSource1 = Util.LongsArrayWithRandomValues(length);
            var arrSource2 = new ArrayLongRAM(length);
            var arrSource3 = new ArrayLongDisk(Filename1, length);
            _fileHandle1 = arrSource3.FileStream;

            var arrDestination1 = Util.LongsArrayWithRandomValues(length + index);
            var arrDestination2 = new ArrayLongRAM(length + index);
            var arrDestination3 = new ArrayLongDisk(Filename2, length + index);
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
        
        /// <summary>
        /// When trying to CopyTo a smaller array than length of source + index, method should throw NotImplementedException.
        /// </summary>
        [TestMethod]
        public void TestCopyToTooSmallDestinationArray()
        {
            var arrSource = new ArrayLongRAM(20);
            var arrDestination = new ArrayLongRAM(10);

            Assert.ThrowsException<NotImplementedException>(() => arrSource.CopyTo(arrDestination, 0));
        }

        /// <summary>
        /// CopyTo method should work the same as System.Array.CopyTo(), which reads:
        /// Copies all the elements of the current one-dimensional array to the specified
        /// one-dimensional array starting at the specified destination array index. The index is specified as a 32-bit integer.
        ///
        /// This test checks CopyTo correctness when copying data to structure of different type than source.
        /// </summary>
        /// <param name="length">Length of the array to be created</param>
        /// <param name="index">Index of the array to start copying from</param>
        [TestMethod]
        [DataRow(6, 0)]
        [DataRow(6, 4)]
        [DataRow(1, 0)]
        [DataRow(68453, 0)]
        [DataRow(68453, 9456)]
        [DataRow(int.MaxValue / 400, 3843215)]
        public void TestCopyToDifferentType(int length, int index)
        {
            var arrSource1 = Util.LongsArrayWithRandomValues(length);
            var arrSource2 = new ArrayLongRAM(length);
            var arrSource3 = new ArrayLongDisk(Filename1, length);
            _fileHandle1 = arrSource3.FileStream;

            var arrDestination1 = Util.LongsArrayWithRandomValues(length + index);
            var arrDestination2 = new ArrayLongRAM(length + index);
            var arrDestination3 = new ArrayLongDisk(Filename2, length + index);
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
        
        /// <summary>
        /// Should swap values at specified indexes in ArrayLongRAM type object.
        /// </summary>
        /// <param name="length">Length of the array to be created</param>
        /// <param name="index1">First index for swapping</param>
        /// <param name="index2">Second index for swapping</param>
        [TestMethod]
        [DataRow(1, 0, 0)]
        [DataRow(163484, 6341, 46451)]
        [DataRow(int.MaxValue / 40, int.MaxValue / 1354, 153)]
        public void TestSwapRAM(int length, int index1, int index2)
        {
            var arrayRAM = new ArrayLongRAM(length);

            TestSwap(arrayRAM, index1, index2);
        }
        
        /// <summary>
        /// Should swap values at specified indexes in ArrayLongDisk type object.
        /// </summary>
        /// <param name="length">Length of the array to be created</param>
        /// <param name="index1">First index for swapping</param>
        /// <param name="index2">Second index for swapping</param>
        [TestMethod]
        [DataRow(1, 0, 0)]
        [DataRow(163484, 6341, 46451)]
        [DataRow(int.MaxValue / 40, int.MaxValue / 1354, 153)]
        public void TestSwapDisk(int length, int index1, int index2)
        {
            var arrayDisk = new ArrayLongDisk(Filename1, length);
            _fileHandle1 = arrayDisk.FileStream;

            TestSwap(arrayDisk, index1, index2);
        }
        
        /// <summary>
        /// Helper method for testing Swap method.
        /// </summary>
        /// <param name="array">Array to swap values in</param>
        /// <param name="index1">First index for swapping</param>
        /// <param name="index2">Second index for swapping</param>
        public void TestSwap(ArrayLong array, int index1, int index2)
        {
            var value1 = array[index1];
            var value2 = array[index2];

            array.Swap(index1, index2);

            Assert.AreEqual(value1, array[index2]);
            Assert.AreEqual(value2, array[index1]);
        }
    }
}