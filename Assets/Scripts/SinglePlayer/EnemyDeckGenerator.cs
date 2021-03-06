using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckGenerator : MonoBehaviour
{
    [SerializeField] private PlayerDeck _enemyDeck;
    [SerializeField] private CardPool _cardPool;

    public void SetDeck(int level)
    {
        _enemyDeck.SetGold(level, level / 3 + 1);
        _enemyDeck.ResetDeck(GenerateDeck());
    }

    private List<UnitData> GenerateDeck()
    {
        var random = new System.Random();
        var units = new List<UnitData>();
        _enemyDeck.King = _cardPool.KingPool.Rand();
        for (int i = 0; i < 3; i++)
        {
            var unit = _cardPool.PawnPool.Rand(random);
            units.Add(unit);
        }
        while (units.Count < 9)
        {
            var unit = _cardPool.UnitPool.Rand(random);
            units.Add(unit);
        }
        return units;
    }
}