using System.Collections.Generic;
using UnityEngine;

public class OvertimeFlood : OvertimeTileDestroyer
{
    public override void ChooseTiles()
    {
        var borderTiles = new List<BoardTile>();
        foreach (var position in BorderTilePositions())
            borderTiles.Add(Board.Instance.GetTileAtPosition(position));
        _tilesToBeDestroyed = borderTiles;
        base.ChooseTiles();
    }

    private List<Vector2> BorderTilePositions()
    {
        List<Vector2> borderTiles = new List<Vector2>();
        int distanceFromCenter = 3;
        for (distanceFromCenter = 3; distanceFromCenter >= 0; distanceFromCenter--)
        {
            if (!Board.Instance.GetTileAtPosition(new Vector2(distanceFromCenter, 0)).IsBlocked)
            {
                break;
            }
        }
        int longEdge = distanceFromCenter + 1;
        int shortEdge = distanceFromCenter;
        for (int x = -longEdge; x <= shortEdge; x++)
        {
            for (int y = -longEdge; y <= shortEdge; y++)
            {
                if (x == -longEdge || y == -longEdge || x == shortEdge || y == shortEdge)
                {
                    borderTiles.Add(new Vector2(x, y));
                }
            }
        }
        return borderTiles;
    }
}
