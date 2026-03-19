using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private CharacterController _character;

    private IInputService _input;

    private void Awake()
    {
        _input = AllServices.Container.Single<IInputService>();
    }


}
