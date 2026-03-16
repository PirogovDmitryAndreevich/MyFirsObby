using UnityEngine;

[RequireComponent(typeof(CharacterAnimation), typeof(CharacterMove), typeof(CharacterRotation))]
public class CharacterController : MonoBehaviour, ISavedProgress
{
    [SerializeField] private Guns _gun;

    private CharacterMove _mover;
    private CharacterAnimation _animation;
    private InputReader _inputReader;
    private CharacterRotation _meshRootRotation;

    public Guns SelectedGuns => _gun;

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
        _meshRootRotation = GetComponent<CharacterRotation>();
        _animation = GetComponent<CharacterAnimation>();
        _mover = GetComponent<CharacterMove>();
    }

    private void FixedUpdate()
    {
        _animation.Run(_mover.IsMoving);
        _animation.KeepWeapon(_inputReader.IsKeepWeapon);

        if (_mover.IsMoving)
            _meshRootRotation.RotateCharacter(_mover.MovementVector);

        if (_inputReader.GetIsShot() && _inputReader.IsKeepWeapon && _gun.CanShotNow)
            _gun.Shoot();
    }
    public void UpdateProgress(PlayerProgress progress) =>
        progress.WorldData.Position = transform.position.AsVectorData();

    public void LoadProgress(PlayerProgress progress)
    {
        Vector3Data savedPosition = progress.WorldData.Position;

        if (savedPosition != null)            
            Warp(to: savedPosition);        
    }

    private void Warp(Vector3Data to)
    {
        _mover.enabled = false;
        transform.position = to.AsUnityVector();
        _mover.enabled = true;
    }
}
