using NUnit.Framework;
using UnityEngine;

public class GameManagerTests
{
    [Test]
    public void UpdateHoverSensibility__SetHoverSensitivityToGameSettingsHoverSensitivity([Values(0, 1, -1, 1.5f, 500)] float hoverSensitivity)
    {
        //SETUP
        GameManager manager = new GameObject(nameof(manager)).AddComponent<GameManager>().Initialized() as GameManager;

        GameSettings settings = ScriptableObject.CreateInstance<GameSettings>();
        settings.hoverSensitivity = hoverSensitivity;
        HoverControll hoverControll = new GameObject(nameof(hoverControll)).AddComponent<HoverControll>().Initialized() as HoverControll;
        manager.settings = settings;
        manager.hoverControll = hoverControll;
        //ACT
        manager.UpdateHoverSensitivity();
        //ASSERT
        Assert.AreEqual(manager.settings.hoverSensitivity, manager.hoverControll.HoverSensitivity);
    }
    [Test]
    public void SetHoverSensibility_FloatValue_SetGameSettingsHoverSensitivityToValue([Values(0, 1, -1, 1.5f, 500)] float hoverSensitivity)
    {
        //SETUP
        GameManager manager = new GameObject(nameof(manager)).AddComponent<GameManager>().Initialized() as GameManager;
        //ACT
        manager.SetHoverSensitivity(hoverSensitivity);
        //ASSERT
        Assert.AreEqual(hoverSensitivity, manager.settings.hoverSensitivity);
    }
    [Test]
    public void SetHoverSensibility_FloatValue_SetHoverControllHoverSensitivityToValue([Values(0, 1, -1, 1.5f, 500)] float hoverSensitivity)
    {
        //SETUP
        GameManager manager = new GameObject(nameof(manager)).AddComponent<GameManager>().Initialized() as GameManager;
        //ACT
        manager.SetHoverSensitivity(hoverSensitivity);
        //ASSERT
        Assert.AreEqual(hoverSensitivity, manager.hoverControll.HoverSensitivity);
    }
}