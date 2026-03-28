using UnityEngine;

public class ConveerLine : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private Material _material;

    private void FixedUpdate()
    {
        _material.mainTextureOffset = new Vector2(Time.time * 10 * Time.deltaTime, 0f);

        Vector3 pos = _rigidbody.position;

        _rigidbody.position += Vector3.back * _speed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(pos);
    }
}
