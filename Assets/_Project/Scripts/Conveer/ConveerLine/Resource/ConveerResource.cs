using System;
using UnityEngine;

public class ConveerResource : MonoBehaviour, IConveerResource
{
    private IPoolService _poolService;
    private GameObject _currentPrefab;
    private GameObject _currentModel;
    private bool _isCollect;

    public int Reward { get; private set; }

    public void Construct(IPoolService poolService) => 
        _poolService = poolService;

    public void Collect()
    {
        if (_isCollect)
            return;

        _isCollect = true;
        _poolService.Return(this);
    }

    public void Init( int reward, GameObject model)
    {
        _isCollect = false;

        if (model != _currentPrefab)
            ResetModel(model);

        Reward = reward;
    }

    private void ResetModel(GameObject model)
    {
        if (_currentModel != null)
            Destroy(_currentModel);

        _currentModel = Instantiate(model, transform);
        _currentPrefab = model;
    }
}