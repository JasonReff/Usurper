using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VirtualBoard : IBoard<VirtualBoardTile>
{
    public float Evaluation;
    public List<VirtualUnit> VirtualUnits = new List<VirtualUnit>();
    public List<VirtualBoardTile> VirtualTiles = new List<VirtualBoardTile>();
    public Move Move;
    public bool KingInCheck;
    private VirtualUnit _previousUnitOnTile;
    private VirtualUnit _unitMoved;
    public VirtualBoard(Board board)
    {
        foreach (var tile in board.TileArray)
        {
            var virtualTile = new VirtualBoardTile(tile);
            VirtualTiles.Add(virtualTile);
        }
        foreach (var unit in board.PlayerUnits)
        {
            var newVirtualUnit = new VirtualUnit(this, unit);
            VirtualUnits.Add(newVirtualUnit);
            VirtualTiles.First(t => t._tilePosition == unit.Tile.TilePosition()).UnitOnTile = newVirtualUnit;
        }
        foreach (var unit in board.EnemyUnits)
        {
            VirtualUnit newVirtualUnit = new VirtualUnit(this, unit);
            VirtualUnits.Add(newVirtualUnit);
            VirtualTiles.First(t => t._tilePosition == unit.Tile.TilePosition()).UnitOnTile = newVirtualUnit;
        }
    }

    public VirtualBoard(VirtualBoard virtualBoard)
    {
        foreach (var tile in virtualBoard.TileArray)
        {
            VirtualTiles.Add(new VirtualBoardTile(tile));
        }
        foreach (var unit in virtualBoard.VirtualUnits)
        {
            var newVirtualUnit = new VirtualUnit(this, unit);
            VirtualUnits.Add(newVirtualUnit);
            VirtualTiles.First(t => t._tilePosition == unit.Tile.TilePosition()).UnitOnTile = newVirtualUnit;
        }
    }

    public VirtualBoard(VirtualBoard board, Move move)
    {
        foreach (var tile in board.VirtualTiles)
            VirtualTiles.Add(new VirtualBoardTile(tile, tile.UnitOnTile as VirtualUnit));
        foreach (var virtualUnit in board.VirtualUnits)
        {
            var newVirtualUnit = new VirtualUnit(this, virtualUnit);
            VirtualUnits.Add(newVirtualUnit);
            VirtualTiles.First(t => t._tilePosition == virtualUnit.Tile.TilePosition()).UnitOnTile = newVirtualUnit;
        }
        _previousUnitOnTile = null;
        if (board.VirtualUnits.Where(t => t.Tile.TilePosition() == move.NewTile.TilePosition()).Count() > 0)
            _previousUnitOnTile = board.VirtualUnits.First(t => t.Tile.TilePosition() == move.NewTile.TilePosition());
        Move = move;
        _unitMoved = null;
        if (VirtualUnits.Where(t => t.Tile._tilePosition == move.CurrentTile.TilePosition()).Count() > 0)
            _unitMoved = VirtualUnits.First(t => t.Tile._tilePosition == move.CurrentTile.TilePosition());
        var newTile = VirtualTiles.First(t => t.TilePosition() == move.NewTile.TilePosition());
        _unitMoved?.MoveToTile(newTile);
    }

    public VirtualBoard(VirtualBoard board, UnitPlacement placement)
    {
        foreach (var tile in board.VirtualTiles)
            VirtualTiles.Add(new VirtualBoardTile(tile));
        foreach (var virtualUnit in board.VirtualUnits)
        {
            var newVirtualUnit = new VirtualUnit(this, virtualUnit);
            VirtualUnits.Add(newVirtualUnit);
            VirtualTiles.First(t => t._tilePosition == virtualUnit.Tile.TilePosition()).UnitOnTile = newVirtualUnit;
        }
        Move = placement;
        var unit = new VirtualUnit(this, placement);
        VirtualUnits.Add(unit);
        unit.PlaceOnTile(placement.NewTile as VirtualBoardTile);
    }

    public List<VirtualBoard> GetAllPossibleBoards(UnitFaction faction, VirtualBoard currentBoard)
    {
        List<VirtualBoard> boards = new List<VirtualBoard>();
        foreach (var unit in currentBoard.VirtualUnits)
            if (unit.Faction == faction)
                foreach (var move in unit.UnitData.AllPossibleMoves(unit, unit.Tile, currentBoard))
                {
                    boards.Add(new VirtualBoard(currentBoard, move));
                }
        return boards;
    }

    public List<VirtualBoard> GetPossibleBoardsAfterUnitPlacement(IUnit unit, UnitFaction faction, VirtualBoard currentBoard)
    {
        List<VirtualBoard> boards = new List<VirtualBoard>();
        var enemyUnits = currentBoard.VirtualUnits.Where(t => t.Faction == faction).ToList();
        var king = enemyUnits.OrderByDescending(t => t.UnitData.IsKing).FirstOrDefault();
        if (king == null)
            return null;
        List<UnitPlacement> placements = (king.UnitData as KingUnitData).GetVirtualKingUnitPlacements(unit, king.Tile, currentBoard);
        foreach (var placement in placements)
            boards.Add(new VirtualBoard(currentBoard, placement));
        return boards;
    }

    public void CalculateEvaluation()
    {
        float eval = 0;
        foreach (var unit in VirtualUnits)
        {
            unit.CalculateEvaluation();
            unit.CheckIfTileTargeted();
            if (unit.InCheck)
                KingInCheck = true;
            if (unit.Faction == UnitFaction.Black)
                eval += unit.Evaluation;
            else
                eval -= unit.Evaluation;
        }

        Evaluation = eval;
    }

    public bool IsCaptureFree()
    {
        if (_previousUnitOnTile == null)
            return false;
        if (_previousUnitOnTile.UnitData.IsExplosive)
        {
            if (ShouldCaptureExplodingUnit())
                return true;
            else return false;
        }
        if (_previousUnitOnTile?.Faction != _unitMoved.Faction)
        {
            if (_unitMoved.CountAttackingEnemyUnits() < _unitMoved.CountDefendingUnits())
                return true;
        }
        return false;
    }

    public int CaptureValue()
    {
        if (_previousUnitOnTile == null)
            return 0;
        if (_previousUnitOnTile?.Faction != _unitMoved.Faction)
        {
            return _previousUnitOnTile.UnitData.Cost - _unitMoved.UnitData.Cost;
        }
        return 0;
    }

    public bool IsPieceHanging()
    {
        var enemyUnits = VirtualUnits.Where(t => t.Faction == UnitFaction.Black).ToList();
        foreach (var unit in enemyUnits)
        {
            if (unit.UnitData.IsExplosive)
                return false;
            if (unit.CountAttackingEnemyUnits() > unit.CountDefendingUnits())
                return true;
        }
        return false;
    }

    public bool IsKingNextToHangingBomb()
    {
        var enemyBombs = VirtualUnits.Where(t => t.Faction == UnitFaction.Black && t.UnitData.IsExplosive).ToList();
        var kings = VirtualUnits.Where(t => t.UnitData.IsKing && t.Faction == UnitFaction.Black).ToList();
        var adjacentBombs = new List<VirtualUnit>();
        foreach (var king in kings)
            adjacentBombs.AddRange(enemyBombs.Where(t => t.Tile.IsTileAdjacent(king.Tile) || t.Tile.IsTileDiagonal(king.Tile)));
        foreach (var adjacentBomb in adjacentBombs)
            if (adjacentBomb.CountAttackingEnemyUnits() > 0)
                return true;
        return false;
    }

    public bool ShouldCaptureExplodingUnit()
    {
        if (_unitMoved.UnitData.IsKing)
            return false;
        var unitsInRange = new List<VirtualUnit>();
        foreach (var unit in VirtualUnits)
        {
            if (unit.Tile.IsTileAdjacent(_previousUnitOnTile.Tile) || unit.Tile.IsTileDiagonal(_previousUnitOnTile.Tile))
            {
                unitsInRange.Add(unit);
            }
        }
        if (unitsInRange.Any(t => t.UnitData.IsKing && t.Faction == UnitFaction.Black))
            return false;
        int enemyValue = 0;
        int playerValue = 0;
        foreach (var enemyUnit in unitsInRange.Where(t => t.Faction == UnitFaction.Black).ToList())
            enemyValue += enemyUnit.UnitData.Cost;
        enemyValue += _unitMoved.UnitData.Cost;
        foreach (var playerUnit in unitsInRange.Where(t => t.Faction == UnitFaction.White).ToList())
            playerValue += playerUnit.UnitData.Cost;
        if (playerValue > enemyValue)
            return true;
        return false;
    }

    public int UnitsAttackedByPlacedPiece()
    {
        var placement = Move as UnitPlacement;
        var boardWithoutSummoningSickness = new VirtualBoard(this);
        VirtualUnit placedUnit = boardWithoutSummoningSickness.VirtualUnits.Where(t => t.Tile.TilePosition() == placement.NewTile.TilePosition()).First();
        placedUnit.SummoningSickness = false;
        return placedUnit.CountAttackableEnemyUnits();
    }

    public int CountEnemyPiecesAttacked()
    {
        int enemyPieces = 0;
        foreach (var unit in VirtualUnits.Where(t => t.Faction == UnitFaction.White).ToList())
            if (unit.CountAttackingEnemyUnits() > 0)
                enemyPieces++;
        return enemyPieces;
    }

    public VirtualBoardTile[] TileArray => VirtualTiles.ToArray();

    public VirtualBoardTile? GetTileAtPosition(Vector2 position)
    {
        if (VirtualTiles.Where(t => t._tilePosition == position).Count() == 1)
            return VirtualTiles.First(t => t._tilePosition == position);
        else return null;
    }
}
