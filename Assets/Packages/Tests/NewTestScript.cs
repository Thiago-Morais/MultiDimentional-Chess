using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class NewTestScript
    {
        // A Test behaves as an ordinary method
        [Test]
        public void NewTestScriptSimplePasses()
        {
            // Use the Assert class to test conditions
        }
        [Test]
        public void selected_piece_display_highlight()
        {
            //ARRANGE
            GameObject selectorObj = new GameObject();
            GameObject pieceObj = new GameObject();
            Selector selector = selectorObj.AddComponent<Selector>();
            Piece piece = pieceObj.AddComponent<Piece>();
            piece.Awake();
            // Highlight highlight = pieceObj.AddComponent<Highlight>();
            // piece.highlight = highlight;

            //ACT
            selector.ChangeSelection(piece as ISelectable);
            bool isHighlighted = piece.highlight.IsHighlighted;

            //ASSERT
            Assert.IsTrue(isHighlighted);
        }
        [Test]
        public void hover_over_piece_highlight_it()
        {
            //ARRANGE
            Piece hoveredPiece = new GameObject().AddComponent<Piece>();
            HoverHighlight hoverHighlight = new GameObject().AddComponent<HoverHighlight>();
            hoveredPiece.Awake();

            //ACT
            hoverHighlight.HoveredOver(hoveredPiece);

            //ASSERT
            Assert.IsTrue(hoveredPiece.highlight.IsHighlighted);
        }
        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator NewTestScriptWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
