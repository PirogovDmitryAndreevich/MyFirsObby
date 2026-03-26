using UnityEngine;

public class Muzzle : MonoBehaviour
{
    [SerializeField] private Transform _muzzle;

    public Transform CurrentMuzzle => _muzzle;
}