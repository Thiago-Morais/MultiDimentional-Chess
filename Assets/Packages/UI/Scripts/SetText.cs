using System;
using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour, IInitializable
{
    public TextMeshProUGUI text;
    [Header("Float")]
    public bool roundToInt;
    public RemapHandler floatRemapper;
    public IInitializable Initialized(Transform parent = null)
    {
        transform.SetParent(parent);
        return this;
    }
    public void SetFloat(float value)
    {
        float remapped = floatRemapper.Remap(value);

        if (roundToInt) remapped = Mathf.RoundToInt(remapped);

        text.text = remapped.ToString("F2");
    }
}
