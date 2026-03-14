using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    private const float minMovementMagnitude = 0.001f;

    [SerializeField] private Transform _characterView;
    [SerializeField] private float _smooth = 10f;

    public void RotateCharacter(Vector3 direction)
    {
        Vector3 horizontalDirection = new Vector3(direction.x, 0f, direction.z);

        if (horizontalDirection.sqrMagnitude < minMovementMagnitude)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);
        _characterView.rotation = Quaternion.Slerp(_characterView.rotation, targetRotation, _smooth * Time.fixedDeltaTime);
    }
}
