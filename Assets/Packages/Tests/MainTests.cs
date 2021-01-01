using NUnit.Framework;
using ExtensionMethods;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    #region -------- EXTENSIONS TESTS
    #region -------- LISTEXTENSION TESTS
    public class ListExtensionTests
    {
        [Test]
        public void RemoveMatchingElements_IntZeroThroughTenAndIntEvenThroughTen_ZeroToTenBecomesOddNumbers()
        {
            //SETUP
            List<int> zeroToTen = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int> evenToTen = new List<int>() { 0, 2, 4, 6, 8, 10 };
            //ACT
            ListExtension.RemoveMatchingElements<int>(zeroToTen, evenToTen);
            //ASSERT
            Assert.AreEqual(new List<int>(0) { 1, 3, 5, 7, 9 }, zeroToTen);
        }
        [Test]
        public void RemoveMatchingElements_IntZeroThroughTenAndIntEvenThroughTen_EvenNumbersBecomesEmpty()
        {
            //SETUP
            List<int> zeroToTen = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int> evenToTen = new List<int>() { 0, 2, 4, 6, 8, 10 };
            //ACT
            ListExtension.RemoveMatchingElements<int>(zeroToTen, evenToTen);
            //ASSERT
            Assert.IsEmpty(evenToTen);
        }
        [Test]
        public void IsEmpty_IntListWithOneElement_False()
        {
            //SETUP
            List<int> list = new List<int>() { 0 };
            //ACT
            bool o = list.IsEmpty();
            //ASSERT
            Assert.IsFalse(o);
        }
        [Test]
        public void IsEmpty_IntListWithZeroElement_True()
        {
            //SETUP
            List<int> list = new List<int>() { };
            //ACT
            bool o = list.IsEmpty();
            //ASSERT
            Assert.IsTrue(o);
        }
        [Test]
        public void IsEmpty_NulledIntList_True()
        {
            //SETUP
            List<int> list = default(List<int>);
            //ACT
            bool o = list.IsEmpty();
            //ASSERT
            Assert.IsTrue(o);
        }
        [Test]
        public void Peek_NulledIntList_NullReferenceException()
        {
            //SETUP
            List<int> list = default(List<int>);
            //ACT
            TestDelegate peek = () => list.Peek();
            //ASSERT
            Assert.Throws<System.InvalidOperationException>(peek);
        }
        [Test]
        public void Peek_EmptyIntList_ThrowsException()
        {
            //SETUP
            List<int> list = new List<int>();
            //ACT
            TestDelegate peek = () => list.Peek();
            //ASSERT
            Assert.Throws<System.InvalidOperationException>(peek);
        }
        [Test]
        public void Peek_IntListWithOneElement_GetElementOnList()
        {
            //SETUP
            List<int> list = new List<int>() { 0 };
            //ACT
            int i = list.Peek();
            //ASSERT
            Assert.AreEqual(list[0], i);
        }
        [Test]
        public void Peek_IntListWithTwoElement_GetLastElementOnList()
        {
            //SETUP
            List<int> list = new List<int>() { 0, 1 };
            //ACT
            int i = list.Peek();
            //ASSERT
            Assert.AreEqual(list[1], i);
        }
        [Test]
        public void Push_ValueInEmptyList_ValueIsAtTheEndOfList()
        {
            //SETUP
            List<int> list = new List<int>();
            //ACT
            list.Push(0);
            //ASSERT
            Assert.AreEqual(0, list[0]);
        }
        [Test]
        public void Push_ValueInOneElementList_ValueIsAtTheEndOfList()
        {
            //SETUP
            List<int> list = new List<int>() { 0 };
            //ACT
            list.Push(1);
            //ASSERT
            Assert.AreEqual(1, list[1]);
        }
        [Test]
        public void Pop_IntListWithOneElement_GetsTheElementOnList()
        {
            //SETUP
            List<int> list = new List<int>() { 0 };
            //ACT
            int i = list.Pop();
            //ASSERT
            Assert.AreEqual(0, i);
        }
        [Test]
        public void Pop_IntListWithOneElement_ListGetsEmpty()
        {
            //SETUP
            List<int> list = new List<int>() { 0 };
            //ACT
            int i = list.Pop();
            //ASSERT
            Assert.IsEmpty(list);
        }
    }
    #endregion //LISTEXTENSION TESTS

    #region -------- VECTOREXTENSION TESTS
    public class VectorExtensionTests
    {
        [Test]
        public void AsInt_Vector3Up_VectorIntUp()
        {
            //SETUP
            //ACT
            Vector3Int vector = Vector3.up.AsInt();
            //ASSERT
            Assert.AreEqual(new Vector3Int(0, 1, 0), vector);
        }
    }
    #endregion //VECTOREXTENSION TESTS
    #endregion //EXTENSIONS TESTS

    #region ------- BOARDPIECE TESTS
    public class BoardPieceTests
    {
        [Test]
        public void TestMethod()
        {
            //ACT
            //ASSERT
        }
    }
    #endregion //BOARDPIECE TESTS
}
