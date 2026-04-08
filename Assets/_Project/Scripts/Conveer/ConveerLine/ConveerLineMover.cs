using UnityEngine;

public class ConveerLineMover : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private Material _material;

    private void FixedUpdate()
    {
        _material.mainTextureOffset = new Vector2(0f, Time.time * 5 * Time.deltaTime);

        Vector3 pos = _rigidbody.position;

        _rigidbody.position += Vector3.back * _speed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(pos);
    }
}
