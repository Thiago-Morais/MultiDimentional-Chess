using System;
using NUnit.Framework;
using UnityEngine;

public class RemapHandlerTests
{
    static Vector2[] Vector2Values => new[]
    {
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(0, 1),
        new Vector2(-1, -1),
        new Vector2(1, -1),
    };
    [TestCaseSource(nameof(Vector2Values))]
    [Test]
    public void SetBaseRange_PassedVector2_SetBaseRangeToVector2(Vector2 baseRange)
    {
        //SETUP
        RemapHandler remapHandler = new RemapHandler();
        //ACT
        remapHandler.SetBaseRange(baseRange);
        //ASSERT
        Assert.AreEqual(baseRange, remapHandler.baseRange);
    }
    [TestCaseSource(nameof(Vector2Values))]
    [Test]
    public void SetNewRange_PassedVector2_SetNewRangeToVector2(Vector2 newRange)
    {
        //SETUP
        RemapHandler remapHandler = new RemapHandler();
        //ACT
        remapHandler.SetNewRange(newRange);
        //ASSERT
        Assert.AreEqual(newRange, remapHandler.newRange);
    }
    static object[] RemapCases => new object[]
    {
        new object[] { 1, new Vector2(0, 1), new Vector2(0, 10), 10 },
        new object[] { 2, new Vector2(0, 1), new Vector2(0, 10), 20 },
        new object[] { 2, new Vector2(0, 2), new Vector2(0, 10), 10 },
    };
    [TestCaseSource(nameof(RemapCases))]
    [Test]
    public void Remap_AccendentRangesWithDifferentXnY_RemapValueFromBaseRangeToNewRange(float value, Vector2 baseRange, Vector2 newRange, float expected)
    {
        //SETUP
        RemapHandler remapHandler = new RemapHandler();
        //ACT
        float remapped = remapHandler.Remap(value, baseRange, newRange);
        //ASSERT
        Assert.AreEqual(expected, remapped);
    }
}