using UnityEngine;

[RequireComponent(typeof(CharacterAnimation), typeof(CharacterMove), typeof(CharacterRotation))]
[RequireComponent(typeof(Shooter), typeof(CharacterHealth), typeof(LineDraw))]
[RequireComponent(typeof(CharacterDeath))]
public class Character : MonoBehaviour, IInteractable
{
    private CharacterAnimation _animation;
    private CharacterRotation _meshRootRotation;
    private CharacterMove _mover;
    private Vector3 _resetPoint;
    private IDifficultyService _difficulty;
    private IInputService _input;
    private IInteractObject _currentInteract;

    public CharacterHealth Health { get; private set; }
    public CharacterView CurrentView { get; private set; }
    public Transform CameraFollow { get; private set; }
    public WeaponView SelectedWeapon { get; private set; }
    public LineDraw LineDraw { get; private set; }
    public Shooter Shooter { get; private set; }
    public CharacterDeath CharacterDeath { get; private set; }

    private void Update()
    {
        _animation.Run(_mover.IsMoving);
        _animation.KeepWeapon(Shooter.IsWeaponSelected);

        if (_input.JumpPressed)
            _animation.Jump();

        if (_mover.IsMoving)
            _meshRootRotation.RotateCharacter(_mover.MovementVector);

        if (_input.IsShoot && Shooter.IsWeaponSelected && Shooter.CurrentWeapon.RuntimeState.CanShoot)
            Shooter.TryShoot(_input.ShootPosition);

        if (_currentInteract != null && _input.GetInteractButtonDown())
            _currentInteract.Interact();
    }

    private void OnDestroy()
    {
        CharacterDeath.CharacterDead -= Death;
    }

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

        viewTransform.position = transform.position;
        _meshRootRotation.CharacterView = viewTransform;

        CameraFollow = CurrentView.CameraFollow;
        _animation.Animator = CurrentView.Animator;

        LineDraw.Construct(CurrentView.CameraFollow);
    }

    public void SelectWeapon(Weapon weapon)
    {
        if (SelectedWeapon != null)
            Destroy(SelectedWeapon.gameObject);

        GameObject prefab = Instantiate(weapon.StaticData.Prefab, CurrentView.WeaponPoint);
        SelectedWeapon = prefab.GetComponent<WeaponView>();
        prefab.transform.position = CurrentView.WeaponPoint.position;

        Shooter.SelectWeapon(weapon, SelectedWeapon.Muzzle);
    }

    public void SetInteractable(IInteractObject interact) =>
        _currentInteract = interact;

    public void UnselectWeapon()
    {
        if (SelectedWeapon != null)
            Destroy(SelectedWeapon.gameObject);

        Shooter.UnselectWeapon();
    }

    private void Death()
    {
        Health.ResetHealth();
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
