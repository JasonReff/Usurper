public class OnlineKingPlacer : KingPlacer
{
    public KingUnit King { get => _king; }
    public override void Start()
    {
       
    }

    public override void PlaceKing()
    {
        base.PlaceKing();
        _king = Board.Instance.GetTileAtPosition(_startingTile).Unit as KingUnit;
    }
}