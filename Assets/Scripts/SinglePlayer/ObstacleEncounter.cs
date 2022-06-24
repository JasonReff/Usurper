using System.Collections.Generic;
using UnityEngine;

public class ObstacleEncounter : SinglePlayerSpecialEncounter
{
    [SerializeField] private List<BoardTile> _centerTiles;
    [SerializeField] private GameObject _obstaclePrefab;
    public override void StartEncounter()
    {
        foreach (var tile in _centerTiles)
        {
            Instantiate(_obstaclePrefab, tile.transform.position, Quaternion.identity);
            tile.IsBlocked = true;
        }
    }
}
