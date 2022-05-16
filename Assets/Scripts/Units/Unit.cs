using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] protected BoardTile _tile;
    [SerializeField] private UnitFaction _faction;
    [SerializeField] private SpriteRenderer _renderer;
    private float _deathTime = 0.3f;
    [SerializeField] private bool _summoningSickness = true;

    public BoardTile Tile { get => _tile; set => _tile = value; }
    public UnitFaction Faction { get => _faction; set => _faction = value; }

    public static event Action OnUnitMoved;

    private void OnEnable()
    {
        MoveUnitState.OnMoveStateEnded += RemoveSummoningSickness;
    }

    private void OnDisable()
    {
        MoveUnitState.OnMoveStateEnded -= RemoveSummoningSickness;
    }

    public void MoveToTile(BoardTile tile)
    {
        if (_summoningSickness)
            return;
        if (!CanMoveToTile(tile))
            return;
        var previousTile = _tile;
        if (previousTile)
            previousTile.Unit = null;
        _tile = tile;
        var otherUnit = _tile.Unit;
        if (otherUnit != null)
            CaptureUnit(otherUnit);
        _tile.Unit = this;
        transform.DOMove(_tile.transform.position, 1f);
        OnUnitMoved?.Invoke();
    }

    private void RemoveSummoningSickness()
    {
        _summoningSickness = false;
    }

    public void SetSprite(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }

    private void CaptureUnit(Unit unit)
    {
        StartCoroutine(unit.UnitDeath());
    }

    protected virtual bool CanMoveToTile(BoardTile tile)
    {
        if (tile.Unit != null && tile.Unit?._faction == _faction)
            return false;
        return true;
    }

    protected virtual IEnumerator UnitDeath()
    {
        _renderer.DOFade(0, _deathTime);
        yield return new WaitForSeconds(_deathTime);
        Destroy(gameObject);
    }

    protected Vector2 GetForwardVector()
    {
        if (_faction == UnitFaction.Player)
            return new Vector2(0, 1);
        else return new Vector2(0, -1);
    }

    protected List<BoardTile> GetFurthestUnobstructedPaths(List<Vector2> directions)
    {
        List<BoardTile> tiles = new List<BoardTile>();
        foreach (var direction in directions)
            tiles.AddRange(GetTilesInDirectionUntilObstructed(direction));
        return tiles;
    }

    protected List<BoardTile> GetTilesInDirectionUntilObstructed(Vector2 direction)
    {
        var tilePositions = new List<BoardTile>();
        var nextTile = Board.GetTileAtPosition((Vector2)_tile.transform.localPosition + direction);
        if (DoesNextTileContainEnemy(nextTile))
            tilePositions.Add(nextTile);
        while (nextTile != null && nextTile.Unit == null)
        {
            tilePositions.Add(nextTile);
            nextTile = Board.GetTileAtPosition((Vector2)nextTile.transform.localPosition + direction);
            if (DoesNextTileContainEnemy(nextTile))
            {
                tilePositions.Add(nextTile);
                break;
            }
        }
        return tilePositions;

        bool DoesNextTileContainEnemy(BoardTile tile)
        {
            if (nextTile != null && nextTile.Unit != null && nextTile.Unit.Faction != Faction)
                return true;
            else return false;
        }
    }

    protected List<Vector2> DiagonalVectors()
    {
        var vectors = new List<Vector2>() { new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1)};
        return vectors;
    }

    protected List<Vector2> OrthogonalVectors()
    {
        var vectors = new List<Vector2>() { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
        return vectors;
    }

    protected bool IsForwardTile(BoardTile tile)
    {
        if ((Vector2)tile.transform.localPosition == (Vector2)_tile.transform.localPosition + GetForwardVector())
            return true;
        else return false;
    }

    protected List<BoardTile> GetForwardDiagonalTiles()
    {
        var forwardVector = GetForwardVector();
        var leftDiagonal = (Vector2)_tile.transform.localPosition + forwardVector + new Vector2(-1, 0);
        var rightDiagonal = (Vector2)_tile.transform.localPosition + forwardVector + new Vector2(1, 0);
        return new List<BoardTile> { Board.GetTileAtPosition(leftDiagonal), Board.GetTileAtPosition(rightDiagonal) };
    }

    protected List<BoardTile> GetNeighboringTiles(List<Vector2> directions)
    {
        var tiles = new List<BoardTile>();
        foreach (var direction in directions)
        {
            var tile = Board.GetTileAtPosition((Vector2)_tile.transform.localPosition + direction);
            if (tile != null)
                tiles.Add(tile);
        }
        return tiles;
    }
}

public enum UnitFaction
{
    Player = 0,
    Enemy = 1,
}
