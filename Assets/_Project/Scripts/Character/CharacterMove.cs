using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMove : MonoBehaviour
{
    [SerializeField] private PlayerCamera _playerCamera;
    [SerializeField] private Transform _cameraFollowPoint;
    [SerializeField] private float _movementSpeed = 10f;

    private Rigidbody _rigidbody;
    private IInputService _inputService;
    private Vector3 _movementVector;

    public Vector3 MovementVector => _movementVector;
    public bool IsMoving => _inputService.Axis.sqrMagnitude > ConstantsData.Epsilon;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _inputService = Game.InputService;
    }

    private void Start() => 
        _playerCamera.SetFollowTransform(_cameraFollowPoint);

    private void Update()
    {
        _movementVector =
            (_playerCamera.CameraTransform.right * _inputService.Axis.x
            + _inputService.Axis.y * _playerCamera.CameraTransform.forward)
            .normalized;
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            Vector3 move = _movementVector * _movementSpeed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + move);
        }
    }
}
