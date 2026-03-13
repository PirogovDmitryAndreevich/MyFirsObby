using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(InputReader))]
public class CharacterController : MonoBehaviour
{
    [SerializeField] private CharacterAnimation _animation;
    [SerializeField] private float _moveSpeed = 10f;

    private InputReader _inputReader;
    private Rigidbody _rigidbody;
    private Vector3 _movementVector;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _inputReader = GetComponent<InputReader>();
    }

    private void Update()
    {
        _movementVector = 
            (transform.right * _inputReader.Direction.x 
            + _inputReader.Direction.y * transform.forward)
            .normalized;
    }

    private void FixedUpdate()
    {
        _animation.Run(_inputReader.Direction.x != 0
                            || _inputReader.Direction.y != 0);

        _rigidbody.MovePosition(transform.position + _movementVector * _moveSpeed * Time.fixedDeltaTime);
    }
}
