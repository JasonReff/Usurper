using System.Collections.Generic;
using System.Linq;

public class VirtualUnit : IUnit
{
    public float Evaluation;
    public UnitData UnitData;
    public VirtualBoardTile Tile;
    private UnitFaction _faction;
    private VirtualBoard _board;
    private bool _summoningSickness = true;
    private bool _inCheck;
    public bool SummoningSickness { get => _summoningSickness; }
    public VirtualUnit(VirtualBoard board, Unit unit)
    {
        _board = board;
        UnitData = unit.UnitData;
        Tile = board.GetTileAtPosition(unit.Tile.TilePosition());
        Tile.UnitOnTile = this;
        _summoningSickness = unit.SummoningSickness;
        _faction = unit.Faction;
    }

    public VirtualUnit(VirtualBoard board, VirtualUnit unit)
    {
        _board = board;
        UnitData = unit.UnitData;
        Tile = board.GetTileAtPosition(unit.Tile.TilePosition());
        Tile.UnitOnTile = this;
        _summoningSickness = unit.SummoningSickness;
        _faction = unit.Faction;
    }

    public VirtualUnit(VirtualBoard board, UnitPlacement placement)
    {
        _board = board;
        UnitData = placement.Unit.UnitData;
        Tile = board.GetTileAtPosition(placement.NewTile.TilePosition());
        Tile.UnitOnTile = this;
        _summoningSickness = placement.Unit.SummoningSickness;
        _faction = placement.Unit.Faction;
    }

    public UnitFaction Faction {get => _faction;}

    UnitData IUnit.UnitData => UnitData;

    public bool InCheck { get => _inCheck; }

    public void CalculateEvaluation()
    {
        int eval = UnitData.Cost;
        var moves = UnitData.AllPossibleMoves(this, Tile, _board);
        eval += moves.Count;
        eval += EvaluateVisibleEnemyUnits(moves);
        eval -= (CountAttackingEnemyUnits() * UnitData.Cost * 2);
        Evaluation = eval;
    }

    private int EvaluateVisibleEnemyUnits(List<Move> moves)
    {
        int enemies = 0;
        foreach (var move in moves)
        {
            if (move.NewTile.UnitOnTile != null && move.NewTile.UnitOnTile.Faction != _faction)
                enemies += move.NewTile.UnitOnTile.UnitData.Cost / 2;
        }
        return enemies;
    }

    private int CountAttackingEnemyUnits()
    {
        int attackingEnemies = 0;
        var enemyUnits = _board.VirtualUnits.Where(t => t.Faction != _faction).ToList();
        var allEnemyMoves = new List<Move>();
        foreach (var unit in enemyUnits)
            allEnemyMoves.AddRange(unit.UnitData.AllPossibleMoves(unit, unit.Tile, _board));
        foreach (var move in allEnemyMoves)
            if (move.NewTile.TilePosition() == Tile.TilePosition())
            {
                if (UnitData.GetType() == typeof(KingUnitData))
                    _inCheck = true;
                attackingEnemies++;
            }
                
        return attackingEnemies;
    }

    public void MoveToTile<T>(T tile) where T : IBoardTile
    {
        var otherUnit = tile.UnitOnTile as VirtualUnit;
        Tile.UnitOnTile = null; //clear previous position
        Tile = tile as VirtualBoardTile;
        if (otherUnit != null)
            CaptureUnit(otherUnit);
        Tile.UnitOnTile = this;
    }

    public void PlaceOnTile(VirtualBoardTile tile)
    {
        Tile = tile;
        tile.UnitOnTile = this;
    }

    public void CaptureUnit(VirtualUnit otherUnit)
    {
        _board.VirtualUnits.Remove(otherUnit);
    }
}
