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
        Piece piece;
        HoverHighlight hoverHighlight;
        [SetUp]
        public void Setup()
        {
            selector = new GameObject().AddComponent<Selector>();
            piece = new GameObject().AddComponent<Piece>();
            piece.Awake();
            hoverHighlight = new GameObject().AddComponent<HoverHighlight>();
        }
        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(selector.gameObject);
            Object.DestroyImmediate(piece.gameObject);
            Object.DestroyImmediate(hoverHighlight.gameObject);
        }
        [Test]
        public void SelectPiece()
        {
            //ACT
            selector.ChangeSelection(piece as ISelectable);

            //ASSERT
            Assert.IsTrue(piece.highlight.IsHighlighted);
            Assert.AreEqual(selector.HighlightType, piece.highlight.CachedHighlightType);
        }
        [Test]
        public void HoverOverPiece()
        {
            //ACT
            hoverHighlight.HoveredIn(piece);

            //ASSERT
            Assert.IsTrue(piece.highlight.IsHighlighted);
        }
        [Test]
        public void HoverOutOfPieceAfterHoveringIn()
        {
            //ACT
            hoverHighlight.HoveredIn(piece);
            hoverHighlight.HoveredOut(piece);

            //ASSERT
            Assert.IsFalse(piece.highlight.IsHighlighted);
            Assert.AreEqual(piece.highlight.CachedHighlightType, piece.highlight.HighlightType);
        }
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            yield return null;
        }
    }
}
