using UnityEngine;

public class FreePawnEncounter : SinglePlayerSpecialEncounter
{
    [SerializeField] private CardPool _cardPool;

    public override void StartEncounter()
    {
        foreach (var pawn in _cardPool.PawnPool)
        {
            pawn.Cost = 0;
        }
    }

    public override void EndEncounter()
    {
        foreach (var pawn in _cardPool.PawnPool)
        {
            pawn.Cost = 1;
        }
    }
}
