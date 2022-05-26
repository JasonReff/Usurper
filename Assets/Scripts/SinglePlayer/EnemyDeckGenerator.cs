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
        var units = new List<UnitData>();
        for (int i = 0; i < 3; i++)
        {
            var random = new System.Random(i);
            var unit = _cardPool.PawnPool.Rand(random);
            units.Add(unit);
        }
        while (units.Count < 9)
        {
            var random = new System.Random(units.Count);
            var unit = _cardPool.UnitPool.Rand(random);
            units.Add(unit);
        }
        return units;
    }
}