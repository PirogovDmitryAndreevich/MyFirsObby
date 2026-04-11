using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConveerInitializer : MonoBehaviour, ISavedProgressReader
{
    [SerializeField] private ConveerView _view;
    [SerializeField] private ConveerFinisher _finisher;
    [SerializeField] private ConveerLine[] _conveerLines;
    
    private IStaticDataService _staticData;
    private IConveerUpdaterService _updater;
    private IPoolService _poolService;
    public Dictionary<ConveerNumbers, ConveerLine> Lines = new();

    public ConveerFinisher Finisher => _finisher;
    public ConveerView ConveerView => _view;

    public void Construct( IStaticDataService staticData, IConveerUpdaterService updater,IPoolService poolService)
    {
        _staticData = staticData;
        _updater = updater;
        _poolService = poolService;
        Lines = _conveerLines.ToDictionary(x => x.Numbers, x => x);
    }

    public void LoadProgress(PlayerProgress progress) => 
        InitLines(progress);

    private void InitLines(PlayerProgress progress)
    {
        foreach (ConveerLineProgress line in progress.Conveers.ConveerLines)        
            InitLine(progress, line, _staticData.ForConveers(line.Numbers));        
    }

    private void InitLine(PlayerProgress progress, ConveerLineProgress lineProgress, ConveerStaticInfo staticData) => 
        Lines[lineProgress.Numbers].Construct(lineProgress, staticData, progress, _updater, _poolService);
}
