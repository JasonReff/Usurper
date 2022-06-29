using System.Collections.Generic;
using UnityEngine;

public abstract class OvertimeTileDestroyer : MonoBehaviour
{
    protected List<BoardTile> _tilesToBeDestroyed = new List<BoardTile>();
    private List<GameObject> _tileTargets = new List<GameObject>();
    [SerializeField] private GameObject _destroyedTilePrefab, _tileTargetPrefab;
    [SerializeField] private int _chooseDelay;
    [SerializeField] private string _overtimeName;

    public string OvertimeName { get => _overtimeName; }
    public int ChooseDelay { get => _chooseDelay; }
    public List<BoardTile> TilesToBeDestroyed { get => _tilesToBeDestroyed; }

    public virtual void ChooseTiles()
    {
        foreach (var tile in _tilesToBeDestroyed)
        {
            tile.IsTargeted = true;
            var target = Instantiate(_tileTargetPrefab, tile.transform.position, Quaternion.identity, tile.transform);
            _tileTargets.Add(target);
        }
    }

    public void SetTiles(List<Vector2> tilePositions)
    {
        foreach (var position in tilePositions)
        {
            var tile = Board.Instance.GetTileAtPosition(position);
            _tilesToBeDestroyed.Add(tile);
            tile.IsTargeted = true;
            var target = Instantiate(_tileTargetPrefab, tile.transform.position, Quaternion.identity, tile.transform);
            _tileTargets.Add(target);
        }
    }

    public List<Vector2> GetTilePositions()
    {
        var tilePositions = new List<Vector2>();
        foreach (var tile in _tilesToBeDestroyed)
            tilePositions.Add(tile.TilePosition());
        return tilePositions;
    }

    public void DestroyTiles()
    {
        foreach (var tile in _tilesToBeDestroyed)
        {
            var unit = tile.UnitOnTile as Unit;
            if (unit != null)
                unit.TriggerUnitDeath();
            Instantiate(_destroyedTilePrefab, tile.transform.position, Quaternion.identity, tile.transform);
            tile.IsBlocked = true;
            tile.IsTargeted = false;
        }
        foreach (var target in _tileTargets)
            Destroy(target);
        _tileTargets.Clear();
    }
}
