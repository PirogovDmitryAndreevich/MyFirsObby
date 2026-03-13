using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    private const string IsRun = "IsRun";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Run(bool isRun)
    {
        _animator.SetBool(IsRun, isRun);
    }
}
