using NUnit.Framework;
using UnityEngine;
using ExtensionMethods;
using System.Collections.Generic;

namespace Tests
{
    #region ------- POOL TESTS
    public class PoolTests
    {
        [Test]
        public void InitializedAs_BoardPieceWithNoPrefab_SampleIsInstanceOfBoardPiece()
        {
            //SETUP
            Pool pool = new GameObject(nameof(pool)).AddComponent<Pool>();
            //ACT
            pool.InitializedAs<BoardPiece>();
            //ASSERT
            Assert.IsInstanceOf<BoardPiece>(pool.sample);
        }
        [Test]
        public void InitializedAs_BoardPieceWithPrefab_SampleIsSameAsPrefabComponent()
        {
            //SETUP
            BoardPiece boardPiece = new GameObject(nameof(boardPiece)).AddComponent<BoardPiece>();
            Pool pool = new GameObject(nameof(pool)).AddComponent<Pool>();
            pool.prefab = boardPiece.gameObject;
            //ACT
            pool.InitializedAs<BoardPiece>();
            //ASSERT
            Assert.AreSame(boardPiece, pool.sample);
        }
        [Test]
        public void PushToPool_Null_KeepPoolCount()
        {
            //SETUP
            Pool pool = new GameObject(nameof(pool)).AddComponent<Pool>().InitializedAs<BoardPiece>();
            //ACT
            pool.PushToPool<BoardPiece>(null);
            //ASSERT
            Assert.AreEqual(0, pool.objectPool.Count);
        }
        [Test]
        public void PushToPool_BoardPiece_GetsDeactivated()
        {
            //SETUP
            Pool pool = new GameObject(nameof(pool)).AddComponent<Pool>();
            BoardPiece boardPiece = new GameObject(nameof(boardPiece)).AddComponent<BoardPiece>();
            //ACT
            pool.PushToPool(boardPiece);
            //ASSERT
            Assert.IsFalse(boardPiece.gameObject.activeSelf);
        }
        [Test]
        public void GetFromPool_WithBoardPieceType_ActiveBoardPiece()
        {
            //SETUP
            Pool pool = new GameObject(nameof(pool)).AddComponent<Pool>();
            //ACT
            BoardPiece boardPiece = pool.GetFromPool<BoardPiece>();
            //ASSERT
            Assert.IsTrue(boardPiece.gameObject.activeSelf);
        }
        [Test]
        public void GetFromPool_WithBoardPieceType_NotNullBoardPiece()
        {
            //SETUP
            Pool pool = new GameObject(nameof(pool)).AddComponent<Pool>();
            //ACT
            BoardPiece boardPiece = pool.GetFromPool<BoardPiece>();
            //ASSERT
            Assert.IsNotNull(boardPiece);
        }
        [Test]
        public void GetFromPoolGrouped_WithBoardPieceType_ActiveBoardPiece()
        {
            //SETUP
            Pool pool = new GameObject(nameof(pool)).AddComponent<Pool>();
            //ACT
            BoardPiece boardPiece = pool.GetFromPoolGrouped<BoardPiece>();
            //ASSERT
            Assert.IsTrue(boardPiece.gameObject.activeSelf);
        }
        [Test]
        public void GetFromPoolGrouped_WithBoardPieceType_NotNullBoardPiece()
        {
            //SETUP
            Pool pool = new GameObject(nameof(pool)).AddComponent<Pool>();
            //ACT
            BoardPiece boardPiece = pool.GetFromPoolGrouped<BoardPiece>();
            //ASSERT
            Assert.IsNotNull(boardPiece);
        }
        [Test]
        public void GetFromPoolGrouped_WithBoardPieceType_BoardPieceParentIsPool()
        {
            //SETUP
            Pool pool = new GameObject(nameof(pool)).AddComponent<Pool>();
            //ACT
            BoardPiece boardPiece = pool.GetFromPoolGrouped<BoardPiece>();
            //ASSERT
            Assert.AreSame(pool.transform, boardPiece.transform.parent);
        }
        [Test]
        public void SetSampleWithPrefab_BoardPieceType_SampleIsThePrefabComponentBoardPiece()
        {
            //SETUP
            Pool pool = new GameObject(nameof(pool)).AddComponent<Pool>();
            BoardPiece prefabComp = new GameObject(nameof(BoardPiece)).AddComponent<BoardPiece>();
            pool.prefab = prefabComp.gameObject;
            //ACT
            pool.SetSampleWithPrefab<BoardPiece>();
            //ASSERT
            Assert.AreSame(prefabComp, pool.sample);
        }
    }
    #endregion //POOL TESTS
    #region ------- HIGHLIGHT TESTS
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
        #region ------- HOVERHIGHLIGHT TESTS
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
        #endregion //HOVERHIGHLIGHT TESTS
    }
    #endregion //HIGHLIGHT TESTS
    #region ------- SELECTOR TESTS
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
    #endregion //SELECTOR TESTS
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
        public void RemoveMatchingElements_ZeroThroughTenAndEvenThroughTen_OddNumbers()
        {
            //SETUP
            //ACT
            //ASSERT
        }
    }
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
    #endregion //PIECE MOVE SET TESTS
    #region ------- PIECE

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
    #endregion //PIECEMOVIMENT TESTS
    #endregion //PIECE
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
