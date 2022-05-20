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
    public VirtualBoard(Board board)
    {
        foreach (var tile in board.TileArray)
        {
            VirtualTiles.Add(new VirtualBoardTile(tile.TilePosition()));
        }
        foreach (var unit in board.PlayerUnits)
        {
            VirtualUnits.Add(new VirtualUnit(this, unit));
            VirtualTiles.First(t => t._tilePosition == unit.Tile.TilePosition()).Unit = unit;
        }
        foreach (var unit in board.EnemyUnits)
        {
            VirtualUnits.Add(new VirtualUnit(this, unit));
            VirtualTiles.First(t => t._tilePosition == unit.Tile.TilePosition()).Unit = unit;
        }
    }

    public VirtualBoard(VirtualBoard board, Move move)
    {
        foreach (var tile in board.VirtualTiles)
            VirtualTiles.Add(new VirtualBoardTile(tile, tile.Unit as VirtualUnit));
        foreach (var virtualUnit in board.VirtualUnits)
            VirtualUnits.Add(new VirtualUnit(this, virtualUnit));
        Move = move;
        VirtualUnit unit = null;
        if (VirtualUnits.Where(t => t.Tile._tilePosition == move.CurrentTile.TilePosition()).Count() > 0)
            unit = VirtualUnits.First(t => t.Tile._tilePosition == move.CurrentTile.TilePosition());
        var newTile = VirtualTiles.First(t => t.TilePosition() == move.NewTile.TilePosition());
        unit?.MoveToTile(newTile);
    }

    public VirtualBoard(VirtualBoard board, UnitPlacement placement)
    {
        foreach (var tile in board.VirtualTiles)
            VirtualTiles.Add(new VirtualBoardTile(tile));
        foreach (var virtualUnit in board.VirtualUnits)
            VirtualUnits.Add(new VirtualUnit(this, virtualUnit));
        Move = placement;
        var unit = new VirtualUnit(this, placement);
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
        var king = currentBoard.VirtualUnits.First(t => t.UnitData.GetType() == typeof(KingUnitData) && t.Faction == faction);
        List<UnitPlacement> placements = (king.UnitData as KingUnitData).GetPossibleUnitPlacements(unit, king.Tile, currentBoard);
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
            if (unit.InCheck)
                KingInCheck = true;
            if (unit.Faction == UnitFaction.Enemy)
                eval += unit.Evaluation;
            else
                eval -= unit.Evaluation;
        }
        Evaluation = eval;
    }

    public VirtualBoardTile[] TileArray => VirtualTiles.ToArray();

    public VirtualBoardTile? GetTileAtPosition(Vector2 position)
    {
        if (VirtualTiles.Where(t => t._tilePosition == position).Count() == 1)
            return VirtualTiles.First(t => t._tilePosition == position);
        else return null;
    }
}
