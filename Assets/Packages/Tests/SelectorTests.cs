using NUnit.Framework;
using UnityEngine;

namespace Tests
{
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
}
