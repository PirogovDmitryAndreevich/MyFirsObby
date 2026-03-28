using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private const string IsRun = "IsRun";
    private const string IsKeepWeapon = "IsKeepWeapon";

    public Animator Animator { get; set; }

    public void Run(bool isRun)
    {
        Animator.SetBool(IsRun, isRun);
    }

    public void KeepWeapon(bool isKeepWeapon)
    {
        Animator.SetBool(IsKeepWeapon,isKeepWeapon);
    }
}
