using System.Collections.Generic;

public class DrawPileUI : CardPileUI
{
    protected override List<UnitCard> GetCards()
    {
        return _deck.DrawPile;
    }
}
