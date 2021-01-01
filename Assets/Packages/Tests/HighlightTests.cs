using NUnit.Framework;
using UnityEngine;
using ExtensionMethods;

namespace Tests
{
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
}
