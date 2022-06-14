using UnityEngine;

public class SpecialEnemyManager : EnemyAIManager
{
    [SerializeField] protected StartingDeck _specialDeck;

    public virtual void ResetDeck()
    {
        Shop.Deck.ResetDeck(_specialDeck);
    }
}