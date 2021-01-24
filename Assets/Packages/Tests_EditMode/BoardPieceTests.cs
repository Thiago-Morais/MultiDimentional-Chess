using System;
using NUnit.Framework;
using UnityEngine;

namespace Tests_EditMode
{

    #region ------- BOARDPIECE TESTS
    public class BoardPieceTests
    {
        static object[] BoundsSize =
        {
            Vector3.zero,
            Vector3.one,
        };
        [TestCaseSource(nameof(BoundsSize))]
        [Test]
        public void GetSize_PieceDataIsSet_ReturnsSizeOnPieceData(Vector3 expected)
        {
            BoardPiece boardPiece = new GameObject(nameof(boardPiece)).AddComponent<BoardPiece>().Initialized() as BoardPiece;
            boardPiece.so_pieceData.pieceBounds = new Bounds(Vector3.zero, expected);
            //ACT
            Vector3 size = boardPiece.GetSize();
            //ASSERT
            Assert.AreEqual(expected, size);
        }
    }
    #endregion //BOARDPIECE TESTS
}
