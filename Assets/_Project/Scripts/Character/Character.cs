using UnityEngine;

[RequireComponent(typeof(CharacterAnimation), typeof(CharacterMove), typeof(CharacterRotation))]
[RequireComponent(typeof(Shooter), typeof(CharacterHealth), typeof(LineDraw))]
[RequireComponent(typeof(CharacterDeath), typeof(CharacterSound))]
public class Character : MonoBehaviour
{
    private CharacterAnimation _animation;
    private CharacterRotation _meshRootRotation;
    private CharacterMove _mover;
    private Vector3 _resetPoint;
    private IDifficultyService _difficulty;
    private IInputService _input;   

    public CharacterHealth Health { get; private set; }
    public CharacterView CurrentView { get; private set; }
    public Transform CameraFollow { get; private set; }
    public LineDraw LineDraw { get; private set; }
    public Shooter Shooter { get; private set; }
    public CharacterDeath CharacterDeath { get; private set; }
    public CharacterSound CharacterSound { get; private set;}

    private void Update()
    {
        _animation.Run(_mover.IsMoving);
        _animation.KeepWeapon(Shooter.IsWeaponSelected);

        if (_input.JumpPressed && _mover.IsGrounded)
        {
            CharacterSound.PlayJumpSound();
            _animation.Jump();
        }

        if (_mover.IsMoving)
        {
            _meshRootRotation.RotateCharacter(_mover.MovementVector);

            if (_mover.IsGrounded)
                CharacterSound.PlayStepSound();
        }

        if (_input.IsShoot && Shooter.IsWeaponSelected && Shooter.CurrentWeapon.CanShoot)        
            Shooter.TryShoot(_input.ShootPosition);
    }

    private void OnDestroy() => 
        CharacterDeath.CharacterDead -= Death;

    public void Construct(IInputService inputService, IDifficultyService difficulty, CharacterView view, Vector3 resetPoint)
    {
        _input = inputService;
        _resetPoint = resetPoint;
        _difficulty = difficulty;

        _meshRootRotation = GetComponent<CharacterRotation>();
        _animation = GetComponent<CharacterAnimation>();
        Shooter = GetComponent<Shooter>();
        _mover = GetComponent<CharacterMove>();

        Health = GetComponent<CharacterHealth>();
        CharacterDeath = GetComponent<CharacterDeath>();
        LineDraw = GetComponent<LineDraw>();
        CharacterSound = GetComponent<CharacterSound>();

        CharacterDeath.CharacterDead += Death;

        _mover.Construct(_input);
        SetCharacterView(view);
    }

    public void SetCharacterView(CharacterView view)
    {
        if (CurrentView != null)
            Destroy(CurrentView);

        CurrentView = Instantiate(view, transform);

        Transform viewTransform = CurrentView.transform;
        viewTransform.localPosition = Vector3.zero;
        viewTransform.localRotation = Quaternion.identity;

        _meshRootRotation.CharacterView = viewTransform;

        CameraFollow = CurrentView.CameraFollow;
        _animation.Animator = CurrentView.Animator;

        LineDraw.Construct(CurrentView.CameraFollow);
    }

    private void Death()
    {
        Health.ResetHealth();
        CharacterDeath.ResetState();
        _difficulty.CharacterDeath();
        Warp();
    }

    private void Warp()
    {
        _mover.Controller.enabled = false;
        transform.position = _resetPoint;
        _mover.Controller.enabled = true;
    }
}
