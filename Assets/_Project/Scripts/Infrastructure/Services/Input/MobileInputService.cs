using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInputService : InputService
{
    private Vector2 _startPos;
    private bool _isDragging;
    private float _dragThreshold = 20f;
    private int _shootFingerId = -1;
    private float _lastPinchDistance;
    private bool _wasPinching;

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
            if (Input.touchCount == 0)
            {
                _shootFingerId = -1;
                return false;
            }

            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    continue;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _startPos = touch.position;
                        _isDragging = false;
                        _shootFingerId = touch.fingerId;
                        break;

                    case TouchPhase.Moved:
                        if (touch.fingerId == _shootFingerId)
                        {
                            if (Vector2.Distance(_startPos, touch.position) > _dragThreshold)
                                _isDragging = true;
                        }
                        break;

                    case TouchPhase.Stationary:
                        if (touch.fingerId == _shootFingerId && !_isDragging)
                        {
                            ShootPosition = touch.position;
                            return true;
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        if (touch.fingerId == _shootFingerId)
                        {
                            _shootFingerId = -1;
                        }
                        break;
                }
            }

            return false;
        }
    }

    public override float ZoomInput
    {
        get
        {
            if (Input.touchCount < 2)
            {
                _wasPinching = false;
                return 0f;
            }

            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Игнорируем, если палец над UI
            if (EventSystem.current.IsPointerOverGameObject(touch0.fingerId) ||
                EventSystem.current.IsPointerOverGameObject(touch1.fingerId))
            {
                _wasPinching = false;
                return 0f;
            }

            float currentDistance = Vector2.Distance(touch0.position, touch1.position);

            if (!_wasPinching)
            {
                _lastPinchDistance = currentDistance;
                _wasPinching = true;
                return 0f;
            }

            float delta = currentDistance - _lastPinchDistance;
            _lastPinchDistance = currentDistance;

            // Масштабируем значение (очень важно!)
            return delta * 0.01f;
        }
    }

    public override bool JumpPressed => throw new System.NotImplementedException();
}
