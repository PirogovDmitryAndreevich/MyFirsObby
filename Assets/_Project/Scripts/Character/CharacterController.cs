using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(InputReader), typeof(CharacterRotation))]
public class CharacterController : MonoBehaviour
{
    [SerializeField] private PlayerCamera _playerCamera;
    [SerializeField] private Transform _cameraFollowPoint;
    [SerializeField] private CharacterAnimation _animation;
    [SerializeField] private Guns _gun;
    [SerializeField] private float _moveSpeed = 10f;

    private InputReader _inputReader;
    private CharacterRotation _meshRootRotation;
    private Rigidbody _rigidbody;
    private Vector3 _movementVector;

    public Guns SelectedGuns => _gun;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _inputReader = GetComponent<InputReader>();
        _meshRootRotation = GetComponent<CharacterRotation>();
    }

    private void Start()
    {
        _playerCamera.SetFollowTransform(_cameraFollowPoint);
    }

    private void FixedUpdate()
    {
        bool isMoving = _inputReader.Direction.x != 0
                            || _inputReader.Direction.y != 0;

        _animation.Run(isMoving);
        _animation.KeepWeapon(_inputReader.IsKeepWeapon);

        if (isMoving)
        {            
            Vector3 move = _movementVector * _moveSpeed * Time.fixedDeltaTime;

            _rigidbody.MovePosition(_rigidbody.position + move);

            _meshRootRotation.RotateCharacter(_movementVector);
        }

        if (_inputReader.GetIsShot() && _inputReader.IsKeepWeapon && _gun.CanShotNow)
            _gun.Shoot();
    }

    private void Update()
    {
        _movementVector =
            (_playerCamera.CameraTransform.right * _inputReader.Direction.x
            + _inputReader.Direction.y * _playerCamera.CameraTransform.forward)
            .normalized;
    }

    private void LateUpdate()
    {
        _playerCamera.UpdateWithInput
            (Time.deltaTime,
            _inputReader.LookInputVector, 
            _inputReader.IsRMBDown);
    }
}
