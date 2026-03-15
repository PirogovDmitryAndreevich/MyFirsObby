using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private const string IsRun = "IsRun";
    private const string IsKeepWeapon = "IsKeepWeapon";

    public void Run(bool isRun)
    {
        _animator.SetBool(IsRun, isRun);
    }

    public void KeepWeapon(bool isKeepWeapon)
    {
        _animator.SetBool(IsKeepWeapon,isKeepWeapon);
    }
}
