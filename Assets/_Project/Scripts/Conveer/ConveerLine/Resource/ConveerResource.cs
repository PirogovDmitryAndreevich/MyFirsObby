using UnityEngine;

public class ConveerResource : MonoBehaviour, IConveerResource
{
    private IPoolService _poolService;
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

    public void SetModel(GameObject view)
    {
        Destroy(_currentModel);
        _currentModel = Instantiate(view, transform);
    }

    public void Init( int reward)
    {
        _isCollect = false;
        Reward = reward;
    }
}