using NUnit.Framework;
using UnityEngine;
using static ExtensionMethods.ReferenceExtension;

namespace Tests_EditMode
{
    public class Ext_ReferenceExtension
    {
        public class TestReference : MonoBehaviour, IInitializable
        {
            public IInitializable Initialized(Transform parent = null) => this;
        }
        [Test]
        public void InstantiateInitialized_PassedComponentTypeWithIInitializable_CreateObjectWithComponent()
        {
            //ACT
            TestReference reference = InstantiateInitialized<TestReference>();
            //ASSERT
            Assert.IsInstanceOf<TestReference>(reference);
        }
    }
}