using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveButtonScript : Button
{
    bool isPressed = false;
    private void LateUpdate()
    {
        if (isPressed) 
        {
            onClick.Invoke();
        }
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        base.OnPointerDown(eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        base.OnPointerUp(eventData);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        isPressed = false;
        base.OnPointerExit(eventData);
    }
}
