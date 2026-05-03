using UnityEngine;

public class Build : MonoBehaviour
{
    [SerializeField] private Transform _playerInitialPoint;
    [SerializeField] private ATM _atm;
    [SerializeField] private MoneyBugRewardAdv _moneyBugRewardAdv;
    [SerializeField] private ConveyerController[] _conveyers;
    [SerializeField] private WeaponUnblocker[] _weaponsUnblockers;

    public Transform PlayerInitialPoint => _playerInitialPoint;
    public ATM ATM => _atm;
    public MoneyBugRewardAdv MoneyBugAdv => _moneyBugRewardAdv;
    public ConveyerController[] Conveyers => _conveyers;
    public WeaponUnblocker[] Stands => _weaponsUnblockers;
}
