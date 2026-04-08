using TMPro;
using UnityEngine;

public class ArsenalOpenerView : MonoBehaviour
{
    [SerializeField] private ArsenalInitializer _arsenal;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _red;
    [SerializeField] private Material _green;

    private void Start() => 
        SetText();

    public void SetText() =>
        _name.text = $"{_arsenal.UnblockPrice}$";

    public void SetRedColor() =>
        _renderer.material = _red;

    public void SetGreenColor() =>
        _renderer.material = _green;
}