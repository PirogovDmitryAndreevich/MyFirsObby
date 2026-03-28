using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConveerManager : ISavedProgress
{
    private readonly ISaveLoadService _saveLoadService;
    private readonly IStaticDataService _staticData;
    private readonly Conveer _conveer;
    private Dictionary<ConveerNumbers, ConveerLineProgress> _lines;

    public ConveerManager(ISaveLoadService saveLoadService, IStaticDataService staticData, Conveer conveer)
    {
        _saveLoadService = saveLoadService;
        _staticData = staticData;
        _conveer = conveer;
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        foreach (ConveerLineProgress line in progress.Conveers.ConveerLines)
        {
            line.Level = _lines[line.Numbers].Level;
            line.CooldownLevel = _lines[line.Numbers].CooldownLevel;
        }
    }

    public void LoadProgress(PlayerProgress progress)
    {
        _lines = progress.Conveers.ConveerLines.ToDictionary(x => x.Numbers, x => x);
        InitConveer();
    }

    public void UpdateLevelConveer(ConveerNumbers number, int level)
    {
        ConveerStaticData staticData = _staticData.ForConveer();
        float cooldown = staticData.GetLevelCooldown(_lines[number].CooldownLevel);

        _lines[number].Level = CheckUpdate(staticData, _lines[number].Level, _lines[number].Level++);

        GameObject prefab = staticData.GetLevelPrefab(_lines[number].Level);

        _conveer.InitLine(number, _lines[number].IsOpened, cooldown, prefab);

        _saveLoadService.SaveProgress();
    }

    public void UpdateCooldownConveer(ConveerNumbers number, int level)
    {
        ConveerStaticData staticData = _staticData.ForConveer();
        GameObject prefab = staticData.GetLevelPrefab(_lines[number].Level);

        _lines[number].CooldownLevel = CheckUpdate(staticData, _lines[number].CooldownLevel, _lines[number].CooldownLevel++);

        float cooldown = staticData.GetLevelCooldown(_lines[number].CooldownLevel);

        _conveer.InitLine(number, _lines[number].IsOpened, cooldown, prefab);

        _saveLoadService.SaveProgress();
    }

    private void InitConveer()
    {
        _conveer.Init();
        ConveerStaticData staticData = _staticData.ForConveer();

        foreach (var lineProgress in _lines)
        {
            ConveerLineProgress line = lineProgress.Value;

            float speed = staticData.GetLevelCooldown(line.CooldownLevel);
            GameObject prefab = staticData.GetLevelPrefab(line.Level);

            _conveer.InitLine(line.Numbers, line.IsOpened, speed, prefab);
        }
    }

    private int CheckUpdate(ConveerStaticData staticData, int level, int newLevel)
    {
        if (newLevel > staticData.MaxLevel)
            return newLevel;
        else
            return level;
    }
}