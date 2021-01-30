using NUnit.Framework;
using UnityEngine;

    public class CanvasVisibilityTests
    {
        [Test]
        public void SetCanvasGroup_Bool_SetCanvasGroupBlockRaycast([Values] bool visible)
        {
            //SETUP
            CanvasVisibility canvasVisibility = new GameObject().AddComponent<CanvasVisibility>().Initialized() as CanvasVisibility;
            //ACT
            canvasVisibility.SetCanvasGroup(visible);
            //ASSERT
            Assert.AreEqual(visible, canvasVisibility.CanvasGroup.blocksRaycasts);
        }
        [Sequential]
        [Test]
        public void SetCanvasGroup_WasOposite_SetCanvasGroupAlpha([Values(true, false)] bool visible, [Values(1, 0)] float expected)
        {
            //SETUP
            CanvasVisibility canvasVisibility = new GameObject().AddComponent<CanvasVisibility>().Initialized() as CanvasVisibility;
            //ACT
            canvasVisibility.SetCanvasGroup(!visible);
            canvasVisibility.SetCanvasGroup(visible);
            //ASSERT
            Assert.AreEqual(expected, canvasVisibility.CanvasGroup.alpha);
        }
    }
