using UnityEngine;

public class Build : MonoBehaviour
{
    [SerializeField] private Transform _playerInitialPoint;
    [SerializeField] private ArsenalInitializer _arsenal;
    [SerializeField] private ATM _atm;
    [SerializeField] private MoneyBugRewardAdv _moneyBugRewardAdv;
    [SerializeField] private ConveyerController[] _conveyers; 

    public Transform PlayerInitialPoint => _playerInitialPoint;
    public ArsenalInitializer Arsenal => _arsenal;
    public ATM ATM => _atm;
    public MoneyBugRewardAdv MoneyBugAdv => _moneyBugRewardAdv;
    public ConveyerController[] Conveyers => _conveyers;
}
