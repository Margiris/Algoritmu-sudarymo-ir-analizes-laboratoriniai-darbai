﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lab1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Array = System.Array;

namespace TestSorting
{
    [TestClass]
    public class SelectionSortTest
    {
        private const string Filename = @"testArray1.dat";
        private const string a_Filename = @"a.dat";
        private const string t_Filename = @"t.dat";
        private const string count_Filename = @"count.dat";
        private const string pref_Filename = @"pref.dat";

        private List<string> filenames;
        private List<FileStream> _fileHandles;

        [TestInitialize]
        public void Initialize()
        {
            filenames = new List<string> {Filename, a_Filename, t_Filename, count_Filename, pref_Filename};
            _fileHandles = new List<FileStream>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            GC.Collect();

            foreach (var fileHandle in _fileHandles)
                Util.CloseFileStream(fileHandle);

            foreach (var file in filenames.Where(File.Exists))
                File.Delete(file);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(64)]
        [DataRow(256)]
        [DataRow(4096)]
        [DataRow(10240)]
//        [DataRow(int.MaxValue / 40)]
        public void TestArraySort(int length)
        {
            var arrSource1 = Util.DoublesArrayWithRandomValues(length);
            var arrSource2 = new ArrayRAM(length, 0);
            var arrSource3 = new ArrayDisk(Filename, length, 0);
            _fileHandles.Add(arrSource3.FileStream);

            var a = new ArrayLongDisk(a_Filename, length);
            var t = new ArrayLongDisk(t_Filename, length);
            var count = new ArrayLongDisk(count_Filename, 1 << RadixSort.GroupLength);
            var pref = new ArrayLongDisk(pref_Filename, 1 << RadixSort.GroupLength);
            _fileHandles.Add(a.FileStream);
            _fileHandles.Add(t.FileStream);
            _fileHandles.Add(count.FileStream);
            _fileHandles.Add(pref.FileStream);

            for (var i = 0; i < length; i++)
            {
                arrSource2[i] = arrSource1[i];
                arrSource3[i] = arrSource1[i];
            }

            if (Util.FindArraysDifferenceIndex(arrSource1, arrSource2) != -1 ||
                Util.FindArraysDifferenceIndex(arrSource1, arrSource3) != -1) return;
            
            System.Array.Sort((Array) arrSource1);
            SelectionSort.Sort(arrSource2, new ArrayLongRAM(length), new ArrayLongRAM(length),
                new ArrayLongRAM(1 << RadixSort.GroupLength), new ArrayLongRAM(1 << RadixSort.GroupLength));
            SelectionSort.Sort(arrSource3, a, t, count, pref);
            
            Assert.AreEqual(-1, Util.FindArraysDifferenceIndex(arrSource1, arrSource2));
            Assert.AreEqual(-1, Util.FindArraysDifferenceIndex(arrSource1, arrSource3));
        }
    }
}