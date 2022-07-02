using System.Collections.Generic;
using UnityEngine;

public class OverlordManager : SpecialEnemyManager
{
    [SerializeField] private StartingDeck _specialDeck2, _specialDeck3;
    [SerializeField] private UnitData _shogunateRook;
    [SerializeField] private OvertimeManager _overtimeManager;
    private int phase = 1;

    protected override void Awake()
    {
        GetComponent<KingPlacer>().Deck.ResetDeck(_specialDeck);
        base.Awake();
        _overtimeManager = FindObjectOfType<OvertimeManager>();
        _overtimeManager.enabled = false;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OverlordUnit.OnOverlordKilled += OnOverlordKilled;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OverlordUnit.OnOverlordKilled -= OnOverlordKilled;
    }

    private void OnOverlordKilled(OverlordUnit overlord)
    {
        phase++;
        if (phase == 2)
        {
            _specialDeck = _specialDeck2;
            RespawnOverlord(overlord.Tile.TilePosition());
            SpawnRooks(overlord.Tile.TilePosition());
            ResetDeck();
        }
        else if (phase == 3)
        {
            _specialDeck = _specialDeck3;
            RespawnOverlord(overlord.Tile.TilePosition());
            StartEarthquake();
            ResetDeck();
        }
    }

    private void RespawnOverlord(Vector2 tilePosition)
    {
        var overlordTile = Board.Instance.GetTileAtPosition(tilePosition);
        overlordTile.PlaceUnit(_specialDeck.King, _faction);
        _kingUnit = overlordTile.UnitOnTile as KingUnit;
    }

    private void SpawnRooks(Vector2 overlordPosition)
    {
        var diagonalPositions = new List<Vector2>()
        {
            new Vector2(overlordPosition.x - 1, overlordPosition.y + 1),
            new Vector2(overlordPosition.x + 1, overlordPosition.y + 1),
            new Vector2(overlordPosition.x - 1, overlordPosition.y - 1),
            new Vector2(overlordPosition.x + 1, overlordPosition.y - 1)
        };
        foreach (var position in diagonalPositions)
        {
            var tile = Board.Instance.GetTileAtPosition(position);
            if (tile != null)
                tile.PlaceUnit(_shogunateRook, _faction);
        }
    }

    private void StartEarthquake()
    {
        _overtimeManager.enabled = true;
        _overtimeManager.ForceEarthquake();
    }
}