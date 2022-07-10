using UnityEngine;

public class Tutorial2Manager : TutorialManager
{
    [SerializeField] private PlayerDeck _tutorialDeck;
    [SerializeField] private StartingDeck _tutorialStartingDeck;
    [SerializeField] private GameObject _nextPanel;
    [SerializeField] private ShopManager _shop;
    private void OnEnable()
    {
        BoardTile.OnUnitPlaced += OnUnitPlaced;
        BoardTile.OnTileSelected += OnBoardTileSelected;
        PurchaseableUnit.OnUnitSelected += OnCardSelected;
    }

    private void OnDisable()
    {
        BoardTile.OnUnitPlaced -= OnUnitPlaced;
        PurchaseableUnit.OnUnitSelected -= OnCardSelected;
        BoardTile.OnTileSelected -= OnBoardTileSelected;
    }

    private void OnUnitPlaced(Unit unit)
    {
        TutorialComplete();
    }

    public override void StartTutorial()
    {
        _tutorialDeck.ResetDeck(_tutorialStartingDeck);
        GameStateMachine.Instance.ChangeState(new BuyUnitState(GameStateMachine.Instance, UnitFaction.White));
    }

    private void OnCardSelected(PurchaseableUnit unit)
    {
        _nextPanel.SetActive(true);
    }

    private void OnBoardTileSelected(BoardTile tile)
    {
        if (_shop.Unit == null)
        {
            _nextPanel.SetActive(false);
        }
        else if (tile.UnitOnTile != null)
        {
            _nextPanel.SetActive(false);
        }
    }
}
