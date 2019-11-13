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

        }
    }
}