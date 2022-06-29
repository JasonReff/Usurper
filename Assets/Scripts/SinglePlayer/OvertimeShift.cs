using System.Collections.Generic;
using UnityEngine;

public class OvertimeShift : OvertimeTileDestroyer
{
    private Wall _selectedWall;
    private bool _wallChosen;

    public int SelectedWall { get => (int)_selectedWall; }
    public override void ChooseTiles()
    {
        SelectWall();
        GetClosestEdge();
        base.ChooseTiles();
    }

    public void SelectWall(int wall)
    {
        _selectedWall = (Wall)wall;
        _wallChosen = true;
    }

    private void SelectWall()
    {
        if (_wallChosen)
            return;
        _selectedWall = (Wall)UnityEngine.Random.Range(0, 4);
        _wallChosen = true;
    }

    private void GetClosestEdge()
    {
        Vector2 directionFromEdge = new Vector2(0, 0);
        Vector2 edgeTilePosition = new Vector2(0, 0);
        switch (_selectedWall)
        {
            case Wall.Top:
                directionFromEdge = new Vector2(0, -1);
                edgeTilePosition = new Vector2(0, 3);
                break;
            case Wall.Left:
                directionFromEdge = new Vector2(1, 0);
                edgeTilePosition = new Vector2(-4, 0);
                break;
            case Wall.Bottom:
                directionFromEdge = new Vector2(0, 1);
                edgeTilePosition = new Vector2(0, -4);
                break;
            case Wall.Right:
                directionFromEdge = new Vector2(-1, 0);
                edgeTilePosition = new Vector2(3, 0);
                break;
        }
        for (int distanceFromEdge = 0; distanceFromEdge <= 7; distanceFromEdge++)
        {
            if (!Board.Instance.GetTileAtPosition(edgeTilePosition).IsBlocked)
            {
                break;
            }
            edgeTilePosition += directionFromEdge;
        }
        List<BoardTile> tiles = new List<BoardTile>();
        if (_selectedWall == Wall.Top || _selectedWall == Wall.Bottom)
        {
            for (int x = -4; x <= 3; x++)
            {
                tiles.Add(Board.Instance.GetTileAtPosition(new Vector2(x, edgeTilePosition.y)));
            }
        }
        else
        {
            for (int y = -4; y <= 3; y++)
            {
                tiles.Add(Board.Instance.GetTileAtPosition(new Vector2(edgeTilePosition.x, y)));
            }
        }
        _tilesToBeDestroyed = tiles;
    }

    private enum Wall
    {
        Top = 0,
        Left = 1,
        Bottom = 2,
        Right = 3
    }
}