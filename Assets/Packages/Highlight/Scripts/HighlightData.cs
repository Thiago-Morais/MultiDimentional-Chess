using UnityEngine;
using System;

[Serializable]
public class HighlightData
{
    public HighlightType type = HighlightType.none;
    [ColorUsageAttribute(true, true)]
    public Color highlightColor = (Color.green + (Color.blue / 10)) * Mathf.Pow(2, 5);
    public float highlightPulseSpeed = 3;
    public Vector2 hightlightPulseAperture = new Vector2(3, 5);
    public bool useFresnel = true;
    public HighlightData(HighlightType highlightType) => type = highlightType;
}