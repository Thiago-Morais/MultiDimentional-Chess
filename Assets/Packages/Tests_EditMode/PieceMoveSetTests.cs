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
        public void HasLockedBackwards_NegativeNotInversedDimentionLocked_True(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
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
        public void HasLockedBackwards_NegativeNotInversedDimentionNotLocked_False(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
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
        public void HasLockedBackwards_NegativeInversedDimentionLocked_False(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
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
        public void HasLockedBackwards_PositiveInversedDimentionNotLocked_False(Dimentions backwards, int direction1, int direction2, int direction3, bool inversed)
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
        public void HasLockedBackwards_AnyInversionAnyVectorNoneDimentionLocked_False(
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
        #region -------- IS RAY BLOCKED
        [Test]
        public void IsRayBlocked_RayBlockedIsDeactivated_ReturnFalse()
        {
            //SETUP
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            Piece piece = new GameObject(nameof(piece)).AddComponent<Piece>();
            BoardPiece boardPiece = new GameObject(nameof(boardPiece)).AddComponent<BoardPiece>();

            moveSet.rayBlocked = Dimentions.none;
            //ACT
            bool isBlocked = moveSet.IsRayBlocked(piece, boardPiece);
            //ASSERT
            Assert.False(isBlocked);
        }
        [TestCase(Dimentions.one, 1, 1, 0)]
        [TestCase(Dimentions.one, 1, 1, 1)]
        [TestCase(Dimentions.two, 1, 0, 0)]
        [TestCase(Dimentions.two, 1, 1, 1)]
        [TestCase(Dimentions.three, 1, 0, 0)]
        [TestCase(Dimentions.three, 1, 1, 0)]
        [TestCase(Dimentions.one | Dimentions.two, 1, 1, 1)]
        [TestCase(Dimentions.one | Dimentions.three, 1, 1, 0)]
        [TestCase(Dimentions.two | Dimentions.three, 1, 0, 0)]
        [Test]
        public void IsRayBlocked_RayBlockedDontHaveDirRank_ReturnFalse(Dimentions rayBlocked, int targetCoordX, int targetCoordY, int targetCoordZ)
        {
            //SETUP
            Vector3Int targetCoord = new Vector3Int(targetCoordX, targetCoordY, targetCoordZ);
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            Piece piece = new GameObject(nameof(piece)).AddComponent<Piece>();
            BoardPiece boardPiece = new GameObject(nameof(boardPiece)).AddComponent<BoardPiece>();

            moveSet.rayBlocked = rayBlocked;
            boardPiece.BoardCoord = targetCoord;
            //ACT
            bool isBlocked = moveSet.IsRayBlocked(piece, boardPiece);
            //ASSERT
            Assert.False(isBlocked);
        }
        [TestCase(Dimentions.one, 4, 2, 2)]
        [TestCase(Dimentions.one, -4, 2, 2)]
        [TestCase(Dimentions.one, 2, 4, 2)]
        [TestCase(Dimentions.one, 2, 2, 4)]
        [TestCase(Dimentions.two, 4, 4, 2)]
        [TestCase(Dimentions.three, 4, 4, 4)]
        [Test]
        public void IsRayBlocked_RayBlockedHasDirRankAndDontHavePieceBetweenTarget_ReturnFalse(//TODO doing
            Dimentions rayBlocked,
            int targetCoordX,
            int targetCoordY,
            int targetCoordZ)
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>().Initialized() as DinamicBoard;
            dinamicBoard.SignOn();
            dinamicBoard.size = new Vector3Int(5, 5, 5);
            dinamicBoard.TryUpdateBoard();

            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            moveSet.rayBlocked = rayBlocked;
            Piece piece = new GameObject(nameof(piece)).AddComponent<Piece>();
            piece.BoardCoord = new Vector3Int(2, 2, 2);

            BoardPiece targetBoardPiece = dinamicBoard.GetSquareAt(targetCoordX, targetCoordY, targetCoordZ);
            //ACT
            bool isBlocked = moveSet.IsRayBlocked(piece, targetBoardPiece);
            //ASSERT
            Assert.False(isBlocked);
        }
        [Test]
        public void IsRayBlocked_RayBlockedHasDirRankAndHasPieceBetweenTarget_ReturnTrue(int targetCoordX, int targetCoordY, int targetCoordZ)
        {
            //SETUP
            Vector3Int targetCoord = new Vector3Int(targetCoordX, targetCoordY, targetCoordZ);
            Piece piece = new GameObject(nameof(piece)).AddComponent<Piece>();
            //ACT
            //ASSERT
            Assert.True(false);
        }
        #endregion //IS RAY BLOCKED
        #region -------- IS WITHIN MAX DIMENTIONS AMOUNT
        [TestCase(0, 0, 0)]
        [Test]
        public void IsWithinMaxDimentionsAmount_NoDimentions_True(int directionX, int directionY, int directionZ)
        {
            //SETUP
            //ACT
            //ASSERT
            Assert.True(false);
        }
        [TestCase(1, 0, 0)]
        [TestCase(0, 1, 0)]
        [TestCase(0, 0, 1)]
        [Test]
        public void IsWithinMaxDimentionsAmount_OneDimentionsWithLimitNotNone_True([Values] Dimentions amount, int directionX, int directionY, int directionZ)
        {
            //SETUP
            //ACT
            //ASSERT
            Assert.True(false);
        }
        [TestCase(1, 0, 0)]
        [TestCase(0, 1, 0)]
        [TestCase(0, 0, 1)]
        [Test]
        public void IsWithinMaxDimentionsAmount_OneDimentionsWithLimitOne_True(int directionX, int directionY, int directionZ)
        {
            //SETUP
            //ACT
            //ASSERT
            Assert.True(false);
        }
        #endregion //IS WITHIN MAX DIMENTIONS AMOUNT
    }
    #endregion //PIECE MOVE SET TESTS
}