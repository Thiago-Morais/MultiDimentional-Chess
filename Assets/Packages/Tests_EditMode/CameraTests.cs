using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Cinemachine;
using UnityEngine.TestTools;

namespace Tests_EditMode
{
    public class CameraTests
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void Initialized__HoverCameraNotNull()
        {
            //SETUP
            CameraControll cameraControll = new GameObject().AddComponent<CameraControll>();
            //ACT
            cameraControll.Initialized();
            //ASSERT
            Assert.NotNull(cameraControll.hoverCamera);
        }
        [UnityTest]
        public IEnumerator ActivateHoverCamera__ActivateHoverCamera()
        {
            //SETUP
            CameraControll cameraControll = new GameObject().AddComponent<CameraControll>().Initialized() as CameraControll;
            //ACT
            cameraControll.ActivateHoverCamera();
            yield return null;
            bool isActive = CinemachineCore.Instance.IsLive(cameraControll.hoverCamera);
            //ASSERT
            Assert.True(isActive);
        }
        [UnityTest]
        public IEnumerator DeactivateHoverCamera__DeactivateHoverCamera()
        {
            //SETUP
            CameraControll cameraControll = new GameObject().AddComponent<CameraControll>().Initialized() as CameraControll;
            //ACT
            cameraControll.DeactivateHoverCamera();
            yield return null;
            bool isActive = CinemachineCore.Instance.IsLive(cameraControll.hoverCamera);
            //ASSERT
            Assert.False(isActive);
        }
    }
}
