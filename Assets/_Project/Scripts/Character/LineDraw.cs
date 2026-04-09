using UnityEngine;

public class LineDraw : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;

    private Material _material;
    private Transform _character;
    private Transform _target;

    private void Update()
    {
        if (_target == null)
            return;

        _material.mainTextureOffset = new Vector2(Time.time * 2f, 0f);

        _line.SetPosition(0, _character.position);
        _line.SetPosition(1, _target.position);
    }

    public void Construct(Transform characterPoint)
    {
        _character = characterPoint;
        _material = _line.material;
    }


    public void StartDraw(Transform target)
    {
        _line.enabled = true;
        _target = target;
    }

    public void StopDraw()
    {
        _line.enabled = false;
        _target = null;
    }
}