using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 6f;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 1.5f;

    [Header("Gravity")]
    [SerializeField] private float _groundStickForce = 2f;

    private Transform _cameraTransform;
    private CharacterController _characterController;
    private IInputService _inputService;

    private Vector3 _velocity;
    private bool _isGrounded;

    public Vector3 MovementVector { get; private set; }
    public bool IsMoving => _inputService.Axis.sqrMagnitude > ConstantsData.Epsilon;

    private void Awake() =>
        _characterController = GetComponent<CharacterController>();

    private void Start() =>
        _cameraTransform = Camera.main.transform;

    private void Update()
    {
        HandleMovement();
        HandleGravity();
        HandleJump();

        Vector3 finalMove = MovementVector * _moveSpeed + _velocity;
        _characterController.Move(finalMove * Time.deltaTime);
    }

    public void Construct(IInputService inputService) =>
        _inputService = inputService;

    private void HandleMovement()
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 input = right * _inputService.Axis.x + forward * _inputService.Axis.y;

        MovementVector = Vector3.ClampMagnitude(input, 1f);
    }

    private void HandleGravity()
    {
        _isGrounded = _characterController.isGrounded;

        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -_groundStickForce;

        _velocity.y += Physics.gravity.y * Time.deltaTime;
    }

    private void HandleJump()
    {
        if (_isGrounded && _inputService.JumpPressed)
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y);
    }
}
