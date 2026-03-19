using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInputService : InputService
{
    private Vector2 startPos;
    private bool isDragging;
    private float dragThreshold = 20f;
    public override bool IsCameraMoving =>
        LookInputVector != Vector3.zero;

    public override Vector2 Axis =>
        SimpleInputAxis();

    public override Vector3 LookInputVector =>
        SimpleInputMouseAxis();

    public override bool IsShoot
    {
        get
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return false;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startPos = touch.position;
                        isDragging = false;
                        break;

                    case TouchPhase.Moved:
                        if (Vector2.Distance(startPos, touch.position) > dragThreshold)
                            isDragging = true;
                        break;

                    case TouchPhase.Ended:
                        if (!isDragging)
                        {
                            ShootPosition = touch.position;
                            return true;
                        }
                        break;
                }
            }

            return false;
        }
    }
}
