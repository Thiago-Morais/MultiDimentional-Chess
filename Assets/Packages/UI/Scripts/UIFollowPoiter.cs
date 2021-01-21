using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIFollowPoiter : MonoBehaviour
{
    public Vector2 point;
    public RectTransform uiElement;
    void Awake() { if (!uiElement) uiElement = GetComponent<RectTransform>(); }
    public void OnPoint(InputAction.CallbackContext context) => uiElement.position = context.ReadValue<Vector2>();
    public void FollowPointer() => uiElement.position = point;

}
