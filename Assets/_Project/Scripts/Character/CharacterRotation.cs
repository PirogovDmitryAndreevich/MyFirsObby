using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    [SerializeField] private Transform _character;
    [SerializeField] private float _sensitivity = 1.5f;
    [SerializeField] private float _smooth = 10f;

    private float _xRotation;
    private float _yRotation;

    private void Update()
    {
        _yRotation += Input.GetAxis(ConstantsData.InputAxis.MouseX) * _sensitivity;
    }

    private void FixedUpdate()
    {
        RotateCharacter();
    }

    private void RotateCharacter()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, _yRotation, 0), Time.deltaTime * _smooth);
        _character.rotation = Quaternion.Lerp(_character.rotation, Quaternion.Euler(0, _yRotation, 0), Time.deltaTime * _smooth);
    }
}
