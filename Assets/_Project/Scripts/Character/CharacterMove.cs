using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10f;

    private PlayerCamera _playerCamera;
    private CharacterController _characterController;
    private IInputService _inputService;

    public Vector3 MovementVector { get; private set; }
    public bool IsMoving => _inputService.Axis.sqrMagnitude > ConstantsData.Epsilon;

    private void Awake() =>
        _characterController = GetComponent<CharacterController>();

    private void Start() =>
        _playerCamera = Camera.main.GetComponent<PlayerCamera>();

    private void Update()
    {
        Vector3 forward = _playerCamera.CameraTransform.forward;
        Vector3 right = _playerCamera.CameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        MovementVector =
            (right * _inputService.Axis.x +
             forward * _inputService.Axis.y).normalized;

        MovementVector += Physics.gravity;


        _characterController.Move(MovementVector * _movementSpeed * Time.fixedDeltaTime);
        //if (IsMoving)

    }

    public void Construct(IInputService inputService) =>
        _inputService = inputService;
}
