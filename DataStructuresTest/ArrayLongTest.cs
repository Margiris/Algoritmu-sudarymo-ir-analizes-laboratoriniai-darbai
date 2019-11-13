using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab1;

// ReSharper disable InconsistentNaming

namespace DataStructuresTest
{
    [TestClass]
    public class ArrayLongTest
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
            var array3 = new ArrayLongDisk(Filename1, length) {FileStream = _fs1};

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
            var arrayDisk = new ArrayLongDisk(Filename1, length) {FileStream = _fs1};

            arrayRAM[index] = originalNumber;
            arrayDisk[index] = originalNumber;

            Assert.AreEqual(originalNumber, arrayRAM[index],
                "Number value in arrayRAM is different from the original number");
            Assert.AreEqual(originalNumber, arrayDisk[index],
                "Number value in arrayDisk is different from the original number");
        }

        [TestMethod]
        [DataRow(1, 0)]
        [DataRow(68453, 0)]
        [DataRow(68453, 9456)]
        [DataRow(int.MaxValue / 40, 6843215)]
        public void TestCopyToSameType(int length, int index)
        {
            var arrSource1 = Util.LongsArrayWithRandomValues(length);
            var arrSource2 = new ArrayLongRAM(length);
            var arrSource3 = new ArrayLongDisk(Filename1, length) {FileStream = _fs1};

            var arrDestination1 = Util.LongsArrayWithRandomValues(length);
            var arrDestination2 = new ArrayLongRAM(length);
            var arrDestination3 = new ArrayLongDisk(Filename2, length) {FileStream = _fs2};

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

            var diffIndex1 = Util.ArraysAreEqual(arrDestination1, arrDestination2);
            var diffIndex2 = Util.ArraysAreEqual(arrDestination1, arrDestination3);
            var diffIndex3 = Util.ArraysAreEqual(arrDestination2, arrDestination3);

            Assert.AreEqual(-1, diffIndex1,
                $"System.Array and ArrayRAM should be the same but differ at index {diffIndex1}");
            Assert.AreEqual(-1, diffIndex2,
                $"System.Array and ArrayDisk should be the same but differ at index {diffIndex2}");
            Assert.AreEqual(-1, diffIndex3,
                $"ArrayRAM and ArrayDisk should be the same but differ at index {diffIndex3}");
        }

        [TestMethod]
        [DataRow(1, 0)]
        [DataRow(68453, 0)]
        [DataRow(68453, 9456)]
        [DataRow(int.MaxValue / 40, 6843215)]
        public void TestCopyToDifferentType(int length, int index)
        {
            var arrSource1 = Util.LongsArrayWithRandomValues(length);
            var arrSource2 = new ArrayLongRAM(length);
            var arrSource3 = new ArrayLongDisk(Filename1, length) {FileStream = _fs1};

            var arrDestination1 = Util.LongsArrayWithRandomValues(length);
            var arrDestination2 = new ArrayLongRAM(length);
            var arrDestination3 = new ArrayLongDisk(Filename2, length) {FileStream = _fs2};

            for (var i = 0; i < length; i++)
            {
                arrSource2[i] = arrSource1[i];
                arrSource3[i] = arrSource1[i];
                arrDestination2[i] = arrDestination1[i];
                arrDestination3[i] = arrDestination1[i];
            }

            arrSource2.CopyTo(arrDestination3, index);
            arrSource3.CopyTo(arrDestination2, index);

            var diffIndex = Util.ArraysAreEqual(arrDestination2, arrDestination3);

            Assert.AreEqual(-1, diffIndex,
                $"ArrayRAM and ArrayDisk objects should be the same but differ at index {diffIndex}");
        }
        

    }
}