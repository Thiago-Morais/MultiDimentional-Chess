using NUnit.Framework;
using UnityEngine;

namespace Tests_EditMode
{
    #region ------- BOARD TESTS
    public class BoardTests
    {
        Piece piece1;
        BoardPiece boardPiece1;
        [SetUp]
        public void Setup()
        {
            piece1 = new GameObject(nameof(piece1)).AddComponent<Piece>();
            piece1.Awake();
            boardPiece1 = new GameObject(nameof(boardPiece1)).AddComponent<BoardPiece>();
            boardPiece1.InitializeVariables();
        }
        static DinamicBoard LoadDinamicBoardPrefab()
        {
            GameObject boardPrefab = (Resources.Load("Board/Prefabs/DinamicBoard") as GameObject);
            DinamicBoard dinamicBoard = UnityEngine.Object.Instantiate(boardPrefab).GetComponent<DinamicBoard>();
            return dinamicBoard;
        }
        [TearDown]
        public void TearDown()
        {
        }
        [Test]
        public void Selected_Pawn_Display_Possible_Moves()
        {
            //SETUP
            PieceMoveSet pawnMoveSet = Resources.Load("Pieces/Scriptable/MoveSet/PawnMoveSet") as PieceMoveSet;
            piece1.moveSet = pawnMoveSet;
            Selector selector = new GameObject(nameof(selector)).AddComponent<Selector>();
            GameObject boardPrefab = (Resources.Load("Board/Prefabs/DinamicBoard") as GameObject);
            DinamicBoard dinamicBoard = UnityEngine.Object.Instantiate(boardPrefab).GetComponent<DinamicBoard>();
            dinamicBoard.Awake();
            //ACT
            selector.ChangeSelection(piece1);
            //ASSERT
            foreach (BoardPiece square in dinamicBoard.board)
                if (piece1.IsAnyMovimentAvailable(square) || piece1.BoardCoord == square.BoardCoord)
                    Assert.IsTrue(square.Highlight.IsHighlighted);
                else
                    Assert.IsFalse(square.Highlight.IsHighlighted);

        }
        [Test]
        public void ResetBoardSize_1x1x1_BoardSizeIs1()
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>();
            dinamicBoard.InitializeVariables();
            //ACT
            dinamicBoard.ResetBoardSize(new Vector3Int(1, 1, 1));
            //ASSERT
            Assert.AreEqual(1, dinamicBoard.board.Length);
        }
        [Test]
        public void ResetBoardSize_2x2x2_BoardSizeIs8()
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>();
            dinamicBoard.InitializeVariables();
            //ACT
            dinamicBoard.ResetBoardSize(new Vector3Int(2, 2, 2));
            //ASSERT
            Assert.AreEqual(8, dinamicBoard.board.Length);
        }
        [Test]
        public void InstantiateABoard_3x3x3_Empty3x3x3Board()
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>();
            //ACT
            BoardPiece[,,] board = DinamicBoard.InstantiateABoard(new Vector3Int(3, 3, 3));
            //ASSERT
            Assert.AreEqual(new BoardPiece[3, 3, 3], board);
        }
        [Test]
        public void InstantiateABoard_NegativeValue_EmptyBoardWithZeroOnTheNegative()
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>();
            //ACT
            BoardPiece[,,] board = DinamicBoard.InstantiateABoard(new Vector3Int(-1, 0, 1));
            //ASSERT
            Assert.AreEqual(new BoardPiece[0, 0, 1], board);
        }
        [Test]
        public void GenerateSquareUsing_PairIndex_WhiteBoardPiece()
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>();
            dinamicBoard.InitializeVariables();
            //ACT
            BoardPiece boardPiece = dinamicBoard.GenerateBoardPieceUsing(0);
            //ASSERT
            Assert.AreSame(((BoardPiece)dinamicBoard.WhitePool.sample).so_pieceData, boardPiece.so_pieceData);
        }
        [Test]
        public void GenerateSquareUsing_OddIndex_PieceDataIsTheSameAs()
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>();
            dinamicBoard.InitializeVariables();
            //ACT
            BoardPiece boardPiece = dinamicBoard.GenerateBoardPieceUsing(1);
            //ASSERT
            Assert.AreSame(((BoardPiece)dinamicBoard.BlackPool.sample).so_pieceData, boardPiece.so_pieceData);
        }
        [Test]
        public void GetPool_PairIndex_WhitePool()
        {
            //SETUP
            GameObject boardPrefab = (Resources.Load("Board/Prefabs/DinamicBoard") as GameObject);
            DinamicBoard dinamicBoard = UnityEngine.Object.Instantiate(boardPrefab).GetComponent<DinamicBoard>();
            dinamicBoard.Awake();
            //ACT
            Pool pool = dinamicBoard.GetPool(0);
            //ASSERT
            Assert.AreSame(dinamicBoard.WhitePool, pool);
        }
        [Test]
        public void GetPool_OddIndex_BlackPool()
        {
            //SETUP
            DinamicBoard dinamicBoard = LoadDinamicBoardPrefab();
            dinamicBoard.Awake();
            //ACT
            Pool pool = dinamicBoard.GetPool(1);
            //ASSERT
            Assert.AreSame(dinamicBoard.BlackPool, pool);
        }
        [Test]
        public void GetSquareAt_OutOfBounds_Null()
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>();
            dinamicBoard.board = DinamicBoard.InstantiateABoard(new Vector3Int(3, 3, 3));
            //ACT
            BoardPiece boardPiece = dinamicBoard.GetSquareAt(new Vector3Int(2, 2, 3));
            //ASSERT
            Assert.IsNull(boardPiece);
        }
        [Test]
        public void GetSquareAt_InBounds_ValueAtCoordenate()
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>();
            dinamicBoard.board = DinamicBoard.InstantiateABoard(new Vector3Int(3, 3, 3));
            //ACT
            BoardPiece boardPiece = dinamicBoard.GetSquareAt(new Vector3Int(2, 2, 2));
            //ASSERT
            Assert.AreSame(dinamicBoard.board[2, 2, 2], boardPiece);
        }
        [Test]
        public void UpdateBoardPieceCoordAt_0x0x0_BoardPieceCoord0x0x0()
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>();
            dinamicBoard.InitializeVariables();
            dinamicBoard.board = dinamicBoard.GenerateBoard(new Vector3Int(1, 1, 1));
            //ACT
            dinamicBoard.UpdateBoardPieceCoordAt(0, 0, 0);
            //ASSERT
            Assert.AreEqual(new Vector3Int(0, 0, 0), dinamicBoard.board[0, 0, 0].BoardCoord);
        }
        [Test]
        public void GenerateBoard_1x1x1_PieceIsNotNull()
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>();
            dinamicBoard.InitializeVariables();
            //ACT
            dinamicBoard.board = dinamicBoard.GenerateBoard(new Vector3Int(1, 1, 1));
            //ASSERT
            Assert.NotNull(dinamicBoard.board[0, 0, 0]);
        }
        [Test]
        public void InitializeVariables_VariablesAreNotNull()
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>();
            //ACT
            dinamicBoard.InitializeVariables();
            //ASSERT
            Assert.NotNull(dinamicBoard.size);
            Assert.NotNull(dinamicBoard.padding);
            Assert.NotNull(dinamicBoard.center);
            Assert.NotNull(dinamicBoard.board);
            Assert.NotNull(dinamicBoard.WhitePool);
            Assert.NotNull(dinamicBoard.BlackPool);
        }
    }
    #endregion //BOARD TESTS
}
