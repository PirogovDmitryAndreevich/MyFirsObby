using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MoneyBugRewardAdv : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private RewardType _rewardID;
    [SerializeField] private TMP_Text _text;

    private RewardAdvPopup _popup;
    private bool _isCharacterOnTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character character))
        {
            if (_isCharacterOnTrigger == false)
            {
                _isCharacterOnTrigger = true;
                ShowPopup();
            }
        }
    }

    private void OnTriggerExit(Collider other) => 
        _isCharacterOnTrigger = false;

    public void Construct(RewardAdvPopup popup, int rewardCount)
    {
        _popup = popup;
        _text.text = rewardCount.ToString();
    }

    private void ShowPopup() => 
        _popup.Show(_rewardID);
}
