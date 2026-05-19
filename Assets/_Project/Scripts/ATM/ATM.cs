using TMPro;
using UnityEngine;

public class ATM : MonoBehaviour
{
    [SerializeField] private ATMCollider _collider;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private AudioHandler _audioHandler;
    [SerializeField] private AudioClip _dropSound;
    [SerializeField] private int _test = 10000;

    private int _cash;

    private IPersistentProgressService _progressService;
    private ISaveLoadService _save;

    public bool isCashCollected {  get; private set; }
    public int Cash => _cash;
    public ATMCollider ATMButton => _collider;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            AddCash(_test);
    }

    private void TriggerEnter(Collider other)
    {
        if (!isCashCollected && _collider.IsPlayerInside)
        {
            isCashCollected = false;
            _progressService.Progress.PlayerState.AddCash(CollectCash());
            _audioHandler.PlaySound(_dropSound);
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

    private void SetState() => 
        _collider.SetInteract(!isCashCollected && _cash > 0);

    private string FormatCash(int value)
    {
        if (value < 1_000)
            return value.ToString();

        // 1,0ê - 9,9ê
        if (value < 10_000)
            return (value / 1000f).ToString("0.0").Replace(".", ",") + "ê";

        // 10ê - 999ê
        if (value < 1_000_000)
            return (value / 1000).ToString("0") + "ê";

        // 1,0ì - 9,9ì
        if (value < 10_000_000)
            return (value / 1_000_000f).ToString("0.0").Replace(".", ",") + "ì";

        // 10ì+
        return (value / 1_000_000).ToString("0") + "ì";
    }

    private string Format(int value, int divider, string suffix)
    {
        int result = value / divider;

        if (result % 1 == 0)
            return result.ToString("0") + suffix;

        return result.ToString("0.#") + suffix;
    }
}
