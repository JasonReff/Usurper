using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviourPunCallbacks
{
    [SerializeField] protected UnitFaction _faction;
    [SerializeField] protected PlayerDeck _deck;
    [SerializeField] protected ShopUI _ui;
    [SerializeField] protected KingUnit _king;
    [SerializeField] protected int _money = 3;
    [SerializeField] protected PurchaseableUnit _unit;
    public static Action<Unit> OnUnitPurchased;
    public static Action OnShopPhaseSkipped;
    public static Action<List<BoardTile>> OnUnitSelected;

    public int Money { get => _money; }
    public PlayerDeck Deck { get => _deck; }
    public PurchaseableUnit Unit { get => _unit; }

    protected virtual void OnEnable()
    {
        BoardTile.OnUnitPlaced += SetKing;
        GameStateMachine.OnStateChanged += OnStateChange;
        BuyUnitState.OnBuyUnitStateEnded += OnBuyStateEnded;
        BoardVisualizer.OnVirtualBoardCreated += ClearSelection;
        MerchantUnit.OnMerchantGainedGold += OnMerchantGainedMoney;
        _money = _deck.StartingGold;
    }

    protected virtual void OnDisable()
    {
        BoardTile.OnUnitPlaced -= SetKing;
        GameStateMachine.OnStateChanged -= OnStateChange;
        BuyUnitState.OnBuyUnitStateEnded -= OnBuyStateEnded;
        PurchaseableUnit.OnUnitSelected -= SelectUnit;
        BoardTile.OnTileSelected -= OnTileSelected;
        BoardVisualizer.OnVirtualBoardCreated -= ClearSelection;
        MerchantUnit.OnMerchantGainedGold -= OnMerchantGainedMoney;
    }

    private void SetKing(Unit unit)
    {
        if (unit.GetType() == typeof(KingUnit) && unit.Faction == _faction)
            _king = (KingUnit)unit;
    }

    protected virtual void OnStateChange(GameState state)
    {
        if (state.GetType() == typeof(BuyUnitState) && state.Faction == _faction)
        {
            GainMoney();
            SetupShop();
        }
    }

    protected virtual void OnBuyStateEnded(GameState state)
    {
        if (state.GetType() == typeof(BuyUnitState) && state.Faction == _faction)
        {
            Unsubscribe();
            _ui.HideShop();
            ShopUI.OnCantAffordPurchase -= OnCantAffordPurchase;
        }
    }

    public virtual void SetupShop()
    {
        Subscribe();
        _deck.DrawUnits();
        _ui.ShowShop();
        _ui.ShowCards(_deck.Hand, _faction);
        _ui.UpdatePurchaseButtons();
        _ui.UpdatePileCounts(_deck);
        ShopUI.OnCantAffordPurchase += OnCantAffordPurchase;
    }

    protected virtual void GainMoney()
    {
        _money += _deck.GoldPerTurn;
    }

    protected virtual void OnMerchantGainedMoney(UnitFaction faction)
    {
        if (faction != _faction)
            return;
        GainMoney();
        _ui.UpdateMoney();
    }

    protected virtual void Subscribe()
    {
        PurchaseableUnit.OnUnitSelected += SelectUnit;
        BoardTile.OnTileSelected += OnTileSelected;
    }

    protected virtual void Unsubscribe()
    {
        PurchaseableUnit.OnUnitSelected -= SelectUnit;
        BoardTile.OnTileSelected -= OnTileSelected;
    }

    protected void SelectUnit(PurchaseableUnit unit)
    {
        if (unit.Cost <= _money)
        {
            _unit = unit;
            OnUnitSelected?.Invoke(GetPlaceableTiles());
        }
            
    }

    protected void OnTileSelected(BoardTile tile)
    {
        if (_unit != null && IsTilePlaceable(tile))
        {
            PurchaseAndPlaceUnit(tile);
        }
        _unit = null;
    }

    public virtual void PurchaseAndPlaceUnit(BoardTile tile)
    {
        PurchaseUnit();
        tile.PlaceUnit(_unit.Data, _faction);
        _deck.DiscardUnits();
        OnUnitPurchased?.Invoke(_unit.Unit);
    }

    private bool IsTilePlaceable(BoardTile tile)
    {
        if (tile.UnitOnTile != null)
            return false;
        if (tile.IsBlocked)
            return false;
        if (_king == null)
            return false;
        if (!(_king.UnitData as KingUnitData).IsPlacementTile(_king, _king.Tile, tile, Board.Instance))
            return false;
        return true;
            
    }

    protected virtual void PurchaseUnit()
    {
        _money -= _unit.Cost;
        _unit.Card.NumberOfUses--;
    }

    public virtual void SkipShopPhase()
    {
        _deck.DiscardUnits();
        OnShopPhaseSkipped?.Invoke();
    }

    private List<BoardTile> GetPlaceableTiles()
    {
        var placeableTiles = new List<BoardTile>();
        foreach (var tile in Board.Instance.TileArray)
            if (IsTilePlaceable(tile))
                placeableTiles.Add(tile);
        return placeableTiles;
    }

    private void OnCantAffordPurchase()
    {
        ClearSelection();
    }

    private void ClearSelection()
    {
        _unit = null;
    }

    
}
