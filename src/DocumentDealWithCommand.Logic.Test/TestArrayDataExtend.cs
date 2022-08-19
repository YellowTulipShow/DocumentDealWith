using Microsoft.VisualStudio.TestTools.UnitTesting;

using DocumentDealWithCommand.Logic.Implementation;
using System.Collections.Generic;

namespace DocumentDealWithCommand.Logic.Test
{
    [TestClass]
    public class TestArrayDataExtend
    {
        [TestInitialize]
        public void Init() { }

        [TestCleanup]
        public void Clean() { }

        [TestMethod]
        public void Test_UserInputToProgramTargetIndex()
        {
            static void Test(int totalListCount, uint userInputIndex, uint programTargetIndex)
            {
                uint r = ArrayDataExtend.UserInputToProgramTargetIndex(userInputIndex, totalListCount);
                Assert.AreEqual(r, programTargetIndex);
            }
            Test(8, 0, 0);
            Test(8, 1, 0);
            Test(8, 2, 1);
            Test(8, 3, 2);
            Test(8, 4, 3);
            Test(8, 5, 4);
            Test(8, 6, 5);
            Test(8, 7, 6);
            Test(8, 8, 7);
            Test(8, 9, 7);
            Test(8, 10, 7);
        }

        [TestMethod]
        public void Test_ConcatCanNull()
        {
            int[] arr1 = new int[] { 1, 2, 3 };
            int[] arr2 = new int[] { 3, 4, 5 };
            IList<int> arr3 = arr1.ConcatCanNull(arr2);

            Assert.AreEqual("1,2,3,3,4,5", string.Join(",", arr3));
            Assert.AreEqual("1,2,3", string.Join(",", arr1));
            Assert.AreEqual("3,4,5", string.Join(",", arr2));

            arr3 = arr1.ConcatCanNull(null);
            Assert.AreEqual("1,2,3", string.Join(",", arr3));
            Assert.AreEqual("1,2,3", string.Join(",", arr1));
            Assert.AreEqual("3,4,5", string.Join(",", arr2));

            arr3 = ((int[])null).ConcatCanNull(arr2);
            Assert.AreEqual("3,4,5", string.Join(",", arr3));
            Assert.AreEqual("1,2,3", string.Join(",", arr1));
            Assert.AreEqual("3,4,5", string.Join(",", arr2));

            arr3 = ((int[])null).ConcatCanNull(null);
            Assert.AreEqual("", string.Join(",", arr3));
            Assert.AreEqual("1,2,3", string.Join(",", arr1));
            Assert.AreEqual("3,4,5", string.Join(",", arr2));
        }
    }
}
