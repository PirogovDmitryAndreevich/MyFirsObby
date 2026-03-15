using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private float _defaultDistance = 11f,
        _minDistance = 3f,
        _maxDistance = 11f,
        _distanceMovementSharpness = 10f,
        _rotationSpeed = 10f,
        _rotationSharpness = 10000f,
        _followSharpness = 10000f,
        _minVerticalAngel = -30f,
        _maxVerticalAngel = 60f,
        _defaultVerticalAngel = 2f,
        _groundAvoidDistance = 4f; 

    private Transform _followTransform;
    private Vector3 _currentFollowPosition;
    private Vector3 _planarDirection;
    private float _targetVerticalAngel;
    private float _currentDistance;
    private float _targetDistance;

    private IInputService _inputService;

    public Transform CameraTransform => transform;

    private void Awake()
    {
        _currentDistance = _defaultDistance;
        _targetDistance = _currentDistance;
        _targetVerticalAngel = 0f;
        _planarDirection = Vector3.forward;

        _inputService = Game.InputService;
    }

    private void OnValidate()
    {
        _defaultDistance = Mathf.Clamp(_defaultDistance, _minDistance, _maxDistance);
        _defaultVerticalAngel = Mathf.Clamp(_defaultVerticalAngel, _minVerticalAngel, _maxVerticalAngel);
    }

    public void SetFollowTransform(Transform t)
    {
        _followTransform = t;
        _currentFollowPosition = t.position;
        _planarDirection = t.forward;
    }

    public void UpdateWithInput(float deltaTime, Vector3 rotationInput, bool isMoveCamera)
    {
        if (_followTransform == null)
            return;

        Quaternion targetRotation = transform.rotation;

        if (isMoveCamera)
        {
            HandleRotationInput(deltaTime, rotationInput, out targetRotation);
        }

        HandlePosition(deltaTime, targetRotation);
    }

    private void HandleRotationInput(float deltaTime, Vector3 rotationInput, out Quaternion targetRotation)
    {
        Quaternion rotationFromInput = Quaternion.Euler(_followTransform.up * (rotationInput.x * _rotationSpeed));
        _planarDirection = rotationFromInput * _planarDirection;

        Quaternion planarRoot = Quaternion.LookRotation(_planarDirection, _followTransform.up);

        _targetVerticalAngel -= (rotationInput.y * _rotationSpeed);
        _targetVerticalAngel = Mathf.Clamp(_targetVerticalAngel, _minVerticalAngel, _maxVerticalAngel);

        Quaternion verticalRoot = Quaternion.Euler(_targetVerticalAngel, 0, 0);

        targetRotation = Quaternion.Slerp(
            transform.rotation,
            planarRoot * verticalRoot,
            1 - Mathf.Exp(-_rotationSharpness * deltaTime));

        transform.rotation = targetRotation;
    }

    private void HandlePosition(float deltaTime, Quaternion targetRotation)
    {
        _currentFollowPosition = Vector3.Lerp(
            _currentFollowPosition,
            _followTransform.position,
            1f - Mathf.Exp(-_followSharpness * deltaTime));

        float pitchFactor = Mathf.InverseLerp(_minVerticalAngel, 0f, _targetVerticalAngel);
        float distanceOffset = Mathf.Lerp(_groundAvoidDistance, 0f, pitchFactor);

        _targetDistance = Mathf.Clamp(_defaultDistance - distanceOffset, _minDistance, _maxDistance);

        _currentDistance = Mathf.Lerp(
            _currentDistance,
            _targetDistance,
            1 - Mathf.Exp(-_distanceMovementSharpness * deltaTime));

        Vector3 targetPosition =
            _currentFollowPosition
            - (targetRotation * Vector3.forward * _currentDistance);

        transform.position = targetPosition;
    }
}