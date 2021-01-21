using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InputTestGlobal : MonoBehaviour
{
    [Header("TextMeshProUGUI reference")]
    public TextMeshProUGUI navigateUGUI;
    public TextMeshProUGUI submitUGUI;
    public TextMeshProUGUI cancelUGUI;
    public TextMeshProUGUI pointUGUI;
    public TextMeshProUGUI clickUGUI;
    public TextMeshProUGUI scrollWheelUGUI;
    public TextMeshProUGUI middleClickUGUI;
    public TextMeshProUGUI rightClickUGUI;
    public TextMeshProUGUI trackedDevicePositionUGUI;
    public TextMeshProUGUI trackedDeviceOrientationUGUI;
    public TextMeshProUGUI startPointUGUI;
    public TextMeshProUGUI hoverUGUI;
    public TextMeshProUGUI scrollUGUI;
    public TextMeshProUGUI firstPointPositionUGUI;
    public TextMeshProUGUI firstPointDeltaUGUI;
    public TextMeshProUGUI secondPointPositionUGUI;
    public TextMeshProUGUI secondPointDeltaUGUI;
    [Header("Gradient")]
    public VertexGradient gradientStarted;
    public VertexGradient gradientPerformed;
    public VertexGradient gradientCanceled;
    public VertexGradient gradientWaiting;
    public VertexGradient gradientDisabled;
    #region -------- LEFT PANEL
    public void OnNavigate(InputAction.CallbackContext context) => SetText(context.ReadValue<Vector2>(), navigateUGUI, context);
    public void OnSubmit(InputAction.CallbackContext context) => SetText(submitUGUI, context);
    public void OnCancel(InputAction.CallbackContext context) => SetText(cancelUGUI, context);
    public void OnPoint(InputAction.CallbackContext context) => SetText(context.ReadValue<Vector2>(), pointUGUI, context);
    public void OnClick(InputAction.CallbackContext context) => SetText(clickUGUI, context);
    public void OnScrollWheel(InputAction.CallbackContext context) => SetText(context.ReadValue<Vector2>(), scrollWheelUGUI, context);
    public void OnMiddleClick(InputAction.CallbackContext context) => SetText(middleClickUGUI, context);
    public void OnRightClick(InputAction.CallbackContext context) => SetText(rightClickUGUI, context);
    public void OnTrackedDevicePosition(InputAction.CallbackContext context) => SetText(context.ReadValue<Vector3>(), trackedDevicePositionUGUI, context);
    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) => SetText(context.ReadValue<Quaternion>(), trackedDeviceOrientationUGUI, context);
    public void OnStartPoint(InputAction.CallbackContext context) => SetText(context.ReadValue<Vector2>(), startPointUGUI, context);
    public void OnHover(InputAction.CallbackContext context) => SetText(hoverUGUI, context);
    public void OnScroll(InputAction.CallbackContext context) => SetText(context.ReadValue<float>(), scrollUGUI, context);
    public void OnFirstPointPosition(InputAction.CallbackContext context) => SetText(context.ReadValue<Vector2>(), firstPointPositionUGUI, context);
    public void OnFirstPointDelta(InputAction.CallbackContext context) => SetText(context.ReadValue<Vector2>(), firstPointDeltaUGUI, context);
    public void OnSecondPointPosition(InputAction.CallbackContext context) => SetText(context.ReadValue<Vector2>(), secondPointPositionUGUI, context);
    public void OnSecondPointDelta(InputAction.CallbackContext context) => SetText(context.ReadValue<Vector2>(), secondPointDeltaUGUI, context);
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
