using System;
using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour, IInitializable
{
    public TextMeshProUGUI text;

    public IInitializable Initialized(Transform parent = null)
    {
        transform.SetParent(parent);
        return this;
    }

    public void SetFloat(float value)
    {
        text.text = value.ToString("F2");
    }
}
