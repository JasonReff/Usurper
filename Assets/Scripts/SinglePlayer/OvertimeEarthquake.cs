using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OvertimeEarthquake : OvertimeTileDestroyer
{
    [SerializeField] private int _numberOfTiles = 4;
    public override void ChooseTiles()
    {
        _tilesToBeDestroyed = RandomTiles();
        base.ChooseTiles();
    }

    private List<BoardTile> RandomTiles()
    {
        List<BoardTile> tiles = Board.Instance.TileArray.Where(t => t.IsBlocked == false).ToList();
        var randomTiles = tiles.Pull(_numberOfTiles).ToList();
        return randomTiles;
    }
}