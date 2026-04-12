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

        Vector3 finalMove = MovementVector + _velocity;
        _characterController.Move(finalMove * Time.deltaTime);
    }

    public void Construct(IInputService inputService) =>
        _inputService = inputService;

    private void HandleMovement()
    {
        Vector3 input = _cameraTransform.right * _inputService.Axis.x + _cameraTransform.forward * _inputService.Axis.y;
        input.y = 0;
        input.Normalize();

        MovementVector = Vector3.ClampMagnitude(input, 1f) * _moveSpeed;
    }

    private void HandleGravity()
    {
        _isGrounded = _characterController.isGrounded;

        if (_isGrounded && _velocity.y < 0)        
            _velocity.y = -_groundStickForce;        

        _velocity += Physics.gravity * Time.deltaTime;
    }

    private void HandleJump()
    {
        if (_isGrounded && _inputService.JumpPressed)        
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y);        
    }
}
