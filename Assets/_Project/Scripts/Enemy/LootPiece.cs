using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LootPiece : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private AudioClip _pickedSound;

    private PlayerState _resources;
    private ISaveLoadService _save;
    private UIAudioHandler _audioHandler;
    private IPoolService _poolService;
    private Loot _loot;
    private bool _picked;
    public Collider Collider { get; private set; }

    public void Construct(PlayerState resources, ISaveLoadService saveLoadService, IPoolService poolService, UIAudioHandler audioHandler)
    {
        _poolService = poolService;
        _resources = resources;
        _save = saveLoadService;
        _audioHandler = audioHandler;

        Collider = GetComponent<Collider>();
    }

    public void Initialize(Loot loot)
    {
        _picked = false;
        _loot = loot;
        _text.text = _loot.Value.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
            Pickup();
    }

    private void Pickup()
    {
        if (_picked)
            return;

        _picked = true;

        _audioHandler.PlaySound(_pickedSound);
        _resources.AddCash(_loot.Value);

        _save.SaveProgress();
        _poolService.Return(this);
    }
}