using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lab1;

// ReSharper disable InconsistentNaming

namespace DataStructuresTest
{
    [TestClass]
    public class ArrayRAMTest
    {
        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        public int length = 150345;
        public double[] normalDoublesArray;
        public ArrayRAM projectDoublesArray;

        [TestInitialize]
        public void Initialize()
        {
            normalDoublesArray = new double[length];
            projectDoublesArray = new ArrayRAM(length, 0);

            var randomArray = Util.DoublesArrayWIthRandomValues(length);

            for (int i = 0; i < length; i++)
            {
                normalDoublesArray[i] = randomArray[i];
                projectDoublesArray[i] = randomArray[i];
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            normalDoublesArray = null;
            projectDoublesArray = null;
        }

        /// <summary>
        /// Length of System.Array and ArrayRAM created from the same array should be equal.
        /// </summary>
        [TestMethod]
        public void TestLength()
        {
            Assert.AreEqual(length, projectDoublesArray.Length);
            Assert.AreEqual(normalDoublesArray.Length, projectDoublesArray.Length);
        }

        [TestMethod]
        public void TestGetSet()
        {
            Assert.Fail();
        }

        /// <summary>
        /// Two arrays created with same seed should not be equal number-by-number more than 1 %.
        /// </summary>
        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(167483521)]
        [DataRow(-8432)]
        [DataRow(48)]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void TestRandomnessWithSameSeed(int seed)
        {
            var arr1 = new ArrayRAM(length, seed);
            var arr2 = new ArrayRAM(length, seed);

            var equalityCoefficient = 0.0;

            for (var i = 0; i < length; i++)
                if (Math.Abs(arr1[i] - arr2[i]) < 0.00001)
                {
                    equalityCoefficient += 1.0 / length;
                    Console.WriteLine("{0} == {1}, {2}", arr1[i], arr2[i], equalityCoefficient);
                }

            Assert.IsTrue(equalityCoefficient < 0.01);
        }

        /// <summary>
        /// Two arrays created with different seeds should not be equal number-by-number more than 0.1 %.
        /// </summary>
        [TestMethod]
        [DataRow(0, 1)]
        [DataRow(164231, 64135)]
        [DataRow(-1650, 41635)]
        [DataRow(-46213, -0)]
        [DataRow(0, int.MaxValue)]
        [DataRow(int.MaxValue, int.MaxValue - 1)]
        [DataRow(int.MinValue, int.MaxValue)]
        [DataRow(int.MaxValue, int.MinValue)]
        public void TestRandomnessWithDifferentSeeds(int seed1, int seed2)
        {
            var arr1 = new ArrayRAM(length, seed1);
            var arr2 = new ArrayRAM(length, seed2);

            var equalityCoefficient = 0.0;

            for (var i = 0; i < length; i++)
                if (Math.Abs(arr1[i] - arr2[i]) < 0.00001)
                {
                    equalityCoefficient += 1.0 / length;
                    Console.WriteLine("{0} == {1}, {2}", arr1[i], arr2[i], equalityCoefficient);
                }

            Assert.IsTrue(equalityCoefficient < 0.001);
        }
    }
}