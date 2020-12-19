using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] bool isHighlighted;
    public bool IsHighlighted { get => isHighlighted; private set => isHighlighted = value; }
    [SerializeField] new List<Renderer> renderer;
    [SerializeField] Material highlightMaterial;
    Material originalMaterial;
    void Awake() => InitializeVariables();
    [ContextMenu(nameof(InitializeVariables))]
    void InitializeVariables()
    {
        if (!originalMaterial) originalMaterial = GetComponentInChildren<Renderer>().sharedMaterial;
        if (renderer == null || renderer.Count == 0) renderer = GetComponentsInChildren<Renderer>().ToList();
        else
        {
            bool HasAnyRenderer = renderer.Any(rend => rend != null);
            if (!HasAnyRenderer) renderer = GetComponentsInChildren<Renderer>().ToList();
        }
        IsHighlighted = !renderer.Any(mat => mat.sharedMaterial != highlightMaterial);
    }
    public void SetHighlight(bool shouldHighlight)
    {
        if (shouldHighlight == IsHighlighted) return;
        if (shouldHighlight)
        {
            renderer.ForEach(rend => rend.sharedMaterial = highlightMaterial);
            IsHighlighted = true;
        }
        else
        {
            renderer.ForEach(rend => rend.sharedMaterial = originalMaterial);
            IsHighlighted = false;
        }
    }
    [ContextMenu(nameof(HighlightOn))]
    void HighlightOn() => SetHighlight(true);
    [ContextMenu(nameof(HighlightOff))]
    void HighlightOff() => SetHighlight(false);
}
