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
        static object[] DirectionRankIsNotBlocked =
        {
            new object[] { Dimentions.one, new Vector3Int(1, 1, 0) },
            new object[] { Dimentions.one, new Vector3Int(1, -1, 0) },
            new object[] { Dimentions.one, new Vector3Int(-1, -1, 0) },
            new object[] { Dimentions.one, new Vector3Int(1, 1, 1) },
            new object[] { Dimentions.two, new Vector3Int(1, 0, 0) },
            new object[] { Dimentions.two, new Vector3Int(1, 1, 1) },
            new object[] { Dimentions.three, new Vector3Int(1, 0, 0) },
            new object[] { Dimentions.three, new Vector3Int(1, 1, 0) },
            new object[] { Dimentions.one | Dimentions.two, new Vector3Int(1, 1, 1) },
            new object[] { Dimentions.one | Dimentions.three, new Vector3Int(1, 1, 0) },
            new object[] { Dimentions.two | Dimentions.three, new Vector3Int(1, 0, 0) },
        };
        [TestCaseSource(nameof(DirectionRankIsNotBlocked))]
        [Test]
        public void IsRayBlocked_DirectionRankIsNotBlocked_ReturnFalse(Dimentions rayBlocked, Vector3Int targetCoord)
        {
            //SETUP
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
        static object[] DirectionRankIsBlocked =
        {
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(2, 0, 0), new Vector3Int(1, 0, 0), Dimentions.one, },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(0, -2, 0), new Vector3Int(0, -1, 0), Dimentions.one, },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(2, 2, 0), new Vector3Int(1, 1, 0), Dimentions.two, },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(2, 0, -2), new Vector3Int(1, 0, -1), Dimentions.two, },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(0, -2, -2), new Vector3Int(0, -1, -1), Dimentions.two, },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(3, 3, 3), new Vector3Int(1, 1, 1), Dimentions.three, },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(3, 3, -3), new Vector3Int(1, 1, -1), Dimentions.three, },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(-3, -3, -3), new Vector3Int(-1, -1, -1), Dimentions.three, },
        };
        [TestCaseSource(nameof(DirectionRankIsBlocked))]
        [Test]
        public void IsRayBlocked_DirectionRankIsBlockedAndHavePieceBetweenTarget_ReturnTrue(
            Vector3Int player,
            Vector3Int playerOffset,
            Vector3Int enemyOffset,
            Dimentions rayBlocked)
        {
            //SETUP
            PieceMoveSet moveSet = ScriptableObject.CreateInstance<PieceMoveSet>();
            Piece piece = new GameObject(nameof(piece)).AddComponent<Piece>();
            Piece enemy = new GameObject(nameof(piece)).AddComponent<Piece>();
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>().Initialized() as DinamicBoard;

            dinamicBoard.size = new Vector3Int(10, 10, 10);
            dinamicBoard.TryUpdateBoard();

            moveSet.rayBlocked = rayBlocked;
            piece.BoardCoord = player;
            enemy.MoveTo(dinamicBoard.GetSquareAt(player + enemyOffset));

            BoardPiece target = dinamicBoard.GetSquareAt(player + playerOffset);
            //ACT
            bool isBlocked = moveSet.IsRayBlocked(piece, target);
            //ASSERT
            Assert.True(isBlocked);
        }
        // Se não tem nenhuma peça entre player e a coordenada
        [TestCase(Dimentions.one, 4, 2, 2)]
        [Test]
        public void IsRayBlocked_DirectionRankIsBlockedAndDontPieceBetweenTarget_ReturnFalse(//TODO doing
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