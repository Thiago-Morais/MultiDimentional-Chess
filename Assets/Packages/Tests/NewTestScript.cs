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
        HoverHighlight hoverHighlight;
        [SetUp]
        public void Setup()
        {
            selector = new GameObject().AddComponent<Selector>();
            piece1 = new GameObject(nameof(piece1)).AddComponent<Piece>();
            piece1.Awake();
            piece2 = new GameObject(nameof(piece2)).AddComponent<Piece>();
            piece2.Awake();
            hoverHighlight = new GameObject().AddComponent<HoverHighlight>();
        }
        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(selector.gameObject);
            Object.DestroyImmediate(piece1.gameObject);
            Object.DestroyImmediate(piece2.gameObject);
            Object.DestroyImmediate(hoverHighlight.gameObject);
        }
        [Test]
        public void SelectPiece()
        {
            //ACT
            selector.ChangeSelection(piece1 as ISelectable);

            //ASSERT
            Assert.IsTrue(piece1.highlight.IsHighlighted);
            Assert.AreEqual(selector.HighlightType, piece1.highlight.CachedHighlightType);
        }
        [Test]
        public void SelectPieceThenSelectOtherPiece()
        {
            //ACT
            selector.ChangeSelection(piece1 as ISelectable);
            selector.ChangeSelection(piece2 as ISelectable);

            //ASSERT
            Assert.IsFalse(piece1.highlight.IsHighlighted);
            Assert.IsTrue(piece2.highlight.IsHighlighted);
            Assert.AreEqual(selector.HighlightType, piece2.highlight.CachedHighlightType);
        }
        [Test]
        public void HoverOverPiece()
        {
            //ACT
            hoverHighlight.HoveredIn(piece1);

            //ASSERT
            Assert.IsTrue(piece1.highlight.IsHighlighted);
        }
        [Test]
        public void HoverOutOfPieceAfterHoveringIn()
        {
            //ACT
            hoverHighlight.HoveredIn(piece1);
            hoverHighlight.HoveredOut(piece1);

            //ASSERT
            Assert.IsFalse(piece1.highlight.IsHighlighted);
            Assert.AreEqual(piece1.highlight.CachedHighlightType, piece1.highlight.HighlightType);
        }
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            yield return null;
        }
    }
}
