using System;
using UnityEngine;

public class ConveerCubeMover : MonoBehaviour
{
    [SerializeField] private Transform _createPoint;
    [SerializeField] private Transform _creatorCub;
    [SerializeField] private Transform _creatorStartPosition;
    [SerializeField] private Transform _creatorEndPosition;
    [SerializeField] private float _speed = 0.3f;

    private ConveerLine _line;
    private float _cooldown;
    private float _cooldownTimer;
    private bool _isWorking = false;
    private bool _movingToEnd = true;

    public event Action Create;

    public Transform ResourceSpawnPoint => _createPoint;

    private void OnDestroy() => 
        _line.LineProgress.ConveerLineUpdated -= SetCooldown;

    private void Update()
    {
        if (!_isWorking)
        {
            _cooldownTimer += Time.deltaTime;

            if (_cooldownTimer >= _cooldown)
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
                Create?.Invoke();
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

    public void Construct(ConveerLine line)
    {
        _line = line;

        _line.LineProgress.ConveerLineUpdated += SetCooldown;
        SetCooldown(ConveerUpdateType.Cooldawn);
    }

    private void SetCooldown(ConveerUpdateType type)
    {
        if (type == ConveerUpdateType.Cooldawn)
            _cooldown = _line.LineStaticInfo.GetLevelCooldowInfo(_line.LineProgress.GetLevel(type)).Value;
    }

    private void MoveTo(Vector3 target)
    {
        _creatorCub.position = Vector3.MoveTowards(
            _creatorCub.position,
            target,
            _speed * Time.deltaTime
        );
    }

    private bool IsAtPosition(Vector3 target) =>
        Vector3.Distance(_creatorCub.position, target) < 0.01f;
}