using System.Collections.Generic;

public class DrawPileUI : CardPileUI
{
    protected override List<UnitData> GetCards()
    {
        return _deck.DrawPile;
    }
}
