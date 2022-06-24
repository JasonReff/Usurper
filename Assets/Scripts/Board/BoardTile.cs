using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BoardTile : MonoBehaviourPunCallbacks, IPointerDownHandler, IBoardTile, IPointerEnterHandler
{
    [SerializeField] protected Unit _unit;
    [SerializeField] protected SpriteRenderer _highlight;
    private bool _isBlocked, _isTargeted;

    public IUnit UnitOnTile { get => _unit; set => _unit = value as Unit; }
    public bool IsBlocked { get => _isBlocked; set => _isBlocked = value; }
    public bool IsTargeted { get => _isTargeted; set => _isTargeted = value; }

    public static Action<BoardTile> OnTileSelected;
    public static Action<Unit> OnUnitPlaced;
    public static Action<BoardTile> OnMouseOver;
    public static Action OnMouseExit;

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

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (BoardVisualizer.Instance.DemoBoard.gameObject.activeInHierarchy)
            return;
        OnTileSelected?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseOver?.Invoke(this);
    }

    public virtual void PlaceUnit(UnitData unitData, UnitFaction faction)
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

    public void ShowHighlight(bool highlight, Color highlightColor)
    {
        _highlight.enabled = highlight;
        _highlight.color = highlightColor;
    }
}

public interface IBoardTile
{
    public IUnit UnitOnTile { get; set; }
    public Vector2 TilePosition();
    public bool IsTileAdjacent(IBoardTile otherTile);
    public bool IsTileDiagonal(IBoardTile otherTile);
    public bool IsBlocked { get; set; }
    public bool IsTargeted { get; set; }
}
