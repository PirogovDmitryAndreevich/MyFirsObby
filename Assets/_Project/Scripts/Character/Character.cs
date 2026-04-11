using UnityEngine;

[RequireComponent(typeof(CharacterAnimation), typeof(CharacterMove), typeof(CharacterRotation))]
[RequireComponent(typeof(Shooter), typeof(CharacterHealth), typeof(LineDraw))]
public class Character : MonoBehaviour, IInteractable
{
    private Shooter _shooter;
    private CharacterAnimation _animation;
    private CharacterRotation _meshRootRotation;
    private CharacterMove _mover;

    private IInputService _input;
    private IInteractObject _currentInteract;

    public CharacterHealth Health { get; private set; }
    public CharacterView CurrentView { get; private set; }
    public Transform CameraFollow { get; private set; }
    public WeaponView SelectedWeapon { get; private set; }
    public LineDraw LineDraw { get; private set; }

    private void Update()
    {
        _animation.Run(_mover.IsMoving);
        _animation.KeepWeapon(_input.GetAttackState());

        if (_mover.IsMoving)
            _meshRootRotation.RotateCharacter(_mover.MovementVector);

        if (_input.IsShoot && _input.GetAttackState() && _shooter.CanShootNow)
            _shooter.TryShoot(_input.ShootPosition);

        if (_currentInteract != null && Input.GetKeyDown(KeyCode.E))
            _currentInteract.Interact();
    }

    public void Construct(IInputService inputService, CharacterView view)
    {
        _input = inputService;

        _meshRootRotation = GetComponent<CharacterRotation>();
        _animation = GetComponent<CharacterAnimation>();
        _shooter = GetComponent<Shooter>();
        _mover = GetComponent<CharacterMove>();

        Health = GetComponent<CharacterHealth>();
        LineDraw = GetComponent<LineDraw>();
        
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

    public void SelectWeapon(WeaponStaticData weapon, int damageLevel, int magazineLevel)
    {
        if (SelectedWeapon != null)
            Destroy(SelectedWeapon.gameObject);

        GameObject prefab = Instantiate(weapon.Prefab, CurrentView.WeaponPoint);
        SelectedWeapon = prefab.GetComponent<WeaponView>();
        prefab.transform.position = CurrentView.WeaponPoint.position;

        _shooter.SelectWeapon(weapon, SelectedWeapon.Muzzle, damageLevel, magazineLevel);
    }

    public void SetInteractable(IInteractObject interact) =>
        _currentInteract = interact;
}
