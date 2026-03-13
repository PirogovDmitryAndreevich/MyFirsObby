using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    private const string IsRun = "IsRun";
    private const string IsKeepWeapon = "IsKeepWeapon";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Run(bool isRun)
    {
        _animator.SetBool(IsRun, isRun);
    }

    public void KeepWeapon(bool isKeepWeapon)
    {
        _animator.SetBool(IsKeepWeapon,isKeepWeapon);
    }
}
