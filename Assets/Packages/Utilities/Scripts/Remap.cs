using System;
using ExtensionMethods;
using UnityEngine;

[Serializable]
public class RemapHandler
{
    public Vector2 baseRange;
    public Vector2 newRange;

    public void SetBaseRange(Vector2 baseRange) => this.baseRange = baseRange;
    public void SetNewRange(Vector2 newRange) => this.newRange = newRange;
    public float Remap(float value) => Remap(value, baseRange, newRange);
    public float Remap(float value, Vector2 baseRange, Vector2 newRange)
    {
        float inverseLerp = value.InverseLerpUnclamped(baseRange.x, baseRange.y);
        return Mathf.LerpUnclamped(newRange.x, newRange.y, inverseLerp);
    }
}