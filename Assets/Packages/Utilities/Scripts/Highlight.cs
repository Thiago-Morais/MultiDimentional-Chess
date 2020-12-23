using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ExtensionMethods;
using System;

public class Highlight : MonoBehaviour
{
    #region -------- FIELDS
    public bool lockHighlight;
    [SerializeField] bool m_IsHighlighted;
    [SerializeField] HighlightType currentHighlight;
    [SerializeField] List<SampleHighlight> highlightVariations = new List<SampleHighlight> { new SampleHighlight() };
    [Serializable]
    public class SampleHighlight
    {
        public HighlightType type = HighlightType.selected;
        [ColorUsageAttribute(true, true)]
        public Color highlightColor = (Color.green + Color.blue / 10) * 10;
        public float highlightPulseSpeed = 3;
        public Vector2 hightlightPulseAperture = new Vector2(3, 5);
    }
    [SerializeField] List<Material> highlightMaterials = new List<Material>();
    [SerializeField] List<Renderer> m_Renderer;
    #endregion //FIELDS

    #region -------- PROPERTIES
    #endregion //PROPERTIES

    void Awake() => InitializeVariables();
    [ContextMenu(nameof(InitializeVariables))]
    void InitializeVariables()
    {
        UpdateRenderers();
        UpdateMaterials();
    }

    #region -------- METHODS
    [ContextMenu(nameof(HighlightOn))]
    public void HighlightOn() => SetHighlightOn(true);
    [ContextMenu(nameof(HighlightOff))]
    public void HighlightOff() => SetHighlightOn(false);
    [ContextMenu(nameof(UpdateHighlightValues))]
    public void UpdateHighlightValues() => TrySetHighlightValues(currentHighlight);
    [ContextMenu(nameof(UpdateRenderers))]
    public void UpdateRenderers() { if (m_Renderer.IsEmpty()) m_Renderer = GetComponentsInChildren<Renderer>().ToList(); }
    [ContextMenu(nameof(UpdateMaterials))]
    void UpdateMaterials()
    {
        highlightMaterials = new List<Material>();
        foreach (var renderer in m_Renderer)
            highlightMaterials.AddRange(renderer.materials);
    }
    public void SetHighlightOnUsing(HighlightType highlightType)
    {
        TrySetHighlightValues(highlightType);
        SetHighlightOn(true);
    }
    public void SetHighlightOn(bool shouldHighlight)
    {
        foreach (Material material in highlightMaterials)
        {
            if (shouldHighlight)
                material.EnableKeyword("HIGHLIGHT_ON");
            else
                material.DisableKeyword("HIGHLIGHT_ON");
        }
        m_IsHighlighted = shouldHighlight;
    }
    public bool TrySetHighlightValues(HighlightType highlightType)
    {
        SampleHighlight sampleHighlight = highlightVariations.FirstOrDefault(c => c.type == highlightType);
        if (sampleHighlight == null)
        {
            Debug.LogError($"Highlight type not found", gameObject);
            return false;
        }
        SetHighlightValues(sampleHighlight);
        return true;
    }
    void SetHighlightValues(SampleHighlight target)
    {
        foreach (Material material in highlightMaterials)
        {
            material.SetFloat("HIGHLIGHT_PULSE_SPEED", target.highlightPulseSpeed);
            material.SetVector("HIGHTLIGHT_PULSE_APERTURE", target.hightlightPulseAperture);
            material.SetColor("HIGHLIGHT_COLOR", target.highlightColor);
        }
        currentHighlight = target.type;
    }
    #endregion //METHODS

    #region -------- ENUM
    public enum HighlightType { none, hover, selected, error, atention }
    #endregion //ENUM
}