using NUnit.Framework;
using UnityEngine;

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
}
