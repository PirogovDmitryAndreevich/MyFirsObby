using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private const string IsRun = "IsRun";
    private const string IsKeepWeapon = "IsKeepWeapon";
    private const string IsJump = "IsJump";

    public Animator Animator { get; set; }

    public void Run(bool isRun)
    {
        Animator.SetBool(IsRun, isRun);
    }

    public void KeepWeapon(bool isKeepWeapon)
    {
        Animator.SetBool(IsKeepWeapon,isKeepWeapon);
    }

    public void Jump()
    {
        Animator.SetTrigger(IsJump);
    }
}
