﻿using System.Collections.Generic;
using System.Linq;

public class EnemyShopManager : ShopManager
{
    public UnitData SelectedUnit;
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
        PurchaseUnit();
        tile.PlaceUnit(SelectedUnit, _faction);
        _deck.DiscardUnits();
    }

    public override void SkipShopPhase()
    {
        _deck.DiscardUnits();
    }
}