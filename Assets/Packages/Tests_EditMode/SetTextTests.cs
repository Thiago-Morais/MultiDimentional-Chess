using NUnit.Framework;
using TMPro;
using UnityEngine;

public class SetTextTests
{
    [Test]
    public void SetFloat_FloatValue_SetTextReferenceToValue([Values(0, 1, 1.5f, -1)] float value)
    {
        //SETUP
        float expected = value;
        SetText setText = new GameObject(nameof(setText)).AddComponent<SetText>().Initialized() as SetText;
        setText.text = new GameObject().AddComponent<TextMeshProUGUI>();
        //ACT
        setText.SetFloat(value);
        //ASSERT
        Assert.AreEqual(expected.ToString("F2"), setText.text.text);
    }
}
