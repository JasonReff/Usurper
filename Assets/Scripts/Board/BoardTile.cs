using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardTile : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Unit _unit;

    public Unit Unit { get => _unit; set => _unit = value; }

    public static Action<BoardTile> OnTileSelected;
    public static Action<Unit> OnUnitPlaced;

    public bool IsTileAdjacent(BoardTile otherTile)
    {
        Vector2 tilePosition = transform.localPosition;
        Vector2 otherTilePosition = otherTile.transform.localPosition;
        int distance = (int)(Mathf.Abs(tilePosition.x - otherTilePosition.x) + Mathf.Abs(tilePosition.y - otherTilePosition.y));
        if (distance == 1)
        {
            return true;
        }
        return false;
    }

    public bool IsTileDiagonal(BoardTile otherTile)
    {
        Vector2 tilePosition = transform.localPosition;
        Vector2 otherTilePosition = otherTile.transform.localPosition;
        Vector2 difference = otherTilePosition - tilePosition;
        if (Mathf.Abs(difference.x) == Mathf.Abs(difference.y))
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
}
