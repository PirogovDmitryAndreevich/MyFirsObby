using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Opener : MonoBehaviour, ISavedProgressReader
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Collider _collider;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private AudioClip _openerSound;

    private int _price;
    private OpeningType _type;
    private MaterialPropertyBlock _block;
    private IOpenService _openService;
    private PlayerProgress _progress;
    private UIAudioHandler _audio;

    private void Awake() => 
        _block = new MaterialPropertyBlock();

    private void OnDestroy() => 
        _progress.PlayerState.CashChanged -= SetState;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Character character))
            return;

        if (_progress.PlayerState.Cash >= _price)
        {
            _audio.PlaySound(_openerSound);
            _progress.PlayerState.SpendCash(_price);
            _openService.OpenObject(_type);
        }
    }

    public void Construct(IOpenService openService ,IPersistentProgressService progress, OpeningType type, int price, UIAudioHandler audioHandler)
    {
        _openService = openService;
        _progress = progress.Progress;
        _price = price;
        _type = type;
        _name.text = $"{_price}$";
        _audio = audioHandler;

        _progress.PlayerState.CashChanged += SetState;
        SetState();
    }

    public void LoadProgress(PlayerProgress progress)
    {
        if (progress.BuildProgress.OpenedOpeners.Contains(_type))
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    private void SetState() =>
        SetInteract(_progress.PlayerState.Cash >= _price);

    private void SetInteract(bool isActive)
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
