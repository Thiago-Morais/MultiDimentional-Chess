using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
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
        HoverHighlight hoverHighlight;

        [SetUp]
        public void Setup()
        {
            selector = new GameObject().AddComponent<Selector>();
            piece1 = new GameObject(nameof(piece1)).AddComponent<Piece>();
            piece1.Awake();
            piece2 = new GameObject(nameof(piece2)).AddComponent<Piece>();
            piece2.Awake();
            boardPiece1 = new GameObject(nameof(boardPiece1)).AddComponent<BoardPiece>();
            boardPiece2 = new GameObject(nameof(boardPiece2)).AddComponent<BoardPiece>();
            hoverHighlight = new GameObject(nameof(hoverHighlight)).AddComponent<HoverHighlight>();
        }
        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(selector.gameObject);
            Object.DestroyImmediate(piece1.gameObject);
            Object.DestroyImmediate(piece2.gameObject);
            Object.DestroyImmediate(boardPiece1.gameObject);
            Object.DestroyImmediate(boardPiece2.gameObject);
            Object.DestroyImmediate(hoverHighlight.gameObject);
        }
        [Test]
        public void IsSelectedPieceHighlighted()
        {
            //ACT
            selector.ChangeSelection(piece1 as ISelectable);
            //ASSERT
            Assert.IsTrue(piece1.highlight.IsHighlighted);
            Assert.AreEqual(selector.HighlightType, piece1.highlight.CachedHighlightType);
        }
        [Test]
        public void SelectPieceChangeHighlightType()
        {
            //ACT
            selector.ChangeSelection(piece2 as ISelectable);
            //ASSERT
            Assert.AreEqual(selector.HighlightType, piece2.highlight.CachedHighlightType);
        }
        [Test]
        public void SelectSecondPieceDisableFirstHighlight()
        {
            //ACT
            selector.ChangeSelection(piece1 as ISelectable);
            selector.ChangeSelection(piece2 as ISelectable);
            //ASSERT
            Assert.IsFalse(piece1.highlight.IsHighlighted);
        }
        [Test]
        public void SelectSecondPieceResetFirstHighlightType()
        {
            //ACT
            selector.ChangeSelection(piece1 as ISelectable);
            selector.ChangeSelection(piece2 as ISelectable);
            //ASSERT
            Assert.AreEqual(selector.HighlightType, piece2.highlight.CachedHighlightType);
        }
        [Test]
        public void IsHoveredPieceHighlighted()
        {
            //ACT
            hoverHighlight.HoveredIn(piece1);
            //ASSERT
            Assert.IsTrue(piece1.highlight.IsHighlighted);
        }
        [Test]
        public void HoverOutOfPieceDisableHighlight()
        {
            //ACT
            hoverHighlight.HoveredIn(piece2);
            hoverHighlight.HoveredOut(piece2);
            //ASSERT
            Assert.IsFalse(piece2.highlight.IsHighlighted);
        }
        [Test]
        public void HoverOutOfPieceResetHighlightType()
        {
            //ACT
            hoverHighlight.HoveredIn(piece1);
            hoverHighlight.HoveredOut(piece1);
            //ASSERT
            Assert.AreEqual(piece1.highlight.CachedHighlightType, piece1.highlight.HighlightType);
        }
        [Test]
        public void SelectOwnPiece()
        {
            //ACT
            selector.ChangeSelection(piece2 as ISelectable);
            //ASSERT
        }
        /*
        TODO testes a implementar
        peça selecionada mostra possibilidade de movimento
        troca de seleção desliga tabuleiro
        */
    }
    public class BoardTests
    {
        Piece piece1;
        BoardPiece boardPiece1;
        DinamicBoard board;
        [SetUp]
        public void Setup()
        {
            piece1 = new GameObject(nameof(piece1)).AddComponent<Piece>();
            piece1.Awake();
            boardPiece1 = new GameObject(nameof(boardPiece1)).AddComponent<BoardPiece>();
            boardPiece1.InitializeVariables();
            GameObject boardPrefab = (Resources.Load("Board/Prefabs/DinamicBoard") as GameObject);
            board = UnityEngine.Object.Instantiate(boardPrefab).GetComponent<DinamicBoard>();
            board.ForceUpdateBoard();
        }
        [TearDown]
        public void TearDown()
        {
        }
        [Test]
        public void SelectedPawnDisplayPossibleMoves()
        {
            //SETUP
            PieceMoveSet pawnMoveSet = Resources.Load("Pieces/Scriptable/MoveSet/PawnMoveSet") as PieceMoveSet;
            piece1.moveSet = pawnMoveSet;
            Selector selector = new GameObject().AddComponent<Selector>();
            board.SignOn();
            //ACT
            selector.ChangeSelection(piece1);
            //ASSERT
            foreach (BoardPiece square in board.board)
                if (piece1.IsAnyMovimentAvailable(square) || piece1.BoardCoord == square.BoardCoord)
                    Assert.IsTrue(square.Highlight.IsHighlighted);
                else
                    Assert.IsFalse(square.Highlight.IsHighlighted);

        }
        [Test]
        public void PawnCanMoveToTheSquareInFrontOfIt()
        {
            //SETUP
            PieceMoveSet pawnMoveSet = Resources.Load("Pieces/Scriptable/MoveSet/PawnMoveSet") as PieceMoveSet;
            piece1.moveSet = pawnMoveSet;
            board.ForceUpdateBoard(Vector3Int.RoundToInt(Vector3.forward + Vector3.one), Vector3.zero);
            BoardPiece boardPiece = board.GetSquareAt(new Vector3Int(0, 0, 0));
            piece1.MoveTo(boardPiece);
            //ACT
            bool? canMove = board.IsMovimentAvailable(piece1, new Vector3Int(0, 0, 1));
            //ASSERT
            Assert.IsTrue(canMove);
        }
        [Test]
        public void BoardCanBe3x3()
        {
            //SETUP

            //ACT
            board.size = new Vector3Int(3, 3, 3);
            board.ResetBoardSize(new Vector3Int(3, 3, 3));
            //ASSERT
            Assert.AreEqual(new BoardPiece[3, 3, 3].Length, board.board.Length);
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
    public class Pool
    {
        class Poolable : Component, IPoolable
        {
            public IPoolable Activated() => throw new System.NotImplementedException();
            public IPoolable Deactivated() => throw new System.NotImplementedException();
            public IPoolable Instantiate(Transform poolParent) => throw new System.NotImplementedException();
        }

        [Test]
        public void CantPushNullToPool()
        {
            //SETUP
            Pool<Poolable> pool = new Pool<Poolable>();
            //ACT
            pool.PushToPool(null);
            //ASSERT
            Assert.AreEqual(0, pool.objectPool.Count);
        }
        [Test]
        public void BoardPieceAddedToPoolGetsDeactivated()
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
        public void BoardPieceRemovedFromPoolGetsActivated()
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
        public void GettingBoardPieceFromEmptyPoolCreatesAnBoardPiece()
        {
            //SETUP
            Pool<BoardPiece> squarePool1 = new Pool<BoardPiece>().Initialized();
            //ACT
            BoardPiece boardPiece = squarePool1.GetFromPool();
            //ASSERT
            Assert.IsNotNull(boardPiece);
        }
        [Test]
        public void GetBoardPieceFromPoolGroupedPutBoardPieceAsTransformChild()
        {
            //SETUP
            BoardPiece boardPiece1 = new GameObject(nameof(boardPiece1)).AddComponent<BoardPiece>();
            Pool<BoardPiece> squarePool1 = new Pool<BoardPiece>().Initialized();
            //ACT
            BoardPiece boardPiece = squarePool1.GetFromPoolGrouped();
            //ASSERT
            Assert.AreEqual(squarePool1.poolParent, boardPiece.transform.parent);
        }
    }
}
