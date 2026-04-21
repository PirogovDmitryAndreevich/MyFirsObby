using TMPro;
using UnityEngine;

public class ATM : MonoBehaviour
{
    [SerializeField] private ATMCollider _collider;
    [SerializeField] private TMP_Text _text;

    private int _cash;

    private bool isCashCollected;
    private IPersistentProgressService _progressService;
    private ISaveLoadService _save;

    private void OnEnable()
    {
        _collider.TriggerEnter += TriggerEnter;
        _collider.TriggerExit += TriggerExit;
    }

    private void OnDisable()
    {
        _collider.TriggerEnter -= TriggerEnter;
        _collider.TriggerExit -= TriggerExit;
    }

    private void Start()
    {
        UpdateText(_cash);
        SetState();
    }

    private void TriggerEnter(Collider other)
    {
        if (!isCashCollected && _collider.IsPlayerInside)
        {
            isCashCollected = false;
            _progressService.Progress.PlayerState.AddCash(CollectCash());
            _save.SaveProgress();
            SetState();
        }
    }

    private void TriggerExit(Collider other)
    {
        isCashCollected = true;
        SetState();
    }

    public void Construct(IPersistentProgressService progressService, ISaveLoadService save)
    {
        _progressService = progressService;
        _save = save;
    }

    public void AddCash(int value)
    {
        _cash += value;

        if (!_collider.IsPlayerInside && _cash > 0)
            isCashCollected = false;

        UpdateText(_cash);
        SetState();
    }

    private int CollectCash()
    {
        int collect = _cash;
        _cash = 0;
        UpdateText(_cash);
        SetState();
        return collect;
    }

    private void UpdateText(int value) =>
        _text.text = FormatCash(value);

    private void SetState()
    {
        Debug.Log($"[ATM] SetState - IsCashCollected({isCashCollected}) - cash({_cash})");
        _collider.SetInteract(!isCashCollected && _cash > 0);
    }

    private string FormatCash(int value)
    {
        if (value < 1000)
            return value.ToString("0");

        if (value < 1_000_000)
            return Format(value, 1000, "ê");

        if (value < 1_000_000_000)
            return Format(value, 1_000_000, "ì");

        return Format(value, 1_000_000_000, "b");
    }

    private string Format(int value, int divider, string suffix)
    {
        int result = value / divider;

        if (result % 1 == 0)
            return result.ToString("0") + suffix;

        return result.ToString("0.#") + suffix;
    }
}
