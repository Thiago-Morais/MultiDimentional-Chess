using NUnit.Framework;
using UnityEngine;
using Cinemachine;

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
        public void SetZoom_Minus3WithHeight2AndRadius2WhenMinZoomIs1_SetHeightTo1([Values(0, 1, 2)] int orbit)
        {
            //SETUP
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>();
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();

            hoverControll.hoverCamera = freeLook;

            freeLook.m_Orbits[orbit].m_Height = 2;
            freeLook.m_Orbits[orbit].m_Radius = 2;
            hoverControll.minZoom = 1;
            Vector2 normalizedOrbit = new Vector2(freeLook.m_Orbits[orbit].m_Height, freeLook.m_Orbits[orbit].m_Radius).normalized;
            //ACT
            hoverControll.SetZoom(-3);
            //ASSERT
            Assert.AreEqual(normalizedOrbit.x, hoverControll.hoverCamera.m_Orbits[orbit].m_Height);
        }
        [Test]
        public void SetZoom_PositiveWithPositiveHeight_IncreaseHeight([Values(0, 1, 2)] int orbit, [Values(1, 2, 3)] float positiveZoom)
        {
            //SETUP
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>();
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();

            for (int i = 0; i < freeLook.m_Orbits.Length; i++) freeLook.m_Orbits[i].m_Height = 1;
            float m_CachedHeight = freeLook.m_Orbits[orbit].m_Height;

            hoverControll.hoverCamera = freeLook;
            //ACT
            hoverControll.SetZoom(positiveZoom);
            //ASSERT
            Assert.Greater(hoverControll.hoverCamera.m_Orbits[orbit].m_Height, m_CachedHeight);
        }
        [Test]
        public void SetZoom_NegativeWithPositiveHeight_DecreaseHeight([Values(0, 1, 2)] int orbit, [Values(-1, -2, -3)] float positiveZoom)
        {
            //SETUP
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>();
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();

            for (int i = 0; i < freeLook.m_Orbits.Length; i++) freeLook.m_Orbits[i].m_Height = 1;
            float m_CachedHeight = freeLook.m_Orbits[orbit].m_Height;

            hoverControll.hoverCamera = freeLook;
            //ACT
            hoverControll.SetZoom(positiveZoom);
            //ASSERT
            Assert.Less(hoverControll.hoverCamera.m_Orbits[orbit].m_Height, m_CachedHeight);
        }
        [Test]
        public void SetZoom_PositiveWithNegativeHeight_DecreaseHeight([Values(0, 1, 2)] int orbit, [Values(1, 2, 3)] float positiveZoom)
        {
            //SETUP
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>();
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();

            for (int i = 0; i < freeLook.m_Orbits.Length; i++) freeLook.m_Orbits[i].m_Height = -1;
            float m_CachedHeight = freeLook.m_Orbits[orbit].m_Height;

            hoverControll.hoverCamera = freeLook;
            //ACT
            hoverControll.SetZoom(positiveZoom);
            //ASSERT
            Assert.Less(hoverControll.hoverCamera.m_Orbits[orbit].m_Height, m_CachedHeight);
        }
        [Test]
        public void SetZoom_NegativeWithNegativeHeight_IncreaseHeight([Values(0, 1, 2)] int orbit, [Values(-1, -2, -3)] float positiveZoom)
        {
            //SETUP
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>();
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();

            for (int i = 0; i < freeLook.m_Orbits.Length; i++) freeLook.m_Orbits[i].m_Height = -1;
            float m_CachedHeight = freeLook.m_Orbits[orbit].m_Height;

            hoverControll.hoverCamera = freeLook;
            //ACT
            hoverControll.SetZoom(positiveZoom);
            //ASSERT
            Assert.Greater(hoverControll.hoverCamera.m_Orbits[orbit].m_Height, m_CachedHeight);
        }
    }
}
