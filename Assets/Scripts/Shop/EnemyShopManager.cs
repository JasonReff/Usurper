using System;
using System.Collections.Generic;
using System.Linq;

public class EnemyShopManager : ShopManager
{
    public static Action EnemyShopPhaseSkipped;
    public UnitData SelectedUnit;

    public void SetKing(KingUnit king)
    {
        _king = king;
    }

    public override void SetupShop()
    {
        Subscribe();
        _deck.DrawUnits();
    }

    public void SelectRandomShopCard()
    {
        SelectRandomCard();
        if (SelectedUnit == null)
            SkipShopPhase();
    }

    private void SelectRandomCard()
    {
        var cards = _deck.Hand.Where(t => t.Cost <= _money).ToList();
        if (cards.Count > 0)
        {
            var unitPurchased = cards.Rand();
            SelectedUnit = unitPurchased;
        }
        else
            SkipShopPhase();
    }

    public List<UnitData> GetPurchaseableCardsInHand()
    {
        var cards = _deck.Hand.Where(t => t.Cost <= _money).ToList();
        return cards;
    }

    protected override void PurchaseUnit()
    {
        _money -= SelectedUnit.Cost;
    }

    public override void PurchaseAndPlaceUnit(BoardTile tile)
    {
        if (SelectedUnit == null)
            return;
        PurchaseUnit();
        tile.PlaceUnit(SelectedUnit, _faction);
        _deck.DiscardUnits();
        SelectedUnit = null;
    }

    public override void SkipShopPhase()
    {
        _deck.DiscardUnits();
        EnemyShopPhaseSkipped?.Invoke();
    }

    protected override void OnMerchantGainedMoney(UnitFaction faction)
    {
        if (_faction != faction)
            return;
        GainMoney();
    }
}