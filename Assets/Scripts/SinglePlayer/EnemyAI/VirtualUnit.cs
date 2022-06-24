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
    public bool SummoningSickness { get => _summoningSickness; set => _summoningSickness = value; }
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

    private VirtualUnit(VirtualBoard board, UnitFaction faction)
    {
        _board = board;
        _faction = faction;
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
        eval += EvaluateRangedTargets(moves);
        eval -= (CountAttackingEnemyUnits() * UnitData.Cost * 2);
        Evaluation = eval;
    }

    public int CountAttackableEnemyUnits()
    {
        var moves = UnitData.AllPossibleMoves(this, Tile, _board);
        int attackableEnemies = EvaluateVisibleEnemyUnits(moves);
        attackableEnemies += EvaluateRangedTargets(moves);
        return attackableEnemies;
    }

    private int EvaluateVisibleEnemyUnits(List<Move> moves)
    {
        int enemies = 0;
        if (UnitData is IDontCaptureOnMovement)
            return 0;
        foreach (var move in moves)
        {
            if (move.NewTile.UnitOnTile != null && move.NewTile.UnitOnTile.Faction != _faction)
                enemies += move.NewTile.UnitOnTile.UnitData.Cost / 2;
        }
        return enemies;
    }

    private int EvaluateRangedTargets(List<Move> moves)
    {
        if (!UnitData.GetType().IsSubclassOf(typeof(RangedUnitData)))
            return 0;
        int enemies = 0;
        foreach (var move in moves)
        {
            if ((UnitData as RangedUnitData).IsRangedAttack(this, move.CurrentTile, move.NewTile, _board))
                enemies += move.NewTile.UnitOnTile.UnitData.Cost * 2;
        }
        return enemies;
    }

    public int CountAttackingEnemyUnits()
    {
        int attackingEnemies = 0;
        var enemyUnits = _board.VirtualUnits.Where(t => t.Faction != _faction).ToList();
        var allEnemyMoves = new List<Move>();
        foreach (var unit in enemyUnits)
        {
            if (unit.UnitData is IHopPieces)
            {
                foreach (var position in (unit.UnitData as IHopPieces).HopSpaces())
                {
                    VirtualBoardTile hoppedTile = _board.GetTileAtPosition(unit.Tile.TilePosition() + position);
                    if (hoppedTile != null)
                        allEnemyMoves.Add(new Move(unit.UnitData, unit.Tile, hoppedTile));
                }
            }
            else
                allEnemyMoves.AddRange(unit.UnitData.AllPossibleMoves(unit, unit.Tile, _board));
        }
        foreach (var move in allEnemyMoves)
            if (move.NewTile.TilePosition() == Tile.TilePosition())
            {
                if (UnitData.GetType() == typeof(KingUnitData))
                    _inCheck = true;
                attackingEnemies++;
            }
        return attackingEnemies;
    }

    public int CountDefendingUnits()
    {
        int defendingUnits = 0;
        CountMeleeDefenders();
        CountRangedDefenders();
        return defendingUnits;

        void CountMeleeDefenders() 
        {
            var simulatedBoard = CreateBoardWithoutUnit(_board, this);
            var friendlyUnits = simulatedBoard.VirtualUnits.Where(t => t.Faction == _faction && !(t is IDontCaptureOnMovement)).ToList();
            var allFriendlyMoves = new List<Move>();
            foreach (var unit in friendlyUnits)
            {
                allFriendlyMoves.AddRange(unit.UnitData.AllPossibleMoves(unit, unit.Tile, simulatedBoard));
            }
            foreach (var move in allFriendlyMoves)
            {
                if (move.NewTile.TilePosition() == Tile.TilePosition())
                    defendingUnits++;
            }
        }

        void CountRangedDefenders()
        {
            var simulatedBoard = CreateBoardWithoutUnit(_board, this);
            var friendlyUnits = simulatedBoard.VirtualUnits.Where(t => t.Faction == _faction && t.UnitData is RangedUnitData).ToList();
            var allFriendlyMoves = new List<Move>();
            var tile = simulatedBoard.GetTileAtPosition(Tile.TilePosition());
            var simulatedEnemy = new VirtualUnit(simulatedBoard, UnitFaction.Player);
            simulatedEnemy.Tile = tile;
            tile.UnitOnTile = simulatedEnemy;
            foreach (var unit in friendlyUnits)
            {
                allFriendlyMoves.AddRange(unit.UnitData.AllPossibleMoves(unit, unit.Tile, simulatedBoard));
            }
            foreach (var move in allFriendlyMoves)
            {
                if (move.NewTile.TilePosition() == Tile.TilePosition())
                    defendingUnits++;
            }
        }
    }

    private VirtualBoard CreateBoardWithoutUnit(VirtualBoard boardWithUnit, VirtualUnit unitToRemove)
    {
        var simulatedBoard = new VirtualBoard(boardWithUnit);
        var correspondingUnit = simulatedBoard.VirtualUnits.First(t => t.Tile.TilePosition() == unitToRemove.Tile.TilePosition());
        simulatedBoard.VirtualUnits.Remove(correspondingUnit);
        simulatedBoard.GetTileAtPosition(unitToRemove.Tile.TilePosition()).UnitOnTile = null;
        return simulatedBoard;
    }

    public void CheckIfTileTargeted()
    {
        if (Tile.IsTargeted)
            _inCheck = true;
    }

    public void MoveToTile<T>(T tile) where T : IBoardTile
    {
        var otherUnit = tile.UnitOnTile as VirtualUnit;
        if (UnitData.GetType().IsSubclassOf(typeof(RangedUnitData)))
        {
            if((UnitData as RangedUnitData).IsRangedAttack(this, Tile, tile, _board))
            {
                CaptureUnit(otherUnit);
                return;
            }
        }
        else if (UnitData.GetType() == typeof(JesterUnitData))
        {
            var hoppedUnit = (UnitData as JesterUnitData).HoppedUnit(this, Tile, tile, _board);
            if (hoppedUnit != null)
            {
                CaptureUnit(hoppedUnit as VirtualUnit);
                return;
            }
        }
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
