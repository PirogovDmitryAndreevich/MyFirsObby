using UnityEngine;
using UnityEngine.UI;

public class ConveerShop : MonoBehaviour, IWindow
{
    [SerializeField] private WindowsID _id;

    [SerializeField] private Button _closeButton;
    [SerializeField] private Transform _slidingArea;

    private CanvasGroup _canvasGroup;

    public WindowsID WindowID => _id;
    public Transform ConveerStatsParent => _slidingArea;

    private void Awake()
    {
        _closeButton.onClick.AddListener(Hide);
        Hide();
    }

    private void OnDestroy() =>
        _closeButton?.onClick.RemoveListener(Hide);

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
