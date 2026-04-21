using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ATMCollider : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Collider _collider;

    private MaterialPropertyBlock _block;

    public event Action<Collider> TriggerEnter;
    public event Action<Collider> TriggerExit;

    public bool IsPlayerInside { get; private set; }

    private void Awake() =>
        _block = new MaterialPropertyBlock();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Character>(out Character character))
        {
            IsPlayerInside = true;
            TriggerEnter?.Invoke(other);
        }            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Character>(out Character character))
        {
            IsPlayerInside = false;
            TriggerExit?.Invoke(other);
        }
    }

    public void SetInteract(bool isActive)
    {
        _collider.enabled = isActive;
        SetView(isActive);
    }

    private void SetView(bool isGreen)
    {
        if (isGreen)
            SetGreenColor();
        else
            SetRedColor();
    }

    private void SetRedColor() =>
        SetColor(Color.red);

    private void SetGreenColor() =>
        SetColor(Color.green);

    private void SetColor(Color color)
    {
        _renderer.GetPropertyBlock(_block);
        _block.SetColor("_Color", color);
        _renderer.SetPropertyBlock(_block);
    }
}