using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ExtensionMethods;

public class Highlight : MonoBehaviour
{
    #region -------- FIELDS
    public bool lockHighlight;
    [SerializeField] bool isHighlighted;
    [SerializeField] HighlightType m_CurrentHighlight;
    [SerializeField] List<SampleHighlight> m_HighlightVariations = new List<SampleHighlight> { new SampleHighlight() };
    [SerializeField] List<Material> highlightMaterials = new List<Material>();
    [SerializeField] List<Renderer> m_Renderer;
    HighlightType m_CacheHighlight;
    // public HighlightVariations defaultHighlights;

    #endregion //FIELDS

    #region -------- PROPERTIES
    public bool IsHighlighted { get => isHighlighted; private set => isHighlighted = value; }
    #endregion //PROPERTIES

    void Awake() => InitializeVariables();
    [ContextMenu(nameof(InitializeVariables))]
    void InitializeVariables()
    {
        UpdateRenderersRef();
        UpdateMaterialsRef();
    }

    #region -------- METHODS
    [ContextMenu(nameof(HighlightOn))]
    public void HighlightOn() => SetHighlightOn(true);
    [ContextMenu(nameof(HighlightOff))]
    // public void HighlightOff() => SetHighlightOn(false);
    public void HighlightOff()
    {
        SetHighlightOn(false);
        m_CurrentHighlight = HighlightType.none;
    }
    [ContextMenu(nameof(UpdateHighlightValues))]
    public void UpdateHighlightValues() => TrySetHighlightValues(m_CurrentHighlight);
    public void HighlightUndo() { if (!TrySetHighlightValues(m_CacheHighlight)) HighlightOff(); }
    [ContextMenu(nameof(UpdateRenderersRef))]
    public void UpdateRenderersRef() { if (m_Renderer.IsEmpty()) m_Renderer = GetComponentsInChildren<Renderer>().ToList(); }
    [ContextMenu(nameof(UpdateMaterialsRef))]
    void UpdateMaterialsRef()
    {
        highlightMaterials = new List<Material>();
        foreach (var renderer in m_Renderer)
            highlightMaterials.AddRange(renderer.materials);
    }
    public void HighlightOn(HighlightType highlightType)
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
        IsHighlighted = shouldHighlight;
    }
    public bool TrySetHighlightValues(HighlightType highlightType)
    {
        if (highlightType == HighlightType.none)
        {
            HighlightOff();
            return true;
        }

        SampleHighlight sampleHighlight = m_HighlightVariations.FirstOrDefault(c => c.type == highlightType);
        if (sampleHighlight == null)
        {
            // Debug.LogError($"Highlight type {highlightType} not found", gameObject);
            // sampleHighlight = defaultHighlights.GetHighlightData(highlightType);
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
        m_CacheHighlight = m_CurrentHighlight;
        m_CurrentHighlight = target.type;
    }
    #endregion //METHODS
}
public enum HighlightType
{
    none = 0,
    hover = 1 << 0,
    selected = 1 << 1,
    error = 1 << 2,
    atention = 1 << 3,
    movable = 1 << 4,
    capturable = 1 << 5
}
