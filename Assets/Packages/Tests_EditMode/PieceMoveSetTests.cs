using NUnit.Framework;
using UnityEngine;

namespace Tests_EditMode
{
    #region -------- PIECE MOVE SET TESTS
    public class PieceMoveSetTests
    {
        #region -------- IS OWN POSITION TESTS
        [Test]
        public void IsOwnPosition_0x0x0_True()
        {
            //ACT
            bool isOwnPosition = PieceMoveSet.IsOwnPosition(new Vector3Int(0, 0, 0));
            //ASSERT
            Assert.IsTrue(isOwnPosition);
        }
        [Test]
        public void IsOwnPosition_1x0x0_False()
        {
            //ACT
            bool isOwnPosition = PieceMoveSet.IsOwnPosition(new Vector3Int(1, 0, 0));
            //ASSERT
            Assert.IsFalse(isOwnPosition);
        }
        #endregion //IS OWN POSITION TESTS
        #region -------- IS DIMENTION BLOCKED TESTS
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
        public void HasLockedDirection(Dimentions dimentions, int direction1, int direction2, int direction3, bool expected)
        {
            //SETUP
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.lockedDimentions = dimentions;
            //ACT
            bool hasBlockedDirection = moveSet.HasLockedDirection(new Vector3Int(direction1, direction2, direction3));
            //ASSERT
            Assert.AreEqual(expected, hasBlockedDirection);
        }
        #endregion //IS DIMENTION BLOCKED TESTS
        #region -------- IS BACKWARDS BLOCKED TESTS
        [Test]
        [TestCase(Dimentions.none, 0, 0, 0, false)]
        [TestCase(Dimentions.all, 1, 0, 0, false)]
        [TestCase(Dimentions.one, 1, 1, 1, false)]
        [TestCase(Dimentions.two, 0, 1, 0, false)]
        public void HasLockedBackwards_NonNegativeNotInversed_False(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
        {
            //SETUP
            Vector3Int direction = new Vector3Int(direction1, direction2, direction3);
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.lockedBackwards = backwards;
            //ACT
            bool hasBlockedBackwards = moveSet.HasLockedBackwards(direction, inversed);
            //ASSERT
            Assert.False(hasBlockedBackwards);
        }
        [Test]
        [TestCase(Dimentions.one, -1, 0, 0, false)]
        [TestCase(Dimentions.two, -1, 0, -1, false)]
        [TestCase(Dimentions.one | Dimentions.three, -1, -1, -1, false)]
        [TestCase(Dimentions.all, 0, 0, -1, false)]
        public void HasLockedBackwards_NegativeNotInversedDimentionBlocked_True(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
        {
            //SETUP
            Vector3Int direction = new Vector3Int(direction1, direction2, direction3);
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.lockedBackwards = backwards;
            //ACT
            bool hasBlockedBackwards = moveSet.HasLockedBackwards(direction, inversed);
            //ASSERT
            Assert.True(hasBlockedBackwards);
        }
        [Test]
        [TestCase(Dimentions.one, 0, 0, -1, false)]
        [TestCase(Dimentions.two, -1, -1, 0, false)]
        [TestCase(Dimentions.one | Dimentions.three, 0, 0, -1, false)]
        public void HasLockedBackwards_NegativeNotInversedDimentionNotBlocked_False(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
        {
            //SETUP
            Vector3Int direction = new Vector3Int(direction1, direction2, direction3);
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.lockedBackwards = backwards;
            //ACT
            bool hasBlockedBackwards = moveSet.HasLockedBackwards(direction, inversed);
            //ASSERT
            Assert.False(hasBlockedBackwards);
        }
        [Test]
        [TestCase(Dimentions.one, -1, 0, 0, true)]
        [TestCase(Dimentions.two, -1, 0, -1, true)]
        [TestCase(Dimentions.one | Dimentions.three, -1, -1, -1, true)]
        [TestCase(Dimentions.all, 0, 0, -1, true)]
        public void HasLockedBackwards_NegativeInversedDimentionBlocked_False(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
        {
            //SETUP
            Vector3Int direction = new Vector3Int(direction1, direction2, direction3);
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.lockedBackwards = backwards;
            //ACT
            bool hasBlockedBackwards = moveSet.HasLockedBackwards(direction, inversed);
            //ASSERT
            Assert.False(hasBlockedBackwards);
        }
        [Test]
        [TestCase(Dimentions.none, 1, 0, 0, true)]
        [TestCase(Dimentions.one, 0, 1, 0, true)]
        [TestCase(Dimentions.two, 1, 1, 0, true)]
        [TestCase(Dimentions.one | Dimentions.three, 0, 0, 1, true)]
        public void HasLockedBackwards_PositiveInversedDimentionNotBlocked_False(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
        {
            //SETUP
            Vector3Int direction = new Vector3Int(direction1, direction2, direction3);
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.lockedBackwards = backwards;
            //ACT
            bool hasBlockedBackwards = moveSet.HasLockedBackwards(direction, inversed);
            //ASSERT
            Assert.False(hasBlockedBackwards);
        }
        [Test]
        public void HasLockedBackwards_AnyInversionAnyVectorNoneDimentionBlocked_False(
            [Values] bool inversed,
            [Values(-1, 0, 1)] int direction1,
            [Values(-1, 0, 1)] int direction2,
            [Values(-1, 0, 1)] int direction3)
        {
            //SETUP
            Vector3Int direction = new Vector3Int(direction1, direction2, direction3);
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.lockedBackwards = Dimentions.none;
            //ACT
            bool hasBlockedBackwards = moveSet.HasLockedBackwards(direction, inversed);
            //ASSERT
            Assert.False(hasBlockedBackwards);
        }
        #endregion //IS BACKWARDS BLOCKED TESTS
    }
    #endregion //PIECE MOVE SET TESTS
}