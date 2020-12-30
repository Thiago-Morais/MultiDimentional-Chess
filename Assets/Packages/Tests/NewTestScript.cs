using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using ExtensionMethods;
using UnityEngine.TestTools;

namespace Tests
{
    public class HighlightTests
    {
        Selector selector;
        Piece piece1;
        Piece piece2;
        BoardPiece boardPiece1;
        BoardPiece boardPiece2;

        [SetUp]
        public void Setup()
        {
            selector = new GameObject(nameof(selector)).AddComponent<Selector>();
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
            Object.DestroyImmediate(selector.gameObject);
            Object.DestroyImmediate(piece1.gameObject);
            Object.DestroyImmediate(piece2.gameObject);
            Object.DestroyImmediate(boardPiece1.gameObject);
            Object.DestroyImmediate(boardPiece2.gameObject);
        }
        [Test]
        public void Select_Piece_Highlight_It()
        {
            //ACT
            selector.ChangeSelection(piece1 as ISelectable);
            //ASSERT
            Assert.IsTrue(piece1.highlight.IsHighlighted);
        }
        [Test]
        public void Selected_Piece_Highlight_Type_As_Selector()
        {
            //ACT
            selector.ChangeSelection(piece1 as ISelectable);
            //ASSERT
            Assert.AreEqual(selector.HighlightType, piece1.highlight.HighlightType);
        }
        [Test]
        public void Select_Second_Piece_Disable_First_Highlight()
        {
            //ACT
            selector.ChangeSelection(piece1 as ISelectable);
            selector.ChangeSelection(piece2 as ISelectable);
            //ASSERT
            Assert.IsFalse(piece1.highlight.IsHighlighted);
        }
        [Test]
        public void Select_Second_Piece_Reset_First_Highlight_Type()
        {
            //ACT
            HighlightType cachedHighlightPiece1 = piece1.highlight.HighlightType;
            selector.ChangeSelection(piece1 as ISelectable);
            selector.ChangeSelection(piece2 as ISelectable);
            //ASSERT
            Assert.AreEqual(cachedHighlightPiece1, piece1.highlight.HighlightType);
        }
        [Test]
        public void Deselect_Piece_Turn_Highlight_Off()
        {
            //ACT
            piece1.OnSelected();
            piece1.OnDeselected();
            //ASSERT
            Assert.IsFalse(piece1.highlight.IsHighlighted);
        }
        [Test]
        public void Select_Selected_Piece_Turn_Highlight_Off()
        {
            //ACT
            selector.ChangeSelection(piece1 as ISelectable);
            selector.ChangeSelection(piece1 as ISelectable);
            //ASSERT
            Assert.IsFalse(piece1.highlight.IsHighlighted);
        }
        /*
        TODO testes a implementar
        peça selecionada mostra possibilidade de movimento
        troca de seleção desliga tabuleiro
        */
        public class HoverHighlightTests
        {
            HoverHighlight hoverHighlight;
            [SetUp]
            public void SetUp()
            {
                hoverHighlight = new GameObject(nameof(hoverHighlight)).AddComponent<HoverHighlight>();
            }
            [Test]
            public void Is_Hovered_Piece_Highlighted()
            {
                //SETUP
                Piece piece1 = new GameObject(nameof(piece1)).AddComponent<Piece>();
                piece1.Awake();
                //ACT
                hoverHighlight.HoveredIn(piece1);
                //ASSERT
                Assert.IsTrue(piece1.highlight.IsHighlighted);
            }
            [Test]
            public void Hover_In_On_Piece_Store_Highlight_Type_On_Internal_Stack()
            {
                //SETUP
                Piece piece1 = new GameObject(nameof(piece1)).AddComponent<Piece>();
                piece1.Awake();
                //ACT
                HighlightType cachedPieceHighlightType = piece1.highlight.HighlightType;
                hoverHighlight.HoveredIn(piece1);
                //ASSERT
                Assert.AreEqual(cachedPieceHighlightType, piece1.Highlight.HighlightStack.Peek());
            }
            [Test]
            public void Hover_In_And_Out_Of_Selected_Piece_Keeps_Highlight_On()
            {
                //SETUP
                Selector selector = new GameObject(nameof(selector)).AddComponent<Selector>();
                Piece piece1 = new GameObject(nameof(piece1)).AddComponent<Piece>();
                piece1.Awake();
                //ACT
                selector.ChangeSelection(piece1);
                hoverHighlight.HoveredIn(piece1);
                hoverHighlight.HoveredOut(piece1);
                //ASSERT
                Assert.IsTrue(piece1.highlight.IsHighlighted);
            }
            [Test]
            public void Hover_In_Out_Of_Piece_Reset_Highlight_Type()
            {
                //SETUP
                Piece piece1 = new GameObject(nameof(piece1)).AddComponent<Piece>();
                piece1.Awake();
                //ACT
                hoverHighlight.HoveredIn(piece1);
                hoverHighlight.HoveredOut(piece1);
                //ASSERT
                Assert.IsEmpty(piece1.highlight.HighlightStack);
            }
            [Test]
            public void Hover_In_And_Out_Of_Board_Piece_After_Deselect_Selected_Piece_Have_Highligt_Off()
            {
                //SETUP
                GameObject boardPrefab = (Resources.Load("Board/Prefabs/DinamicBoard") as GameObject);
                DinamicBoard board = UnityEngine.Object.Instantiate(boardPrefab).GetComponent<DinamicBoard>();
                board.Awake();
                Selector selector = new GameObject(nameof(selector)).AddComponent<Selector>();
                Piece piece1 = new GameObject(nameof(piece1)).AddComponent<Piece>();
                piece1.Awake();
                //ACT
                selector.ChangeSelection(piece1);
                selector.ChangeSelection(piece1);
                foreach (BoardPiece square in board.board)
                {
                    hoverHighlight.HoveredIn(square);
                    hoverHighlight.HoveredOut(square);
                }
                //ASSERT
                foreach (BoardPiece square in board.board)
                    Assert.IsFalse(square.Highlight.IsHighlighted);
            }
        }
    }
    public class SelectorTests
    {
        Piece piece1;
        Selector selector;
        [SetUp]
        public void SetUp()
        {
            selector = new GameObject(nameof(selector)).AddComponent<Selector>();
            piece1 = new GameObject(nameof(piece1)).AddComponent<Piece>();
            piece1.Awake();
        }
        [Test]
        public void Select_Selected_Piece_Deselect_It()
        {
            Selector selector = new GameObject(nameof(selector)).AddComponent<Selector>();
            //ACT
            selector.ChangeSelection(piece1 as ISelectable);
            selector.ChangeSelection(piece1 as ISelectable);
            //ASSERT
            Assert.IsNull(selector.currentSelected);
        }
        [Test]
        public void Select_Selected_Piece2_After_Selected_Piece1_Have_No_Selection()
        {
            Selector selector = new GameObject(nameof(selector)).AddComponent<Selector>();
            Piece piece1 = new GameObject(nameof(piece1)).AddComponent<Piece>();
            piece1.Awake();
            Piece piece2 = new GameObject(nameof(piece2)).AddComponent<Piece>();
            piece2.Awake();
            //ACT
            selector.ChangeSelection(piece1 as ISelectable);
            selector.ChangeSelection(piece2 as ISelectable);
            selector.ChangeSelection(piece2 as ISelectable);
            //ASSERT
            Assert.IsNull(selector.currentSelected);
        }
    }
    public class BoardTests
    {
        Piece piece1;
        BoardPiece boardPiece1;
        // DinamicBoard dinamicBoard;
        [SetUp]
        public void Setup()
        {
            piece1 = new GameObject(nameof(piece1)).AddComponent<Piece>();
            piece1.Awake();
            boardPiece1 = new GameObject(nameof(boardPiece1)).AddComponent<BoardPiece>();
            boardPiece1.InitializeVariables();
            // GameObject boardPrefab = (Resources.Load("Board/Prefabs/DinamicBoard") as GameObject);
            // dinamicBoard = UnityEngine.Object.Instantiate(boardPrefab).GetComponent<DinamicBoard>();
            // dinamicBoard.Awake();
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
            // board.SignOn();
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
        public void Pawn_Can_Move_To_The_Square_In_Front_Of_It()
        {
            //SETUP
            PieceMoveSet pawnMoveSet = Resources.Load("Pieces/Scriptable/MoveSet/PawnMoveSet") as PieceMoveSet;
            piece1.moveSet = pawnMoveSet;
            GameObject boardPrefab = (Resources.Load("Board/Prefabs/DinamicBoard") as GameObject);
            DinamicBoard dinamicBoard = UnityEngine.Object.Instantiate(boardPrefab).GetComponent<DinamicBoard>();
            dinamicBoard.Awake();
            // dinamicBoard.ForceUpdateBoard(Vector3Int.RoundToInt(Vector3.forward + Vector3.one), Vector3.zero);
            BoardPiece boardPiece = dinamicBoard.GetSquareAt(new Vector3Int(0, 0, 0));
            piece1.MoveTo(boardPiece);
            //ACT
            bool? canMove = dinamicBoard.IsMovimentAvailable(piece1, new Vector3Int(0, 0, 1));
            //ASSERT
            Assert.IsTrue(canMove);
        }
        [Test]
        public void Board_Can_Be_3x3x3()
        {
            //SETUP
            GameObject boardPrefab = (Resources.Load("Board/Prefabs/DinamicBoard") as GameObject);
            DinamicBoard dinamicBoard = UnityEngine.Object.Instantiate(boardPrefab).GetComponent<DinamicBoard>();
            dinamicBoard.Awake();
            //ACT
            dinamicBoard.size = new Vector3Int(3, 3, 3);
            dinamicBoard.ResetBoardSize(new Vector3Int(3, 3, 3));
            //ASSERT
            Assert.AreEqual(new BoardPiece[3, 3, 3].Length, dinamicBoard.board.Length);
        }
        [Test]
        public void Select_Selected_Piece_Turn_Board_Highlight_Off()
        {
            //SETUP
            GameObject boardPrefab = (Resources.Load("Board/Prefabs/DinamicBoard") as GameObject);
            DinamicBoard dinamicBoard = UnityEngine.Object.Instantiate(boardPrefab).GetComponent<DinamicBoard>();
            dinamicBoard.Awake();
            //ACT
            piece1.OnSelected();
            piece1.OnDeselected();
            //ASSERT
            foreach (BoardPiece square in dinamicBoard.board)
                Assert.IsFalse(square.Highlight.IsHighlighted);
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
            BoardPiece whiteBoardPiece = (Resources.Load("Board/Prefabs/WhiteSquare") as GameObject).GetComponent<BoardPiece>();
            GameObject boardPrefab = (Resources.Load("Board/Prefabs/DinamicBoard") as GameObject);
            DinamicBoard dinamicBoard = UnityEngine.Object.Instantiate(boardPrefab).GetComponent<DinamicBoard>();
            dinamicBoard.Awake();
            //ACT
            BoardPiece boardPiece = dinamicBoard.GenerateSquareUsing(2);
            //ASSERT
            Assert.AreEqual(whiteBoardPiece.so_pieceData, boardPiece.so_pieceData);
        }
        [Test]
        public void GenerateSquareUsing_OddIndex_BlackBoardPiece()
        {
            //SETUP
            BoardPiece whiteBoardPiece = (Resources.Load("Board/Prefabs/BlackSquare") as GameObject).GetComponent<BoardPiece>();
            GameObject boardPrefab = (Resources.Load("Board/Prefabs/DinamicBoard") as GameObject);
            DinamicBoard dinamicBoard = UnityEngine.Object.Instantiate(boardPrefab).GetComponent<DinamicBoard>();
            dinamicBoard.Awake();
            //ACT
            BoardPiece boardPiece = dinamicBoard.GenerateSquareUsing(1);
            //ASSERT
            Assert.AreEqual(whiteBoardPiece.so_pieceData, boardPiece.so_pieceData);
        }
        [Test]
        public void GetPool_PairIndex_WhitePool()
        {
            //SETUP
            GameObject boardPrefab = (Resources.Load("Board/Prefabs/DinamicBoard") as GameObject);
            DinamicBoard dinamicBoard = UnityEngine.Object.Instantiate(boardPrefab).GetComponent<DinamicBoard>();
            dinamicBoard.Awake();
            //ACT
            Pool<BoardPiece> pool = dinamicBoard.GetPool(0);
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
            Pool<BoardPiece> pool = dinamicBoard.GetPool(1);
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
        public void UpdateBoardCoordAt_0x0x0_BoardPieceCoord0x0x0()
        {
            //SETUP
            DinamicBoard dinamicBoard = LoadDinamicBoardPrefab();
            dinamicBoard.board = dinamicBoard.GenerateBoard(new Vector3Int(1, 1, 1));
            //ACT
            dinamicBoard.UpdateBoardCoordAt(0, 0, 0);
            //ASSERT
            Assert.AreEqual(new Vector3Int(0, 0, 0), dinamicBoard.board[0, 0, 0].BoardCoord);
        }
        [Test]
        public void GenerateBoard_1x1x1_PieceIsNotNull()
        {
            //SETUP
            // DinamicBoard dinamicBoard = new GameObject(nameof(dinamicBoard)).AddComponent<DinamicBoard>();
            DinamicBoard dinamicBoard = LoadDinamicBoardPrefab();
            //ACT
            dinamicBoard.board = dinamicBoard.GenerateBoard(new Vector3Int(1, 1, 1));
            //ASSERT
            Assert.NotNull(dinamicBoard.board[0, 0, 0]);
        }
    }
    public class PieceMoviment
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
    }
    public class PoolTests
    {
        public class Poolable : Component, IPoolable
        {
            public IPoolable Activated() => throw new System.NotImplementedException();
            public IPoolable Deactivated() => throw new System.NotImplementedException();
            public IPoolable InstantiatePoolable(Transform poolParent) => throw new System.NotImplementedException();

            public IPoolable InstantiatePoolable() => throw new System.NotImplementedException();
        }

        [Test]
        public void Cant_Push_Null_To_Pool()
        {
            //SETUP
            Pool<Poolable> pool = new Pool<Poolable>();
            //ACT
            pool.PushToPool(null);
            //ASSERT
            Assert.AreEqual(0, pool.objectPool.Count);
        }
        [Test]
        public void Push_Board_Piece_To_Pool_Gets_Deactivated()
        {
            //SETUP
            Pool<BoardPiece> squarePool1 = new Pool<BoardPiece>();
            BoardPiece boardPiece1 = new GameObject(nameof(boardPiece1)).AddComponent<BoardPiece>();
            //ACT
            squarePool1.PushToPool(boardPiece1);
            //ASSERT
            Assert.IsFalse(boardPiece1.gameObject.activeSelf);
        }
        [Test]
        public void Get_Board_Piece_From_Pool_Gets_Activated()
        {
            //SETUP
            BoardPiece boardPiece1 = new GameObject(nameof(boardPiece1)).AddComponent<BoardPiece>();
            Pool<BoardPiece> squarePool1 = new Pool<BoardPiece>(new List<BoardPiece> { boardPiece1 });
            //ACT
            BoardPiece boardPiece2 = squarePool1.GetFromPool();
            //ASSERT
            Assert.IsTrue(boardPiece2.gameObject.activeSelf);
        }
        [Test]
        public void Get_Board_Piece_From_Empty_Pool_Creates_An_Board_Piece()
        {
            //SETUP
            Pool<BoardPiece> squarePool1 = new Pool<BoardPiece>().Initialized();
            //ACT
            BoardPiece boardPiece = squarePool1.GetFromPool();
            //ASSERT
            Assert.IsNotNull(boardPiece);
        }
        [Test]
        public void Get_Grouped_Board_Piece_From_Pool_Gets_Activated()
        {
            //SETUP
            BoardPiece boardPiece1 = new GameObject(nameof(boardPiece1)).AddComponent<BoardPiece>();
            Pool<BoardPiece> squarePool1 = new Pool<BoardPiece>(new List<BoardPiece> { boardPiece1 });
            //ACT
            BoardPiece boardPiece2 = squarePool1.GetFromPoolGrouped();
            //ASSERT
            Assert.IsTrue(boardPiece2.gameObject.activeSelf);
        }
        [Test]
        public void Get_Grouped_Board_Piece_From_Empty_Pool_Creates_An_Board_Piece()
        {
            //SETUP
            Pool<BoardPiece> squarePool1 = new Pool<BoardPiece>().Initialized();
            //ACT
            BoardPiece boardPiece = squarePool1.GetFromPoolGrouped();
            //ASSERT
            Assert.IsNotNull(boardPiece);
        }
        [Test]
        public void Get_Grouped_Board_Piece_From_Pool_Put_Board_Piece_As_Transform_Child()
        {
            //SETUP
            BoardPiece boardPiece1 = new GameObject(nameof(boardPiece1)).AddComponent<BoardPiece>();
            Pool<BoardPiece> squarePool1 = new Pool<BoardPiece>().Initialized();
            squarePool1.poolParent = new GameObject().transform;
            //ACT
            BoardPiece boardPiece = squarePool1.GetFromPoolGrouped();
            //ASSERT
            Assert.AreEqual(squarePool1.poolParent, boardPiece.transform.parent);
        }
    }
    public class BoardPieceTests
    {
        [Test]
        public void TestMethod()
        {
            //ACT
            //ASSERT
        }
    }
}
