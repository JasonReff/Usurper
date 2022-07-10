using System.Collections.Generic;

public class DrawPileUI : CardPileUI
{
    protected override List<UnitCard> GetCards()
    {
        foreach (var card in _deck.DrawPile)
            if (card.UnitData == null)
                return null;
        return _deck.DrawPile;
    }
}
