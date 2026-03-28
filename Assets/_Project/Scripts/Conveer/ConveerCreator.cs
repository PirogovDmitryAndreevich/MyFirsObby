using UnityEngine;

public class ConveerCreator : MonoBehaviour
{
    [SerializeField] private ConveerNumbers _numbers;
    [SerializeField] private Transform _creatorCub;
    [SerializeField] private ConveerResource _resource;
    [SerializeField] private Transform _createPoint;
    [SerializeField] private Transform _creatorStartPosition;
    [SerializeField] private Transform _creatorEndPosition;
    [SerializeField] private float _speed = 0.3f;

    private bool _movingToEnd = true;
    private bool _isWorking = false;

    private float _cooldownTimer;

    public ConveerNumbers Numbers => _numbers;
    public GameObject Prefab { get; set; }
    public float Cooldown { get; set; }

    private void Update()
    {
        if (!_isWorking)
        {
            _cooldownTimer += Time.deltaTime;

            if (_cooldownTimer >= Cooldown)
            {
                _cooldownTimer = 0f;
                _isWorking = true;
                _movingToEnd = true;
            }

            return;
        }

        if (_movingToEnd)
        {
            MoveTo(_creatorEndPosition.position);

            if (IsAtPosition(_creatorEndPosition.position))
            {
                Spawn();
                _movingToEnd = false;
            }
        }
        else
        {
            MoveTo(_creatorStartPosition.position);

            if (IsAtPosition(_creatorStartPosition.position))
            {
                _isWorking = false; 
            }
        }
    }

    private void MoveTo(Vector3 target)
    {
        _creatorCub.position = Vector3.MoveTowards(
            _creatorCub.position,
            target,
            _speed * Time.deltaTime
        );
    }

    private bool IsAtPosition(Vector3 target)
    {
        return Vector3.Distance(_creatorCub.position, target) < 0.01f;
    }

    private void Spawn()
    {        
        var pref = Instantiate(_resource, _createPoint.position, Quaternion.identity);
        pref.SetModel(Prefab);
    }
}