using NUnit.Framework;
using ExtensionMethods;
using UnityEngine;

namespace Tests
{
    public partial class ExtensionsTests
    {
        public class VectorExtensionTests
        {
            [Test]
            public void AsInt_Vector3Up_VectorIntUp()
            {
                //SETUP
                //ACT
                Vector3Int vector = Vector3.up.AsInt();
                //ASSERT
                Assert.AreEqual(new Vector3Int(0, 1, 0), vector);
            }
            [Test]
            public void Rank_ZeroOnAllAxis_0()
            {
                //SETUP
                Vector3Int vector = new Vector3Int(0, 0, 0);
                //ACT
                byte rank = vector.Rank();
                //ASSERT
                Assert.Zero(rank);
            }
            [Test]
            [TestCase(1, 0, 0)]
            [TestCase(0, 1, 0)]
            [TestCase(0, 0, 1)]
            [TestCase(-1, 0, 0)]
            [TestCase(0, -1, 0)]
            [TestCase(0, 0, -1)]
            public void Rank_NonZeroOnOneAxis_1(int vectorX, int vectorY, int vectorZ)
            {
                //SETUP
                Vector3Int vector = new Vector3Int(vectorX, vectorY, vectorZ);
                //ACT
                byte rank = vector.Rank();
                //ASSERT
                Assert.AreEqual(1, rank);
            }
            [Test]
            [TestCase(1, 1, 0)]
            [TestCase(1, 0, 1)]
            [TestCase(0, 1, 1)]
            [TestCase(-1, -1, 0)]
            [TestCase(-1, 0, -1)]
            [TestCase(0, -1, -1)]
            public void Rank_NonZeroOnTwoAxis_2(int vectorX, int vectorY, int vectorZ)
            {
                //SETUP
                Vector3Int vector = new Vector3Int(vectorX, vectorY, vectorZ);
                //ACT
                byte rank = vector.Rank();
                //ASSERT
                Assert.AreEqual(2, rank);
            }
            [Test]
            [TestCase(1, 1, 1)]
            [TestCase(-1, -1, -1)]
            public void Rank_NonZeroOnThreeAxis_3(int vectorX, int vectorY, int vectorZ)
            {
                //SETUP
                Vector3Int vector = new Vector3Int(vectorX, vectorY, vectorZ);
                //ACT
                byte rank = vector.Rank();
                //ASSERT
                Assert.AreEqual(3, rank);
            }
        }
    }
}
