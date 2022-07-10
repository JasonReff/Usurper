using Photon.Pun;
using UnityEngine;

public class OnlineShopManager : ShopManager
{

    public void SetFaction(UnitFaction faction)
    {
        _faction = faction;    
    }

    public void SetFaction(UnitFaction faction, string name)
    {
        _faction = faction;
        if (_faction == UnitFaction.Black)
        {
            _ui.ShopParent.gameObject.SetActive(false);
            _king = Board.Instance.GetTileAtPosition(new UnityEngine.Vector2(0, 3)).UnitOnTile as KingUnit;
        }
            
    }

    public UnitFaction GetFaction()
    {
        return _faction;
    }

    public void SetUI(ShopUI ui)
    {
        _ui = ui;
        foreach (var cardPile in _ui.GetComponentsInChildren<CardPileUI>())
        {
            cardPile.SetManager(this);
            cardPile.Deck = _deck;
        }
    }

    public void SetKing(KingUnit king)
    {
        _king = king;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnlinePlayerManager.OnFactionAssigned += SetFaction;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnlineBoard.OnTileSelected -= OnTileSelected;
        OnlinePlayerManager.OnFactionAssigned -= SetFaction;
    }

    protected override void Subscribe()
    {
        PurchaseableUnit.OnUnitSelected += SelectUnit;
        OnlineBoard.OnTileSelected += OnTileSelected;
    }

    protected override void Unsubscribe()
    {
        PurchaseableUnit.OnUnitSelected -= SelectUnit;
        OnlineBoard.OnTileSelected -= OnTileSelected;
    }

    protected override void OnStateChange(GameState state)
    {
        if (!photonView.IsMine)
            return;
        base.OnStateChange(state);
    }

    protected override void OnBuyStateEnded(GameState state)
    {
        if (!photonView.IsMine)
            return;
        base.OnBuyStateEnded(state);
    }
}