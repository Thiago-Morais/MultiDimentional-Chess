using ExtensionMethods;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    #region -------- PIECE MOVE SET TESTS
    public class PieceMoveSetTests
    {
        [Test]
        public void IsOwnPosition_0x0x0_True()
        {
            //ACT
            bool b = PieceMoveSet.IsOwnPosition(new Vector3Int(0, 0, 0));
            //ASSERT
            Assert.IsTrue(b);
        }
        [Test]
        public void IsOwnPosition_1x0x0_False()
        {
            //ACT
            bool b = PieceMoveSet.IsOwnPosition(new Vector3Int(1, 0, 0));
            //ASSERT
            Assert.IsFalse(b);
        }
        [Test]
        [TestCase(Dimentions.none, 0, 0, 0, false)]
        [TestCase(Dimentions.none, 1, 1, 1, false)]
        [TestCase(Dimentions.one, 1, 0, 0, true)]
        [TestCase(Dimentions.one, -1, 0, 0, true)]
        [TestCase(Dimentions.one, 0, 1, 1, false)]
        [TestCase(Dimentions.two, 0, 0, 1, true)]
        [TestCase(Dimentions.two, 0, 0, -1, true)]
        [TestCase(Dimentions.two, 1, 1, 0, false)]
        [TestCase(Dimentions.three, 0, 1, 0, true)]
        [TestCase(Dimentions.three, 0, -1, 0, true)]
        [TestCase(Dimentions.three, 1, 0, 1, false)]
        [TestCase(Dimentions.all, 1, 0, 0, true)]
        [TestCase(Dimentions.all, 0, 1, 0, true)]
        [TestCase(Dimentions.all, 0, 0, 1, true)]
        public void IsDimentionBlocked(Dimentions dimentions, int direction1, int direction2, int direction3, bool expected)
        {
            //SETUP
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.blockedDimentions = dimentions;
            //ACT
            bool b = moveSet.IsDimentionBlocked(new Vector3Int(direction1, direction2, direction3));
            //ASSERT
            Assert.AreEqual(expected, b);
        }
    }
    #endregion //PIECE MOVE SET TESTS
}
