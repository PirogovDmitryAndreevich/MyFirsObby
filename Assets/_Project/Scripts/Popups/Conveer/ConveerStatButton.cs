using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConveerStatButton : MonoBehaviour
{
    [SerializeField] private ConveerUpdateType _type;
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _price;

    private ConveerStaticInfo _staticData;
    private IPersistentProgressService _progress;
    private ConveerLineProgress _progressLine;
    private IConveerUpdaterService _updaterService;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnLevelUpdate);
        _progressLine.ConveerLineUpdated += UpdatePrice;
        _progress.Progress.PlayerState.ValueChanged += UpdateState;

        UpdateState();
        UpdatePrice(_type);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnLevelUpdate);
        _progressLine.ConveerLineUpdated -= UpdatePrice;
        _progress.Progress.PlayerState.ValueChanged -= UpdateState;
    }

    public void Construct(IPersistentProgressService progress, IConveerUpdaterService updaterService, ConveerLineProgress progressLine ,ConveerStaticInfo staticData)
    {
        _staticData = staticData;
        _progress = progress;
        _progressLine = progressLine;
        _updaterService = updaterService;
    }

    private void UpdateState()
    {
        if (CheckUnblockButton())
            SetUnblockState();
        else
            SetBlockState();
    }

    private void UpdatePrice(ConveerUpdateType type)
    {
        if (type != _type)
            return;

        if (_progressLine.CanUpgrade(_type))
            _price.text = _staticData.GetNextLevelPrice(_progressLine.GetLevel(_type), _type).ToString();

        else
            _price.text = "MAX";
    }

    private bool CheckUnblockButton()
    {
        if (!_progressLine.CanUpgrade(_type))
            return false;

        int? nextLevelPrice = _staticData.GetNextLevelPrice(_progressLine.GetLevel(_type), _type);

        return nextLevelPrice.HasValue &&
               nextLevelPrice.Value <= _progress.Progress.PlayerState.Cash;
    }

    private void OnLevelUpdate() =>
        _updaterService.UpdateConveer(_progressLine,_type);

    private void SetBlockState() =>
        _button.interactable = false;

    private void SetUnblockState() =>
        _button.interactable = true;
}