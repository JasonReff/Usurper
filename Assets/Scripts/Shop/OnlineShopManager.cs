using Photon.Pun;
public class OnlineShopManager : ShopManager
{

    public void SetFaction(UnitFaction faction)
    {
        _faction = faction;    
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

    protected override void OnDisable()
    {
        base.OnDisable();
        OnlineBoard.OnTileSelected -= OnTileSelected;
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