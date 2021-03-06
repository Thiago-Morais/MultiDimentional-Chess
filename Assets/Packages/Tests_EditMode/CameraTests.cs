﻿using NUnit.Framework;
using UnityEngine;
using Cinemachine;
using System.Reflection;

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
        #region -------- RESET_FREE_LOOK_AXIS
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

            hoverControll.initialXAxisName = freeLook.m_XAxis.m_InputAxisName;
            hoverControll.initialYAxisName = freeLook.m_YAxis.m_InputAxisName;

            freeLook.m_XAxis.m_InputAxisName = "";
            freeLook.m_XAxis.m_InputAxisValue = 0;
            //ACT
            hoverControll.ResetFreeLookAxis();
            //ASSERT
            Assert.AreEqual(hoverControll.initialXAxisName, hoverControll.hoverCamera.m_XAxis.m_InputAxisName);
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

            hoverControll.initialXAxisName = freeLook.m_XAxis.m_InputAxisName;
            hoverControll.initialYAxisName = freeLook.m_YAxis.m_InputAxisName;

            freeLook.m_YAxis.m_InputAxisName = "";
            freeLook.m_YAxis.m_InputAxisValue = 0;
            //ACT
            hoverControll.ResetFreeLookAxis();
            //ASSERT
            Assert.AreEqual(hoverControll.initialYAxisName, hoverControll.hoverCamera.m_YAxis.m_InputAxisName);
        }
        #endregion //RESET_FREE_LOOK_AXIS
        #region -------- REMOVE_FREE_LOOK_AXIS
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
        #endregion //REMOVE_FREE_LOOK_AXIS
        #region -------- SET_ZOOM
        [Test]
        public void AddScaledDeltaZoom_Minus3WithHeight2AndRadius2WhenMinZoomIs1_SetHeightTo1([Values(0, 1, 2)] int orbit)
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
            hoverControll.AddScaledDeltaZoom(-3);
            //ASSERT
            Assert.AreEqual(normalizedOrbit.x, hoverControll.hoverCamera.m_Orbits[orbit].m_Height);
        }
        [Test]
        public void AddScaledDeltaZoom_PositiveWithPositiveHeight_IncreaseHeight([Values(0, 1, 2)] int orbit, [Values(1, 2, 3)] float positiveZoom)
        {
            //SETUP
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>();
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();

            for (int i = 0; i < freeLook.m_Orbits.Length; i++) freeLook.m_Orbits[i].m_Height = 1;
            float m_CachedHeight = freeLook.m_Orbits[orbit].m_Height;

            hoverControll.hoverCamera = freeLook;
            //ACT
            hoverControll.AddScaledDeltaZoom(positiveZoom);
            //ASSERT
            Assert.Greater(hoverControll.hoverCamera.m_Orbits[orbit].m_Height, m_CachedHeight);
        }
        [Test]
        public void AddScaledDeltaZoom_NegativeWithPositiveHeight_DecreaseHeight([Values(0, 1, 2)] int orbit, [Values(-1, -2, -3)] float positiveZoom)
        {
            //SETUP
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>();
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();

            for (int i = 0; i < freeLook.m_Orbits.Length; i++) freeLook.m_Orbits[i].m_Height = 1;
            float m_CachedHeight = freeLook.m_Orbits[orbit].m_Height;

            hoverControll.hoverCamera = freeLook;
            //ACT
            hoverControll.AddScaledDeltaZoom(positiveZoom);
            //ASSERT
            Assert.Less(hoverControll.hoverCamera.m_Orbits[orbit].m_Height, m_CachedHeight);
        }
        [Test]
        public void AddScaledDeltaZoom_PositiveWithNegativeHeight_DecreaseHeight([Values(0, 1, 2)] int orbit, [Values(1, 2, 3)] float positiveZoom)
        {
            //SETUP
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>();
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();

            for (int i = 0; i < freeLook.m_Orbits.Length; i++) freeLook.m_Orbits[i].m_Height = -1;
            float m_CachedHeight = freeLook.m_Orbits[orbit].m_Height;

            hoverControll.hoverCamera = freeLook;
            //ACT
            hoverControll.AddScaledDeltaZoom(positiveZoom);
            //ASSERT
            Assert.Less(hoverControll.hoverCamera.m_Orbits[orbit].m_Height, m_CachedHeight);
        }
        [Test]
        public void AddScaledDeltaZoom_NegativeWithNegativeHeight_IncreaseHeight([Values(0, 1, 2)] int orbit, [Values(-1, -2, -3)] float positiveZoom)
        {
            //SETUP
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>();
            CinemachineFreeLook freeLook = new GameObject(nameof(freeLook)).AddComponent<CinemachineFreeLook>();

            for (int i = 0; i < freeLook.m_Orbits.Length; i++) freeLook.m_Orbits[i].m_Height = -1;
            float m_CachedHeight = freeLook.m_Orbits[orbit].m_Height;

            hoverControll.hoverCamera = freeLook;
            //ACT
            hoverControll.AddScaledDeltaZoom(positiveZoom);
            //ASSERT
            Assert.Greater(hoverControll.hoverCamera.m_Orbits[orbit].m_Height, m_CachedHeight);
        }
        #endregion //SET_ZOOM
        #region -------- SET_HOVER_SENSITIVITY
        [Test]
        public void SetHoverSensitivity_AnyFloat_SetHoverSpeedToOriginalTimesMultiplier([NUnit.Framework.Range(-3, 3, 1.5f)] float multiplier)
        {
            //SETUP
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>().Initialized() as HoverControll;
            Vector2 expected = new Vector2(hoverControll.hoverCamera.m_XAxis.m_MaxSpeed, hoverControll.hoverCamera.m_YAxis.m_MaxSpeed) * multiplier;
            //ACT
            hoverControll.HoverSensitivity = multiplier;
            //ASSERT
            Assert.AreEqual(expected, hoverControll.GetHoverSpeed());
        }
        [Test]
        public void SetHoverSensitivity_HasCalledBefore_SetHoverSpeedToOriginalTimesMultiplier([Values(0, 1, -1, 1.5f, 500)] float multiplier)
        {
            //SETUP
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>().Initialized() as HoverControll;
            Vector2 expected = new Vector2(hoverControll.hoverCamera.m_XAxis.m_MaxSpeed, hoverControll.hoverCamera.m_YAxis.m_MaxSpeed) * multiplier;
            //ACT
            hoverControll.HoverSensitivity = multiplier;
            hoverControll.HoverSensitivity = multiplier;
            //ASSERT
            Assert.AreEqual(expected, hoverControll.GetHoverSpeed());
        }
        #endregion //SET_HOVER_SENSITIVITY
        #region -------- APPLY_HOVER_SENSITIVITY
        [Test]
        public void ApplyHoverSensitivity_HoverSensitivity_SetHoverSpeedToInitialSpeedTimesSensitivity([Values(0, 1, -1, 1.5f, 500)] float sensitivity)
        {
            //SETUP
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>().Initialized() as HoverControll;
            Vector2 expected = new Vector2(hoverControll.hoverCamera.m_XAxis.m_MaxSpeed, hoverControll.hoverCamera.m_YAxis.m_MaxSpeed) * sensitivity;

            FieldInfo hoverSensitivity = hoverControll.GetType().GetField("hoverSensitivity", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            hoverSensitivity.SetValue(hoverControll, sensitivity);
            //ACT
            hoverControll.ApplyHoverSensitivity();
            //ASSERT
            Assert.AreEqual(expected, hoverControll.GetHoverSpeed());
        }
        #endregion //APPLY_HOVER_SENSITIVITY
        #region -------- HOVER_SPEED
        [Test]
        public void GetHoverSpeed__ReturnHoverCameraAxisControlXnYSpeed(
            [Values(0, 1, 2)] float xSpeed,
            [Values(-1, 0, 1.5f)] float ySpeed)
        {
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>().Initialized() as HoverControll;
            hoverControll.hoverCamera.m_XAxis.m_MaxSpeed = xSpeed;
            hoverControll.hoverCamera.m_YAxis.m_MaxSpeed = ySpeed;

            //SETUP
            Vector2 expected = new Vector2(hoverControll.hoverCamera.m_XAxis.m_MaxSpeed, hoverControll.hoverCamera.m_YAxis.m_MaxSpeed);
            //ACT
            Vector2 hoverSpeed = hoverControll.GetHoverSpeed();
            //ASSERT
            Assert.AreEqual(expected, hoverSpeed);
        }
        [Test]
        public void SetHoverSpeed__ReturnHoverCameraAxisControlXnYSpeed(
            [Values(0, 1, 2)] float xSpeed,
            [Values(-1, 0, 1.5f)] float ySpeed)
        {
            //SETUP
            HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>().Initialized() as HoverControll;

            Vector2 expected = new Vector2(xSpeed, ySpeed);
            //ACT
            hoverControll.SetHoverSpeed(expected);
            Vector2 hoverSpeed = new Vector2(hoverControll.hoverCamera.m_XAxis.m_MaxSpeed, hoverControll.hoverCamera.m_YAxis.m_MaxSpeed);
            //ASSERT
            Assert.AreEqual(expected, hoverSpeed);
        }
        #endregion //HOVER_SPEED
    }
}
