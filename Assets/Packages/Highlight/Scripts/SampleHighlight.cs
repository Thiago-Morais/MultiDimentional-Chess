using UnityEngine;
using System;

[Serializable]
public class SampleHighlight
{
    public HighlightType type = HighlightType.selected;
    [ColorUsageAttribute(true, true)]
    public Color highlightColor = (Color.green + Color.blue / 10) * 10;
    public float highlightPulseSpeed = 3;
    public Vector2 hightlightPulseAperture = new Vector2(3, 5);
}
[Serializable]
public class SampleData
{
    public HighlightType type = HighlightType.selected;
    [ColorUsageAttribute(true, true)]
    public Color highlightColor = (Color.green + Color.blue / 10) * 10;
    public float highlightPulseSpeed = 3;
    public Vector2 hightlightPulseAperture = new Vector2(3, 5);
}