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
        [Test]
        [TestCase(Dimentions.none, 0, 0, 0, false)]
        [TestCase(Dimentions.all, 1, 0, 0, false)]
        [TestCase(Dimentions.one, 1, 1, 1, false)]
        [TestCase(Dimentions.two, 0, 1, 0, false)]
        public void IsBackwardsBlocked_NonNegativeNotInversed_False(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
        {
            //SETUP
            Vector3Int direction = new Vector3Int(direction1, direction2, direction3);
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.backwardsBlocker = backwards;
            //ACT
            bool b = moveSet.IsBackwardsBlocked(direction, inversed);
            //ASSERT
            Assert.False(b);
        }
        [Test]
        [TestCase(Dimentions.one, -1, 0, 0, false)]
        [TestCase(Dimentions.two, -1, 0, -1, false)]
        [TestCase(Dimentions.one | Dimentions.three, -1, -1, -1, false)]
        [TestCase(Dimentions.all, 0, 0, -1, false)]
        public void IsBackwardsBlocked_NegativeNotInversedDimentionBlocked_True(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
        {
            //SETUP
            Vector3Int direction = new Vector3Int(direction1, direction2, direction3);
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.backwardsBlocker = backwards;
            //ACT
            bool b = moveSet.IsBackwardsBlocked(direction, inversed);
            //ASSERT
            Assert.True(b);
        }
        [Test]
        [TestCase(Dimentions.one, 0, 0, -1, false)]
        [TestCase(Dimentions.two, -1, -1, 0, false)]
        [TestCase(Dimentions.one | Dimentions.three, 0, 0, -1, false)]
        public void IsBackwardsBlocked_NegativeNotInversedDimentionNotBlocked_False(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
        {
            //SETUP
            Vector3Int direction = new Vector3Int(direction1, direction2, direction3);
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.backwardsBlocker = backwards;
            //ACT
            bool b = moveSet.IsBackwardsBlocked(direction, inversed);
            //ASSERT
            Assert.False(b);
        }
        [Test]
        [TestCase(Dimentions.one, -1, 0, 0, true)]
        [TestCase(Dimentions.two, -1, 0, -1, true)]
        [TestCase(Dimentions.one | Dimentions.three, -1, -1, -1, true)]
        [TestCase(Dimentions.all, 0, 0, -1, true)]
        public void IsBackwardsBlocked_NegativeInversedDimentionBlocked_False(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
        {
            //SETUP
            Vector3Int direction = new Vector3Int(direction1, direction2, direction3);
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.backwardsBlocker = backwards;
            //ACT
            bool b = moveSet.IsBackwardsBlocked(direction, inversed);
            //ASSERT
            Assert.False(b);
        }
        [Test]
        [TestCase(Dimentions.none, 1, 0, 0, true)]
        [TestCase(Dimentions.one, 0, 1, 0, true)]
        [TestCase(Dimentions.two, 1, 1, 0, true)]
        [TestCase(Dimentions.one | Dimentions.three, 0, 0, 1, true)]
        public void IsBackwardsBlocked_PositiveInversedDimentionNotBlocked_False(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
        {
            //SETUP
            Vector3Int direction = new Vector3Int(direction1, direction2, direction3);
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.backwardsBlocker = backwards;
            //ACT
            bool b = moveSet.IsBackwardsBlocked(direction, inversed);
            //ASSERT
            Assert.False(b);
        }
        [Test]
        public void IsBackwardsBlocked_AnyInversionAnyVectorNoneDimentionBlocked_False(
            [Values] bool inversed,
            [Values(-1, 0, 1)] int direction1,
            [Values(-1, 0, 1)] int direction2,
            [Values(-1, 0, 1)] int direction3)
        {
            //SETUP
            Vector3Int direction = new Vector3Int(direction1, direction2, direction3);
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.backwardsBlocker = Dimentions.none;
            //ACT
            bool b = moveSet.IsBackwardsBlocked(direction, inversed);
            //ASSERT
            Assert.False(b);
        }
    }
    #endregion //PIECE MOVE SET TESTS
}
