using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InputTestUI : MonoBehaviour
{
    [Header("TextMeshProUGUI reference")]
    public TextMeshProUGUI pointUGUI;
    public TextMeshProUGUI clickUGUI;
    [Header("Gradient")]
    public VertexGradient gradientStarted;
    public VertexGradient gradientPerformed;
    public VertexGradient gradientCanceled;
    public VertexGradient gradientWaiting;
    public VertexGradient gradientDisabled;
    #region -------- LEFT PANEL
    public void OnPoint(InputAction.CallbackContext context)
    {
        Vector2 point = context.ReadValue<Vector2>();
        SetText(point, pointUGUI, context);
    }
    public void OnClick(InputAction.CallbackContext context) => SetText(clickUGUI, context);
    #endregion //LEFT PANEL
    void SetText(TextMeshProUGUI text, InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started: text.colorGradient = gradientStarted; break;
            case InputActionPhase.Performed: text.colorGradient = gradientPerformed; break;
            case InputActionPhase.Canceled: text.colorGradient = gradientCanceled; break;
            case InputActionPhase.Waiting: text.colorGradient = gradientWaiting; break;
            case InputActionPhase.Disabled: text.colorGradient = gradientDisabled; break;
        }

        text.SetText(
            $"{text.name}\n" +
            "phase = " + context.phase);
    }
    void SetText<T>(T value, TextMeshProUGUI text, InputAction.CallbackContext context) where T : struct
    {
        switch (context.phase)
        {
            case InputActionPhase.Started: text.colorGradient = gradientStarted; break;
            case InputActionPhase.Performed: text.colorGradient = gradientPerformed; break;
            case InputActionPhase.Canceled: text.colorGradient = gradientCanceled; break;
            case InputActionPhase.Waiting: text.colorGradient = gradientWaiting; break;
            case InputActionPhase.Disabled: text.colorGradient = gradientDisabled; break;
        }

        text.SetText(
            $"{text.name} = \n" +
            value);
    }
}
