using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    #region ------- PIECE TESTS
    #region ------- PIECEMOVIMENT TESTS
    public class PieceTests
    {
        public class PieceMovimentTests
        {
            Piece piece1;
            Piece piece2;
            BoardPiece boardPiece1;
            BoardPiece boardPiece2;
            [SetUp]
            public void Setup()
            {
                piece1 = new GameObject(nameof(piece1)).AddComponent<Piece>();
                piece1.Awake();
                piece2 = new GameObject(nameof(piece2)).AddComponent<Piece>();
                piece2.Awake();
                boardPiece1 = new GameObject(nameof(boardPiece1)).AddComponent<BoardPiece>();
                boardPiece2 = new GameObject(nameof(boardPiece2)).AddComponent<BoardPiece>();
            }
            [TearDown]
            public void TearDown()
            {
                Object.DestroyImmediate(piece1.gameObject);
                Object.DestroyImmediate(piece2.gameObject);
                Object.DestroyImmediate(boardPiece1.gameObject);
                Object.DestroyImmediate(boardPiece2.gameObject);
            }
            [Test]
            public void Pawn_Can_Move_To_The_Square_In_Front_Of_It()
            {
                //SETUP
                PieceMoveSet pawnMoveSet = Resources.Load("Pieces/Scriptable/MoveSet/PawnMoveSet") as PieceMoveSet;
                piece1.moveSet = pawnMoveSet;
                GameObject boardPrefab = (Resources.Load("Board/Prefabs/DinamicBoard") as GameObject);
                DinamicBoard dinamicBoard = UnityEngine.Object.Instantiate(boardPrefab).GetComponent<DinamicBoard>();
                dinamicBoard.Awake();
                BoardPiece boardPiece = dinamicBoard.GetSquareAt(new Vector3Int(0, 0, 0));
                piece1.MoveTo(boardPiece);
                //ACT
                bool? canMove = dinamicBoard.IsMovimentAvailable(piece1, new Vector3Int(0, 0, 1));
                //ASSERT
                Assert.IsTrue(canMove);
            }
        }
    }
    #endregion //BOARDPIECE TESTS
    #endregion //PIECE TESTS
}
