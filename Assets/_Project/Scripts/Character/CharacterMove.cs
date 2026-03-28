using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMove : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10f;

    private PlayerCamera _playerCamera;
    private Rigidbody _rigidbody;
    private IInputService _inputService;

    public Vector3 MovementVector { get; private set; }
    public bool IsMoving => _inputService.Axis.sqrMagnitude > ConstantsData.Epsilon;

    private void Awake() => 
        _rigidbody = GetComponent<Rigidbody>();

    private void Start() => 
        _playerCamera = Camera.main.GetComponent<PlayerCamera>();

    private void Update()
    {
        MovementVector =
            (_playerCamera.CameraTransform.right * _inputService.Axis.x
            + _inputService.Axis.y * _playerCamera.CameraTransform.forward)
            .normalized;
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            Vector3 move = MovementVector * _movementSpeed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + move);
        }
    }

    public void Construct(IInputService inputService) => 
        _inputService = inputService;
}
