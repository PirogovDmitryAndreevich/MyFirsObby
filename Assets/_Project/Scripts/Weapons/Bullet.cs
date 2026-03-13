using System.Security.Cryptography;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _flySpeed;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    public void Shot(Vector3 force)
    {
        _rigidbody.velocity = force;
    }
}