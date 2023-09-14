using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public delegate void OnInputValueChanged(Vector2 inputVal);

    public event OnInputValueChanged onInputValueChanged;

    [SerializeField]
    RectTransform thumbStick;
    [SerializeField]
    RectTransform background;
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 touchPos = eventData.position;
        Vector3 thumbstickLocalOffset = Vector3.ClampMagnitude(touchPos - background.position, background.sizeDelta.x / 2f);

        thumbStick.transform.localPosition = thumbstickLocalOffset;
        onInputValueChanged.Invoke(thumbstickLocalOffset / background.sizeDelta.y * 2);
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
    }
}