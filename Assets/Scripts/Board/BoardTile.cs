using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardTile : MonoBehaviour, IPointerDownHandler, IBoardTile
{
    [SerializeField] private Unit _unit;

    public IUnit Unit { get => _unit; set => _unit = value as Unit; }

    public static Action<BoardTile> OnTileSelected;
    public static Action<Unit> OnUnitPlaced;

    public bool IsTileAdjacent(IBoardTile otherTile)
    {
        Vector2 tilePosition = transform.localPosition;
        Vector2 otherTilePosition = otherTile.TilePosition();
        int distance = (int)(Mathf.Abs(tilePosition.x - otherTilePosition.x) + Mathf.Abs(tilePosition.y - otherTilePosition.y));
        if (distance == 1)
        {
            return true;
        }
        return false;
    }

    public bool IsTileDiagonal(IBoardTile otherTile)
    {
        BoardTile otherBoardTile = otherTile as BoardTile;
        Vector2 tilePosition = transform.localPosition;
        Vector2 otherTilePosition = otherBoardTile.transform.localPosition;
        Vector2 difference = otherTilePosition - tilePosition;
        if (Mathf.Abs(difference.x) == Mathf.Abs(difference.y) && Mathf.Abs(difference.x) == 1)
            return true;
        else return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTileSelected?.Invoke(this);
    }

    public void PlaceUnit(UnitData unitData, UnitFaction faction)
    {
        var unit = Instantiate(unitData.Unit, transform.position, Quaternion.identity, transform);
        unit.Faction = faction;
        _unit = unit;
        unit.Tile = this;
        unit.SetSprite(unitData.GetSprite(faction));
        OnUnitPlaced?.Invoke(unit);
    }

    public Vector2 TilePosition()
    {
        return transform.localPosition;
    }
}

public interface IBoardTile
{
    public IUnit Unit { get; set; }
    public Vector2 TilePosition();
    public bool IsTileAdjacent(IBoardTile otherTile);
    public bool IsTileDiagonal(IBoardTile otherTile);
}
