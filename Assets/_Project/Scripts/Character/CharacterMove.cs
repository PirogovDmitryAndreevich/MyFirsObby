using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10f;

    private Transform _cameraTransform;
    private CharacterController _characterController;
    private IInputService _inputService;

    public Vector3 MovementVector { get; private set; }
    public bool IsMoving => _inputService.Axis.sqrMagnitude > ConstantsData.Epsilon;

    private void Awake() =>
        _characterController = GetComponent<CharacterController>();

    private void Start() =>
        _cameraTransform = Camera.main.transform;

    private void Update()
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        MovementVector = right * _inputService.Axis.x + forward * _inputService.Axis.y;

        MovementVector.Normalize();

        MovementVector += Physics.gravity;
        _characterController.Move(MovementVector * _movementSpeed * Time.deltaTime);
        //if (IsMoving)

    }

    public void Construct(IInputService inputService) =>
        _inputService = inputService;
}
