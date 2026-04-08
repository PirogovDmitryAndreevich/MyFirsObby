using TMPro;
using UnityEngine;

public class ConveerStat : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private ConveerStatButton _updateCooldownButton;
    [SerializeField] private ConveerStatButton _updateLevelButton;

    private ConveerLineProgress _progressLine;
    private IConveerUpdaterService _updateService;

    private void OnDestroy()
    {
        _progressLine.ConveerOpened -= SetState;
    }

    public void Construct(IPersistentProgressService progress, IConveerUpdaterService updaterService, ConveerStaticInfo staticData, ConveerLineProgress progressLine)
    {
        _progressLine = progressLine;
        _updateService = updaterService;

        _updateCooldownButton.Construct(progress, _updateService, progressLine, staticData);
        _updateLevelButton.Construct(progress, _updateService, progressLine, staticData);

        _progressLine.ConveerOpened += SetState;
        SetState();
    }

    private void SetState()
    {
        gameObject.SetActive(_progressLine.IsOpened);
    }
}
