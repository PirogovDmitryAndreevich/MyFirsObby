using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    private const float minMovementMagnitude = 0.001f;

    [SerializeField] private float _smooth = 10f;

    public Transform CharacterView { get; set; }

    public void RotateCharacter(Vector3 direction)
    {
        Vector3 horizontalDirection = new Vector3(direction.x, 0f, direction.z);

        if (horizontalDirection.sqrMagnitude < minMovementMagnitude)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(horizontalDirection);
        CharacterView.rotation = Quaternion.Slerp(CharacterView.rotation, targetRotation, _smooth * Time.fixedDeltaTime);
    }
}
