using System.Collections.Generic;
using UnityEngine;

public class OvertimeSinkhole : OvertimeTileDestroyer
{
    public override void ChooseTiles()
    {
        var centerTiles = new List<BoardTile>();
        foreach (var position in CenterTilePositions())
            centerTiles.Add(Board.Instance.GetTileAtPosition(position));
        _tilesToBeDestroyed = centerTiles;
        base.ChooseTiles();
    }

    private List<Vector2> CenterTilePositions()
    {
        List<Vector2> centerTiles = new List<Vector2>();
        int distanceFromCenter = 0;
        for (distanceFromCenter = 0; distanceFromCenter <= 3; distanceFromCenter++)
        {
            if (!Board.Instance.GetTileAtPosition(new Vector2(distanceFromCenter, 0)).IsBlocked)
            {
                break;
            }
        }
        int shortEdge = distanceFromCenter;
        int longEdge = distanceFromCenter + 1;
        for (int x = -longEdge; x <= shortEdge; x++)
        {
            for (int y = -longEdge; y <= shortEdge; y++)
            {
                if (x == -longEdge || y == -longEdge || x == shortEdge || y == shortEdge)
                {
                    centerTiles.Add(new Vector2(x, y));
                }
            }
        }
        return centerTiles;
    }
}