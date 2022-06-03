using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BoardTile : MonoBehaviourPunCallbacks, IPointerDownHandler, IBoardTile, IPointerEnterHandler
{
    [SerializeField] protected Unit _unit;
    [SerializeField] protected SpriteRenderer _highlight;

    public IUnit Unit { get => _unit; set => _unit = value as Unit; }

    public static Action<BoardTile> OnTileSelected;
    public static Action<Unit> OnUnitPlaced;
    public static Action<Unit> OnMouseOver;
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
        if (_unit != null)
            OnMouseOver?.Invoke(_unit);
        else OnMouseExit?.Invoke();
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

    public void ShowHighlight(bool highlight)
    {
        _highlight.enabled = highlight;
    }
}

public interface IBoardTile
{
    public IUnit Unit { get; set; }
    public Vector2 TilePosition();
    public bool IsTileAdjacent(IBoardTile otherTile);
    public bool IsTileDiagonal(IBoardTile otherTile);
}
