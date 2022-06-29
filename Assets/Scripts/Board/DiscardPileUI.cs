using System.Collections.Generic;

public class DiscardPileUI : CardPileUI
{
    protected override List<UnitCard> GetCards()
    {
        return _deck.DiscardPile;
    }
}