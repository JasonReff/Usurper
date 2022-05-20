using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] protected UnitFaction _faction;
    [SerializeField] protected PlayerDeck _deck;
    [SerializeField] private ShopUI _ui;
    [SerializeField] private KingUnit _king;
    protected int _money = 3;
    protected PurchaseableUnit _unit;
    public static Action<Unit> OnUnitPurchased;
    public static Action OnShopPhaseSkipped;

    public int Money { get => _money; }

    private void OnEnable()
    {
        BoardTile.OnUnitPlaced += SetKing;
        GameStateMachine.OnStateChanged += OnStateChange;
        KingUnit.OnMoneyGenerated += GainMoney;
        BuyUnitState.OnBuyUnitStateEnded += OnBuyStateEnded;
    }

    private void OnDisable()
    {
        BoardTile.OnUnitPlaced -= SetKing;
        GameStateMachine.OnStateChanged -= OnStateChange;
        KingUnit.OnMoneyGenerated -= GainMoney;
        BuyUnitState.OnBuyUnitStateEnded -= OnBuyStateEnded;
    }

    private void SetKing(Unit unit)
    {
        if (unit.GetType() == typeof(KingUnit) && unit.Faction == _faction)
            _king = (KingUnit)unit;
    }

    private void OnStateChange(GameState state)
    {
        if (state.GetType() == typeof(BuyUnitState) && state.Faction == _faction)
        {
            SetupShop();
        }
    }

    private void OnBuyStateEnded(GameState state)
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
        _ui.UpdatePileCounts(_deck);
        ShopUI.OnCantAffordPurchase += OnCantAffordPurchase;
    }

    private void GainMoney(UnitFaction faction)
    {
        if (faction == _faction)
            _money++;
    }

    protected void Subscribe()
    {
        PurchaseableUnit.OnUnitSelected += SelectUnit;
        BoardTile.OnTileSelected += OnTileSelected;
    }

    private void Unsubscribe()
    {
        PurchaseableUnit.OnUnitSelected -= SelectUnit;
        BoardTile.OnTileSelected -= OnTileSelected;
    }

    private void SelectUnit(PurchaseableUnit unit)
    {
        if (unit.Cost <= _money)
            _unit = unit;
    }

    private void OnTileSelected(BoardTile tile)
    {
        if (_unit != null && IsTileUnoccupiedAndNextToKing(tile))
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

    private bool IsTileUnoccupiedAndNextToKing(BoardTile tile)
    {
        if (tile.Unit != null)
            return false;
        if (!tile.IsTileAdjacent(_king.Tile) && !tile.IsTileDiagonal(_king.Tile))
            return false;
        return true;
            
    }

    protected virtual void PurchaseUnit()
    {
        _money -= _unit.Cost;
    }

    public void SkipShopPhase()
    {
        _deck.DiscardUnits();
        OnShopPhaseSkipped?.Invoke();
    }

    

    private void OnCantAffordPurchase()
    {
        _unit = null;
    }

    
}
