using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInputService : InputService
{
    private const string InteractButton = "Interact";
    private const string JumpButton = "Jump";

    private int _shootFingerId = -1;
    private float _lastPinchDistance;
    private bool _wasDragging;

    public override bool IsCameraMoving =>
        LookInputVector != Vector3.zero;

    public override Vector2 Axis =>
        SimpleInputAxis();

    public override Vector3 LookInputVector
    {
        get
        {
            if (Input.touchCount == 0)
                return Vector3.zero;

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    continue;

                if (touch.phase == TouchPhase.Moved)
                {
                    return new Vector3(
                        touch.deltaPosition.x,
                        touch.deltaPosition.y,
                        0f
                    );
                }
            }

            return Vector3.zero;
        }
    }

    public override bool IsShoot
    {
        get
        {
            if (Input.touchCount == 0)
            {
                _shootFingerId = -1;
                _wasDragging = false;
                return false;
            }

            bool isShooting = false;

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    continue;                

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        if (_shootFingerId == -1)
                        {
                            _shootFingerId = touch.fingerId;
                            _wasDragging = false;
                            isShooting = false;
                        }
                        break;

                    case TouchPhase.Moved:
                        if (touch.fingerId == _shootFingerId)
                        {
                            _wasDragging = true;
                            isShooting = false;
                        }
                        break;

                    case TouchPhase.Stationary:
                        if (touch.fingerId == _shootFingerId && !_wasDragging)
                        {
                            ShootPosition = touch.position;
                            isShooting = true;
                        }
                        break;

                    case TouchPhase.Ended:
                        if (touch.fingerId == _shootFingerId)
                        {
                            if (!_wasDragging)
                            {
                                ShootPosition = touch.position;
                                isShooting = true;
                            }

                            _shootFingerId = -1;
                            _wasDragging = false;
                        }
                        break;
                }
            }

            return isShooting;
        }
    }

    public override float ZoomInput
    {
        get
        {
            if (Input.touchCount != 2)
            {
                _lastPinchDistance = 0f;
                return 0f;
            }

            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            if (EventSystem.current.IsPointerOverGameObject(t0.fingerId) ||
                EventSystem.current.IsPointerOverGameObject(t1.fingerId))
                return 0f;

            float currentDistance = Vector2.Distance(t0.position, t1.position);

            if (_lastPinchDistance == 0f)
            {
                _lastPinchDistance = currentDistance;
                return 0f;
            }

            float delta = currentDistance - _lastPinchDistance;
            _lastPinchDistance = currentDistance;

            return delta * 0.01f; 
        }
    }

    public override bool JumpPressed =>
        SimpleInput.GetButtonDown(JumpButton); 

    private Vector2 SimpleInputAxis() =>
        new Vector2
        (
            SimpleInput.GetAxis(ConstantsData.InputAxis.HorizontalAxis),
            SimpleInput.GetAxis(ConstantsData.InputAxis.VerticalAxis)
        );

    public override bool GetInteractButtonDown() => 
        SimpleInput.GetButtonDown(InteractButton);
}
