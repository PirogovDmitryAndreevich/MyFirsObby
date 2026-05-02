using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ConveerResource : MonoBehaviour , IConveyerResource
{
    [SerializeField] private TMP_Text _value;
    [SerializeField] private ConveyerType _type;
    [SerializeField] private int _reward;

    private IConveyersPool _pool;
    private bool _isCollect;
    private Transform _transform;

    public ConveyerType Type => _type;
    public int Reward => _reward;

    public void Construct(IConveyersPool pool)
    {
        _pool = pool;
        _transform = GetComponent<Transform>();
        _value.text = _reward.ToString() + "$";
    }

    public void Collect()
    {
        if (_isCollect)
            return;

        _isCollect = true;
        _pool.Return(this);
    }

    public void ResetState(Vector3 position)
    {
        _transform.position = position;
        _isCollect = false;
    }
}