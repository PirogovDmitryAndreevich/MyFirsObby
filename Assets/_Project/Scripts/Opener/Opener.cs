using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Opener : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Collider _collider;
    [SerializeField] private TMP_Text _name;

    protected int Price;
    private MaterialPropertyBlock _block;

    protected PlayerProgress Progress;

    private void Awake() => 
        _block = new MaterialPropertyBlock();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Character character))
            return;

        OnTrigger();
    }

    protected virtual void OnTrigger()
    {

    }

    protected void SetState() =>
        SetInteract(Progress.PlayerState.Cash >= Price);

    protected void SetText() =>
        _name.text = $"{Price}$";

    protected void UnsubscribeCash()
    {
        Progress.PlayerState.CashChanged -= SetState;
    }

    protected void SubscribeCash() => 
        Progress.PlayerState.CashChanged += SetState;

    protected void SetInteract(bool isActive)
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

    private void SetGreenColor()=>
        SetColor(Color.green);

    private void SetColor(Color color)
    {
        _renderer.GetPropertyBlock(_block);
        _block.SetColor("_Color", color);
        _renderer.SetPropertyBlock(_block);
    }
}
