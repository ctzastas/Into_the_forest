using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 *  Extends the UnityEngine.UI.Button class so that we can override
 *  some of the base functions to allow for greater controll
*/
public class JumpButtonScript : Button
{
    bool isPressed = false;
    private void Update()
    {
        if (isPressed)
        {
            onClick.Invoke();
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {

        isPressed = true;
        isPressed = false;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        isPressed = false;
        base.OnPointerClick(eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        base.OnPointerUp(eventData);
    }
}
