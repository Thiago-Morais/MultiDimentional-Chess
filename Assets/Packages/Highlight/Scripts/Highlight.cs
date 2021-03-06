﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ExtensionMethods;

public class Highlight : MonoBehaviour, IInitializable
{
    #region -------- FIELDS
    public bool lockHighlight;
    [SerializeField] bool isHighlighted;
    [SerializeField] HighlightType highlightType;
    [SerializeField] List<Material> highlightMaterials = new List<Material>();
    [SerializeField] List<Renderer> m_Renderer;
    [SerializeField] HighlightVariations highlightVariations;
    HighlightType cachedHighlightType;
    [SerializeField] List<HighlightType> highlightStack = new List<HighlightType>();
    #endregion //FIELDS

    #region -------- PROPERTIES
    public bool IsHighlighted { get => isHighlighted; private set => isHighlighted = value; }
    public HighlightVariations HighlightVariations { get => highlightVariations; private set => highlightVariations = value; }
    public HighlightType HighlightType { get => highlightType; private set => highlightType = value; }
    public HighlightType CachedHighlightType { get => cachedHighlightType; private set => cachedHighlightType = value; }
    public List<HighlightType> HighlightStack { get => highlightStack; set => highlightStack = value; }
    #endregion //PROPERTIES

    #region -------- OUTSIDE CALL
    void Awake() => InitializeVariables();
    #endregion //OUTSIDE CALL

    #region -------- METHODS
    [ContextMenu(nameof(InitializeVariables))]
    void InitializeVariables()
    {
        UpdateRenderersRef();
        UpdateMaterialsRef();
        if (!HighlightVariations) HighlightVariations = ScriptableObject.CreateInstance<HighlightVariations>();
    }
    [ContextMenu(nameof(ClearHighlight))]
    public void ClearHighlight()
    {
        highlightStack.Clear();
        HighlightOff();
        HighlightType = HighlightType.none;
    }
    public void HighlightOn(HighlightType type)
    {
        SetNewHighlightValues(type);
        HighlightOn();
    }
    [ContextMenu(nameof(UpdateHighlightValues))]
    void UpdateHighlightValues() => SetNewHighlightValues(HighlightType);
    public void SetNewHighlightValues(HighlightType type)
    {
        HighlightStack.Push(HighlightType);
        SetHighlightValues(type);
        return;
    }
    public void HighlightUndo()
    {
        SetHighlightValues(HighlightStack.Pop());
    }
    public void HighlightUndo(HighlightType type)
    {
        HighlightStack.Remove(type);
        SetHighlightValues(HighlightStack.Peek());
    }
    [ContextMenu(nameof(UpdateMaterialsRef))]
    void UpdateMaterialsRef()
    {
        highlightMaterials = new List<Material>();
        foreach (var renderer in m_Renderer)
            highlightMaterials.AddRange(renderer.materials);
    }

    void SetHighlightValues(HighlightType type)
    {
        HighlightData data = HighlightVariations.GetHighlightData(type);
        SetHighlightValues(data);
        if (type == HighlightType.none) ClearHighlight();
        HighlightType = type;
    }
    void SetHighlightValues(HighlightData data)
    {
        foreach (Material material in highlightMaterials)
        {
            material.SetFloat("HIGHLIGHT_PULSE_SPEED", data.highlightPulseSpeed);
            material.SetVector("HIGHTLIGHT_PULSE_APERTURE", data.hightlightPulseAperture);
            material.SetColor("HIGHLIGHT_COLOR", data.highlightColor);
            material.SetKeyword("FRESNEL_ON", data.useFresnel);
        }
    }
    [ContextMenu(nameof(HighlightOn))]
    public void HighlightOn()
    {
        foreach (Material material in highlightMaterials) material.EnableKeyword("HIGHLIGHT_ON");
        IsHighlighted = true;
    }
    [ContextMenu(nameof(HighlightOff))]
    public void HighlightOff()
    {
        foreach (Material material in highlightMaterials) material.DisableKeyword("HIGHLIGHT_ON");
        IsHighlighted = false;
    }
    [ContextMenu(nameof(UpdateRenderersRef))]
    public void UpdateRenderersRef() { if (m_Renderer.IsEmpty()) m_Renderer = GetComponentsInChildren<Renderer>().ToList(); }
    public IInitializable Initialized(Transform parent = null)
    {
        InitializeVariables();
        transform.parent = parent;
        return this;
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
