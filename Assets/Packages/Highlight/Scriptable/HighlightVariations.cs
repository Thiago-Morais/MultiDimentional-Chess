using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = nameof(ScriptableObject) + "/" + nameof(HighlightVariations))]
public class HighlightVariations : ScriptableObject
{
    public List<HighlightData> highlightVariations = new List<HighlightData>(new HighlightData[1]);
    public HighlightData GetHighlightData(HighlightType highlightType)
    {
        HighlightData highlightData = highlightVariations.FirstOrDefault(h => h.type == highlightType);
        if (highlightData == default(HighlightData))
        {
            highlightData = new HighlightData(highlightType);
            highlightVariations.Add(highlightData);
        }
        return highlightData;
    }
}