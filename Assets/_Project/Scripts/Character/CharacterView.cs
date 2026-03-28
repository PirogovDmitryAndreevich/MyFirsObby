using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private CharacterType _type;

    [SerializeField] private Transform _weaponPoint;
    [SerializeField] private Transform _cameraFollow;
    [SerializeField] private Animator _animator;

    public CharacterType Type => _type;
    public Transform WeaponPoint => _weaponPoint;
    public Transform CameraFollow => _cameraFollow;
    public Animator Animator => _animator;
}
