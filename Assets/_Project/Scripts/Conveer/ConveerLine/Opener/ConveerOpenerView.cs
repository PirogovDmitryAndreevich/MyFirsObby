using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ConveerOpenerView : MonoBehaviour
{
    [SerializeField] private TriggerObserver _observer;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _red;
    [SerializeField] private Material _green;

    private PlayerProgress _progress;
    private ConveerLine _line;

    public TriggerObserver TriggerObserver => _observer;

    private void OnDestroy() => 
        _progress.PlayerState.ValueChanged -= SetView;

    public void Construct(ConveerLine line, PlayerProgress progress)
    {
        _progress = progress;
        _line = line;

        _progress.PlayerState.ValueChanged += SetView;

        SetText(_line.LineStaticInfo.UnblockPrice);
        SetView();
    }

    public void SetText(int unblockPrice) => 
        _name.text = $"{unblockPrice}$";

    public void SetRedColor() => 
        _renderer.material = _red;

    public void SetGreenColor() => 
        _renderer.material = _green;

    private void SetView()
    {
        if (_progress.PlayerState.Cash >= _line.LineStaticInfo.UnblockPrice)
            SetGreenColor();
        else
            SetRedColor();
    }
}