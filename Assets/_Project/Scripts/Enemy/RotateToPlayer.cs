using UnityEngine;

public class RotateToPlayer : Follow
{
    [SerializeField] private float _speed = 3f; 
    
    private IGameFactory _gameFactory;
    private Transform _characterTransform;
    private Vector3 _positionToLook;

    private void Start()
    {
        _gameFactory = AllServices.Container.Single<IGameFactory>();

        if (_gameFactory.CharacterObject != null)
            SetCharacterTransform();
        else
            _gameFactory.CharacterCreated += CharacterCreated;
    }

    private void Update()
    {
        if (Initialized())
            RotateTowardsCharacter();
    }

    private void RotateTowardsCharacter()
    {
        UpdatePositionToLookAt();

        transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
    }

    private void UpdatePositionToLookAt()
    {
        Vector3 positionDiff = _characterTransform.position - transform.position;
        _positionToLook = new Vector3(positionDiff.x, transform.position.y, positionDiff.z);
    }

    private void SetCharacterTransform() =>
        _characterTransform = _gameFactory.CharacterObject.transform;

    private void CharacterCreated()
    {
        _gameFactory.CharacterCreated -= CharacterCreated;
        SetCharacterTransform();
    }

    private Quaternion SmoothedRotation(Quaternion rotation, Vector3 positionToLook) =>
        Quaternion.Lerp(rotation, TargetRotation(positionToLook), SpeedFactor());

    private Quaternion TargetRotation(Vector3 position) =>
        Quaternion.LookRotation(position);

    private float SpeedFactor() =>
        _speed * Time.deltaTime;

    private bool Initialized() =>
        _characterTransform != null;
}
