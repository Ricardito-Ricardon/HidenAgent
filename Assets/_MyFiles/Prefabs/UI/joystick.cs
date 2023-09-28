using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public delegate void OnInputValueChanged(Vector2 inputVal);
    public delegate void OnStickTapped();

    public event OnInputValueChanged onInputValueChanged;
    public event OnStickTapped onStickTapped;

    [SerializeField]
    RectTransform thumbStick;
    [SerializeField]
    RectTransform background;
    [SerializeField]
    float deadZone = 0.2f;
    bool wasDragging;
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 touchPos = eventData.position;
        Vector3 thumbstickLocalOffset = Vector3.ClampMagnitude(touchPos - background.position, background.sizeDelta.x / 2f);

        thumbStick.transform.localPosition = thumbstickLocalOffset;
        Vector2 outputVal = thumbstickLocalOffset / background.sizeDelta.y * 2f;
        if (thumbstickLocalOffset.magnitude > deadZone)
        {
            onInputValueChanged?.Invoke(outputVal);
        }
        wasDragging= true; 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        background.position = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        background.localPosition = Vector2.zero;
        thumbStick.localPosition = Vector2.zero;
        onInputValueChanged?.Invoke(Vector2.zero);
        if(wasDragging)
        {
            wasDragging = false;
            return;
        }
        onStickTapped?.Invoke();
    }
}