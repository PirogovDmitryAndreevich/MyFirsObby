using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CharacterMove), typeof(CharacterRotation))]
public class CharacterController : MonoBehaviour
{
    [SerializeField] private PlayerCamera _playerCamera;    
    [SerializeField] private CharacterAnimation _animation;
    [SerializeField] private Guns _gun;

    private InputReader _inputReader;
    private CharacterMove _mover;
    private CharacterRotation _meshRootRotation;
    private IInputService _inputService;

    public Guns SelectedGuns => _gun;

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _meshRootRotation = GetComponent<CharacterRotation>();
        _mover = GetComponent<CharacterMove>();
        _inputService = Game.InputService;
    }

    private void FixedUpdate()
    {
        _animation.Run(_mover.IsMoving);
        _animation.KeepWeapon(_inputReader.IsKeepWeapon);

        if (_mover.IsMoving)
        {   
            _meshRootRotation.RotateCharacter(_mover.MovementVector);
        }

        if (_inputReader.GetIsShot() && _inputReader.IsKeepWeapon && _gun.CanShotNow)
            _gun.Shoot();
    }    

    private void LateUpdate()
    {
        _playerCamera.UpdateWithInput
            (Time.deltaTime,
            _inputService.LookInputVector,
           _inputService.IsCameraMoving);
    }
}
