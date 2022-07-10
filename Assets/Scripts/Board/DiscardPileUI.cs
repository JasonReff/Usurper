using System.Collections.Generic;

public class DiscardPileUI : CardPileUI
{
    protected override List<UnitCard> GetCards()
    {
        foreach (var card in _deck.DiscardPile)
            if (card.UnitData == null)
                return null;
        return _deck.DiscardPile;
    }
}