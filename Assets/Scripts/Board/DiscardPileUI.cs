using System.Collections.Generic;

public class DiscardPileUI : CardPileUI
{
    protected override List<UnitData> GetCards()
    {
        return _deck.DiscardPile;
    }
}