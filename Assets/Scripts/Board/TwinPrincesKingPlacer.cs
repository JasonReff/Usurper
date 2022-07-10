using UnityEngine;

public class TwinPrincesKingPlacer : KingPlacer
{
    public void PlaceKings(UnitData leftPrince, UnitData rightPrince)
    {
        _startingTile = new Vector2(-1, 3);
        var leftTile = Board.Instance.GetTileAtPosition(_startingTile);
        leftTile.PlaceUnit(leftPrince, _deck.Faction);
        _startingTile = new Vector2(0, 3);
        var rightTile = Board.Instance.GetTileAtPosition(_startingTile);
        rightTile.PlaceUnit(rightPrince, _deck.Faction);
        if (!Board.Instance.EnemyUnits.Contains(rightTile.UnitOnTile as Unit))
            Board.Instance.EnemyUnits.Add(rightTile.UnitOnTile as Unit);
        if (!Board.Instance.EnemyUnits.Contains(leftTile.UnitOnTile as Unit))
            Board.Instance.EnemyUnits.Add(leftTile.UnitOnTile as Unit);
        GetComponent<TwinPrincesAIManager>().SetKings(leftTile.UnitOnTile as KingUnit, rightTile.UnitOnTile as KingUnit);
    }

    public override void Start()
    {
        
    }
}