using NUnit.Framework;
using UnityEngine;
using Cinemachine;
using System.Linq;
using UnityEngine.TestTools;
using System.Collections;
using System;

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
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>();
            //ACT
            hoverControll.Initialized();
            //ASSERT
            Assert.NotNull(hoverControll.hoverCamera);
        }
        [Test]
        public void ResetFreeLookAxis__SetFreeLookXAxisNameToCached()
        {
            //SETUP
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();
            freeLook.m_XAxis.m_InputAxisName = "Mouse X";

            GameObject gameObject = new GameObject();
            freeLook.transform.SetParent(gameObject.transform);

            HoverControll hoverControll = gameObject.AddComponent<HoverControll>().Initialized() as HoverControll;
            hoverControll.CacheVCamInputAxis();

            hoverControll.cachedXAxis = freeLook.m_XAxis.m_InputAxisName;
            hoverControll.cachedYAxis = freeLook.m_YAxis.m_InputAxisName;

            freeLook.m_XAxis.m_InputAxisName = "";
            freeLook.m_XAxis.m_InputAxisValue = 0;
            //ACT
            hoverControll.ResetFreeLookAxis();
            //ASSERT
            Assert.AreEqual(hoverControll.cachedXAxis, hoverControll.hoverCamera.m_XAxis.m_InputAxisName);
        }
        [Test]
        public void ResetFreeLookAxis__SetFreeLookYAxisNameToCached()
        {
            //SETUP
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();
            freeLook.m_YAxis.m_InputAxisName = "Mouse Y";

            GameObject gameObject = new GameObject();
            freeLook.transform.SetParent(gameObject.transform);

            HoverControll hoverControll = gameObject.AddComponent<HoverControll>().Initialized() as HoverControll;
            hoverControll.CacheVCamInputAxis();

            hoverControll.cachedXAxis = freeLook.m_XAxis.m_InputAxisName;
            hoverControll.cachedYAxis = freeLook.m_YAxis.m_InputAxisName;

            freeLook.m_YAxis.m_InputAxisName = "";
            freeLook.m_YAxis.m_InputAxisValue = 0;
            //ACT
            hoverControll.ResetFreeLookAxis();
            //ASSERT
            Assert.AreEqual(hoverControll.cachedYAxis, hoverControll.hoverCamera.m_YAxis.m_InputAxisName);
        }
        [Test]
        public void RemoveFreeLookAxis__SetFreeLookXAxisNameToEmpty()
        {
            //SETUP
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();
            freeLook.m_XAxis.m_InputAxisName = "Mouse X";

            GameObject gameObject = new GameObject();
            freeLook.transform.SetParent(gameObject.transform);

            HoverControll hoverControll = gameObject.AddComponent<HoverControll>().Initialized() as HoverControll;
            //ACT
            hoverControll.RemoveFreeLookAxis();
            //ASSERT
            Assert.AreEqual("", hoverControll.hoverCamera.m_XAxis.m_InputAxisName);
        }
        [Test]
        public void RemoveFreeLookAxis__SetFreeLookYAxisNameToEmpty()
        {
            //SETUP
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();
            freeLook.m_YAxis.m_InputAxisName = "Mouse Y";

            GameObject gameObject = new GameObject();
            freeLook.transform.SetParent(gameObject.transform);

            HoverControll hoverControll = gameObject.AddComponent<HoverControll>().Initialized() as HoverControll;
            //ACT
            hoverControll.RemoveFreeLookAxis();
            //ASSERT
            Assert.AreEqual("", hoverControll.hoverCamera.m_YAxis.m_InputAxisName);
        }
        [Test]
        public void RemoveFreeLookAxis__SetFreeLookXAxisValueTo0()
        {
            //SETUP
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();
            freeLook.m_XAxis.m_InputAxisValue = 1;

            GameObject gameObject = new GameObject();
            freeLook.transform.SetParent(gameObject.transform);

            HoverControll hoverControll = gameObject.AddComponent<HoverControll>().Initialized() as HoverControll;
            //ACT
            hoverControll.RemoveFreeLookAxis();
            //ASSERT
            Assert.AreEqual(0, hoverControll.hoverCamera.m_XAxis.m_InputAxisValue);
        }
        [Test]
        public void RemoveFreeLookAxis__SetFreeLookYAxisValueTo0()
        {
            //SETUP
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();
            freeLook.m_YAxis.m_InputAxisValue = 1;

            GameObject gameObject = new GameObject();
            freeLook.transform.SetParent(gameObject.transform);

            HoverControll hoverControll = gameObject.AddComponent<HoverControll>().Initialized() as HoverControll;
            //ACT
            hoverControll.RemoveFreeLookAxis();
            //ASSERT
            Assert.AreEqual(0, hoverControll.hoverCamera.m_YAxis.m_InputAxisValue);
        }
        [Test]
        public void SetZoom_ResultantHeightIsNegative_SetHegihtToZero([Values(0, 1, 2)] int orbit, [NUnit.Framework.Range(0, 1, 0.2f)] float zoom)
        {
            //SETUP
            HoverControll hoverControll = new GameObject().AddComponent<HoverControll>();

            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();
            freeLook.transform.SetParent(hoverControll.transform);

            hoverControll.Initialized();
            hoverControll.cachedHeights = hoverControll.hoverCamera.m_Orbits.Select(c => c.m_Height).ToArray();
            hoverControll.cachedRadiuses = hoverControll.hoverCamera.m_Orbits.Select(c => c.m_Radius).ToArray();

            for (int i = 0; i < freeLook.m_Orbits.Length; i++)
                freeLook.m_Orbits[i].m_Height = hoverControll.minZoom;
            //ACT
            hoverControll.SetZoom(zoom);
            //ASSERT
            Assert.AreEqual(hoverControll.minZoom, hoverControll.hoverCamera.m_Orbits[orbit].m_Height);
        }
        [Test]
        public void SetZoom_Number_AddTheNumberDampedToTheHeightOfTheOrbit([Values(0, 1, 2)] int orbit, [NUnit.Framework.Range(-2, 2, 0.3f)] float zoom)
        {
            //SETUP
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();
            float m_CachedOrbitHeight = freeLook.m_Orbits[orbit].m_Height;

            GameObject gameObject = new GameObject();
            freeLook.transform.SetParent(gameObject.transform);

            HoverControll hoverControll = gameObject.AddComponent<HoverControll>().Initialized() as HoverControll;
            hoverControll.cachedHeights = hoverControll.hoverCamera.m_Orbits.Select(c => c.m_Height).ToArray();
            hoverControll.cachedRadiuses = hoverControll.hoverCamera.m_Orbits.Select(c => c.m_Radius).ToArray();
            //ACT
            hoverControll.SetZoom(zoom);
            //ASSERT
            Assert.AreEqual(m_CachedOrbitHeight - (zoom * hoverControll.zoomMultiplier), hoverControll.hoverCamera.m_Orbits[orbit].m_Height);
        }
        [Test]
        public void SetZoom_Number_AddTheNumberDampedToTheRadiusOfTheOrbit([Values(0, 1, 2)] int orbit, [NUnit.Framework.Range(-2, 2, 0.3f)] float zoom)
        {
            //SETUP
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();
            float m_CachedOrbitRadius = freeLook.m_Orbits[orbit].m_Radius;

            GameObject gameObject = new GameObject();
            freeLook.transform.SetParent(gameObject.transform);

            HoverControll hoverControll = gameObject.AddComponent<HoverControll>().Initialized() as HoverControll;
            hoverControll.cachedHeights = hoverControll.hoverCamera.m_Orbits.Select(c => c.m_Height).ToArray();
            hoverControll.cachedRadiuses = hoverControll.hoverCamera.m_Orbits.Select(c => c.m_Radius).ToArray();
            //ACT
            hoverControll.SetZoom(zoom);
            //ASSERT
            Assert.AreEqual(m_CachedOrbitRadius - (zoom * hoverControll.zoomMultiplier), hoverControll.hoverCamera.m_Orbits[orbit].m_Radius);
        }
    }
}
