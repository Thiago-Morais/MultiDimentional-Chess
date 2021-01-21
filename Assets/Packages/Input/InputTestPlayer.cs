using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InputTestPlayer : MonoBehaviour
{
    [Header("TextMeshProUGUI reference")]
    public TextMeshProUGUI rightClick;
    public TextMeshProUGUI scrollWheel;
    public TextMeshProUGUI middleClick;
    public TextMeshProUGUI navigate;
    public TextMeshProUGUI submit;
    public TextMeshProUGUI click;
    public TextMeshProUGUI trackedDevicePosition;
    public TextMeshProUGUI cancel;
    public TextMeshProUGUI point;
    public TextMeshProUGUI trackedDeviceOrientation;
    public TextMeshProUGUI startPoint;
    [Header("RightPanel")]
    public TextMeshProUGUI look;
    public TextMeshProUGUI scroll;
    public TextMeshProUGUI hover;
    public TextMeshProUGUI secondPointPositionUGUI;
    public TextMeshProUGUI secondPointDeltaUGUI;
    public TextMeshProUGUI firstPointPositionUGUI;
    public TextMeshProUGUI touchZoomUGUI;
    public TextMeshProUGUI firstPointDeltaUGUI;
    [Header("Relations")]
    public TextMeshProUGUI fingerDistance;
    public TextMeshProUGUI inputOrder;
    [Header("Gradient")]
    public VertexGradient gradientStarted;
    public VertexGradient gradientPerformed;
    public VertexGradient gradientCanceled;
    public VertexGradient gradientWaiting;
    public VertexGradient gradientDisabled;
    #region -------- LEFT PANEL
    Vector2 firstPointPosition;
    Vector2 firstPointDelta;
    Vector2 secondPointPosition;
    Vector2 secondPointDelta;
    public void OnRightClick(InputAction.CallbackContext context) => SetText(rightClick, context);
    public void OnScrollWheel(InputAction.CallbackContext context) => SetText(scrollWheel, context);
    public void OnMiddleClick(InputAction.CallbackContext context) => SetText(middleClick, context);
    public void OnNavigate(InputAction.CallbackContext context) => SetText(navigate, context);
    public void OnSubmit(InputAction.CallbackContext context) => SetText(submit, context);
    public void OnClick(InputAction.CallbackContext context) => SetText(click, context);
    public void OnTrackedDevicePosition(InputAction.CallbackContext context) => SetText(trackedDevicePosition, context);
    public void OnCancel(InputAction.CallbackContext context) => SetText(cancel, context);
    public void OnPoint(InputAction.CallbackContext context) => SetText(point, context);
    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) => SetText(trackedDeviceOrientation, context);
    public void OnStartPoint(InputAction.CallbackContext context) => SetText(startPoint, context);
    #endregion //LEFT PANEL
    #region -------- RIGHT PANEL
    public void OnLook(InputAction.CallbackContext context) => SetText(context.ReadValue<Vector2>(), look, context);
    public void OnScroll(InputAction.CallbackContext context) => SetText(context.ReadValue<float>(), scroll, context);
    public void OnHover(InputAction.CallbackContext context) => SetText(hover, context);
    public void OnSecondPointPosition(InputAction.CallbackContext context)
    {
        secondPointPosition = context.ReadValue<Vector2>();
        SetText(secondPointPosition, secondPointPositionUGUI, context);
    }
    public void OnSecondPointDelta(InputAction.CallbackContext context)
    {
        secondPointDelta = context.ReadValue<Vector2>();
        SetText(secondPointDelta, secondPointDeltaUGUI, context);
    }
    public void OnFirstPointPosition(InputAction.CallbackContext context)
    {
        firstPointPosition = context.ReadValue<Vector2>();
        SetText(firstPointPosition, firstPointPositionUGUI, context);
    }
    public void OnFirstPointDelta(InputAction.CallbackContext context)
    {
        firstPointDelta = context.ReadValue<Vector2>();
        SetText(firstPointDelta, firstPointDeltaUGUI, context);
    }
    public void OnTouchZoom(InputAction.CallbackContext context)
    {
        SetText(touchZoomUGUI, context);
        SetFingerDistance();
    }
    #endregion //RIGHT PANEL
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
    void SetFingerDistance()
    {
        float distance = Vector2.Distance(firstPointPosition, secondPointPosition);
        float prefiousDistance = Vector2.Distance(firstPointPosition - firstPointDelta, secondPointPosition - secondPointDelta);
        float deltaDistance = distance - prefiousDistance;
        fingerDistance.SetText(
            $@"Finger Distance  = 
            {distance.ToString("f5")}
            Previous Distance  = 
            {prefiousDistance.ToString("f5")}
            Delta Distance  = 
            {deltaDistance.ToString("f5")}"
            );

    }
}
