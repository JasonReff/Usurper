using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/GoldPerTurn")]
public class GoldPerTurnUpgrade : SinglePlayerUpgrade
{
    [SerializeField] private PlayerDeck _playerDeck;
    [SerializeField] private int _goldIncrease;
    private int _defaultGold;

    public override void StartUpgrade()
    {
        _defaultGold = _playerDeck.GoldPerTurn;
        _playerDeck.GoldPerTurn += _goldIncrease;
    }

    public override void EndUpgrade()
    {
        _playerDeck.GoldPerTurn = _defaultGold;
    }
}