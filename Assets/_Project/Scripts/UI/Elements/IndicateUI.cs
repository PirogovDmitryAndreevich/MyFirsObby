using UnityEngine;

public class IndicateUI : MonoBehaviour, IIndicateUI
{
    public void Show() =>
        gameObject.SetActive(true);

    public void Hide() =>
        gameObject.SetActive(false);
}