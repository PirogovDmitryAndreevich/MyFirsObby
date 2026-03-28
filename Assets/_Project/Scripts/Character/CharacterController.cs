using UnityEngine;

[RequireComponent(typeof(CharacterAnimation), typeof(CharacterMove), typeof(CharacterRotation))]
[RequireComponent(typeof(Shooter), typeof(CharacterHealth))]
public class CharacterController : MonoBehaviour, ISavedProgress, IInteractable
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
    public Shooter SelectedGuns => _shooter;

    private void Update()
    {
        _animation.Run(_mover.IsMoving);
        _animation.KeepWeapon(_input.GetAttackState());

        if (_mover.IsMoving)
            _meshRootRotation.RotateCharacter(_mover.MovementVector);

        if (_input.IsShoot && _input.GetAttackState() && _shooter.CanShotNow)
            _shooter.Shoot(_input.ShootPosition);

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
    }

    public void SelectWeapon(WeaponStaticData weapon, int damageLevel, int magazineLevel)
    {
        GameObject prefab = weapon.Prefab;
        Muzzle muzzle = prefab.GetComponentInChildren<Muzzle>();
        _shooter.SelectWeapon(weapon, muzzle.CurrentMuzzle, damageLevel, magazineLevel);
    }

    public void SetInteractable(IInteractObject interact) =>
        _currentInteract = interact;

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
