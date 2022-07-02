using UnityEngine;

public class SpecialEnemyManager : EnemyAIManager
{
    [SerializeField] protected StartingDeck _specialDeck;
    [SerializeField] private string _bossName;

    public string BossName { get => _bossName; }

    public virtual void ResetDeck()
    {
        Shop.Deck.ResetDeck(_specialDeck);
    }
}
