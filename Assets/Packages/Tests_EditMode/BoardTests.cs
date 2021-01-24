using System;
using NUnit.Framework;
using UnityEngine;

namespace Tests_EditMode
{
    #region ------- BOARD_TESTS
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

        #region -------- RESET_BOARD_SIZE
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
        #endregion //RESET_BOARD_SIZE

        #region -------- INSTANTIATE_A_BOARD
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
        #endregion //INSTANTIATE_A_BOARD

        #region -------- GENERATE_SQUARE_USING
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
        #endregion //GENERATE_SQUARE_USING

        #region -------- GET_POOL
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
        #endregion //GET_POOL

        #region -------- GET_SQUARE_AT
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
        #endregion //GET_SQUARE_AT
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

        #region -------- HAS_PIECE_BETWEEN
        static object[] TargetAheadAndNoPiecesInBetween = {
            new object[] { new Vector3Int(0, 0, 0), new Vector3Int(0, 0, 2)  },
            new object[] { new Vector3Int(0, 2, 3), new Vector3Int(0, 0, 4)  } ,
            new object[] { new Vector3Int(2, 2, 1), new Vector3Int(0, 0, 1)  } };
        [TestCaseSource(nameof(TargetAheadAndNoPiecesInBetween))]
        [Test]
        public void HasPieceBetween_TargetAheadAndNoPiecesInBetween_ReturnFalse(
            Vector3Int player,
            Vector3Int playerMove)
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>().Initialized() as DinamicBoard;
            Piece piece = new GameObject(nameof(piece)).AddComponent<Piece>();

            piece.BoardCoord = player;
            Vector3Int targetCoord = piece.BoardCoord + playerMove;
            //ACT
            bool hasPieceBetween = dinamicBoard.HasPieceBetween(piece.BoardCoord, targetCoord);
            //ASSERT
            Assert.False(hasPieceBetween);
        }
        static object[] TargetAheadAndHavePiecesInBetween = {
            new object[] { new Vector3Int(0, 0, 0), new Vector3Int(0, 0, 2), new Vector3Int(0, 0, 1) },
            new object[] { new Vector3Int(0, 2, 3), new Vector3Int(0, 0, 4), new Vector3Int(0, 2, 6) } };
        [TestCaseSource(nameof(TargetAheadAndHavePiecesInBetween))]
        [Test]
        public void HasPieceBetween_TargetAheadAndHavePiecesInBetween_ReturnTrue(
            Vector3Int playerCoord,
            Vector3Int playerMove,
            Vector3Int enemyCoord)
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>().Initialized() as DinamicBoard;
            dinamicBoard.TryUpdateBoard(new Vector3Int(10, 10, 10), Vector3.zero);

            Piece player = new GameObject(nameof(player)).AddComponent<Piece>();
            player.BoardCoord = playerCoord;

            Piece enemy = new GameObject(nameof(enemy)).AddComponent<Piece>();
            enemy.MoveTo(dinamicBoard.GetSquareAt(enemyCoord));
            //ACT
            Vector3Int playerTarget = playerCoord + playerMove;
            bool hasPieceBetween = dinamicBoard.HasPieceBetween(player.BoardCoord, playerTarget);
            //ASSERT
            Assert.True(hasPieceBetween);
        }
        static object[] TargetBindedAndHavePiecesInBetween =
        {
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(3, 3, 0), new Vector3Int(5+1, 5+1, 5) },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(0, -5, -5), new Vector3Int(5, 5-1, 5-1) },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(4, -4, 4), new Vector3Int(5+3, 5-3, 5+3) },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(2, 2, 2), new Vector3Int(5+1, 5+1, 5+1) },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(-2, -2, -2), new Vector3Int(5-1, 5-1, 5-1) },
        };
        [TestCaseSource(nameof(TargetBindedAndHavePiecesInBetween))]
        [Test]
        public void HasPieceBetween_TargetBindedAndHavePiecesInBetween_ReturnTrue(
            Vector3Int playerCoord,
            Vector3Int playerMove,
            Vector3Int enemyCoord)
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>().Initialized() as DinamicBoard;
            Piece player = new GameObject(nameof(player)).AddComponent<Piece>();
            Piece enemy = new GameObject(nameof(enemy)).AddComponent<Piece>();


            dinamicBoard.TryUpdateBoard(new Vector3Int(10, 10, 10), Vector3.zero);
            player.BoardCoord = playerCoord;
            enemy.MoveTo(dinamicBoard.GetSquareAt(enemyCoord));
            //ACT
            Vector3Int playerTarget = playerCoord + playerMove;
            bool hasPieceBetween = dinamicBoard.HasPieceBetween(player.BoardCoord, playerTarget);
            //ASSERT
            Assert.True(hasPieceBetween);
        }
        static object[] TargetBindedAndNoPiecesInBetween =
        {
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(3, 3, 0), },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(0, -5, -5), },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(4, -4, 4), },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(2, 2, 2), },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(-2, -2, -2), },
        };
        [TestCaseSource(nameof(TargetBindedAndNoPiecesInBetween))]
        [Test]
        public void HasPieceBetween_TargetBindedAndNoPiecesInBetween_ReturnFalse(
            Vector3Int playerCoord,
            Vector3Int playerMove)
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>().Initialized() as DinamicBoard;
            Piece player = new GameObject(nameof(player)).AddComponent<Piece>();

            dinamicBoard.TryUpdateBoard(new Vector3Int(10, 10, 10), Vector3.zero);
            player.BoardCoord = playerCoord;
            //ACT
            Vector3Int playerTarget = playerCoord + playerMove;
            bool hasPieceBetween = dinamicBoard.HasPieceBetween(player.BoardCoord, playerTarget);
            //ASSERT
            Assert.False(hasPieceBetween);
        }
        #endregion //HAS_PIECE_BETWEEN

        #region -------- UPDATE_BOARD_PIECES
        static object[] BoardSizeAndPiece =
        {
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(0, 0, 0), },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(1, 0, 0), },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(1, 1, 0), },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(0, 2, 2), },
            new object[] { new Vector3Int(5, 5, 5), new Vector3Int(4, 4, 4), },
        };
        [TestCaseSource(nameof(BoardSizeAndPiece))]
        [Test]
        public void UpdateBoardPieces__PiecesWithBoardCoordEqualToBoard(Vector3Int boardSize, Vector3Int pieceCoord)
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>().Initialized() as DinamicBoard;
            dinamicBoard.ResetBoardSize(boardSize);
            BoardPiece[,,] board = dinamicBoard.board;
            //ACT
            dinamicBoard.UpdateBoardPieces();
            //ASSERT
            Assert.AreEqual(pieceCoord, board[pieceCoord.x, pieceCoord.y, pieceCoord.z].BoardCoord);
        }
        [Test]
        public void UpdateBoardPieces__SetBoardPiecesBoardReferenceAsDinamicBoard(
            [Values(1, 2)] int x,
            [Values(1, 2, 3)] int y,
            [Values(1, 4, 5)] int z)
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>().Initialized() as DinamicBoard;
            dinamicBoard.ResetBoardSize(new Vector3Int(x, y, z));
            BoardPiece[,,] board = dinamicBoard.board;
            //ACT
            dinamicBoard.UpdateBoardPieces();
            //ASSERT
            foreach (BoardPiece square in dinamicBoard.board)
                Assert.AreEqual(dinamicBoard, square.board);
        }
        #endregion //UPDATE_BOARD_PIECES

        #region -------- UPDATE BOARD COORD
        [TestCaseSource(nameof(BoardSizeAndPiece))]
        [Test]
        public void UpdateBoardCoord__PiecesWithBoardCoordEqualToBoard(Vector3Int boardSize, Vector3Int pieceCoord)
        {
            //SETUP
            DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>().Initialized() as DinamicBoard;
            dinamicBoard.ResetBoardSize(boardSize);
            BoardPiece[,,] board = dinamicBoard.board;
            //ACT
            dinamicBoard.UpdateBoardCoord();
            //ASSERT
            Assert.AreEqual(pieceCoord, board[pieceCoord.x, pieceCoord.y, pieceCoord.z].BoardCoord);
        }
        #endregion //UPDATE BOARD COORD
    }
    #endregion //BOARD_TESTS
}
