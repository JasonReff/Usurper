using System.Collections.Generic;
using UnityEngine;

public class UIHighlighter : MonoBehaviour
{
    [SerializeField] private Color _moveColor, _rangedAttackColor;
    private void OnEnable()
    {
        CharacterManager.OnUnitSelected += HighlightMovableTiles;
        ShopManager.OnUnitSelected += HighlightPlaceableTiles;
        ShopManager.OnShopPhaseSkipped += RemoveAllHighlights;
        Unit.OnUnitMoved += RemoveAllHighlights;
        BoardTile.OnUnitPlaced += RemoveAllHighlights;
        BoardTile.OnTileSelected += RemoveHighlightsOnEmptyTileSelected;
        BoardVisualizer.OnBoardCreated += RemoveAllHighlights;
    }

    private void OnDisable()
    {
        CharacterManager.OnUnitSelected -= HighlightMovableTiles;
        ShopManager.OnUnitSelected -= HighlightPlaceableTiles;
        ShopManager.OnShopPhaseSkipped -= RemoveAllHighlights;
        Unit.OnUnitMoved -= RemoveAllHighlights;
        BoardTile.OnUnitPlaced -= RemoveAllHighlights;
        BoardTile.OnTileSelected -= RemoveHighlightsOnEmptyTileSelected;
        BoardVisualizer.OnBoardCreated -= RemoveAllHighlights;
    }

    private void HighlightMovableTiles(Unit unit)
    {
        if (unit == null)
            return;
        foreach (var tile in Board.Instance.TileArray)
        {
            if (unit as RangedUnit && (unit as RangedUnit).IsRangedAttack(tile))
            {
                tile.ShowHighlight(true, _rangedAttackColor);
            }
            else if (unit.CanMoveToTile(tile))
            {
                tile.ShowHighlight(true, _moveColor);
            }
            else tile.ShowHighlight(false, _moveColor);
        }
    }

    private void HighlightPlaceableTiles(List<BoardTile> placeableTiles)
    {
        foreach (var tile in Board.Instance.TileArray)
        {
            if (placeableTiles.Contains(tile))
                tile.ShowHighlight(true, _moveColor);
            else tile.ShowHighlight(false, _moveColor);
        }
    }

    private void RemoveAllHighlights()
    {
        foreach (var tile in Board.Instance.TileArray)
        {
            tile.ShowHighlight(false, _moveColor);
        }
    }

    private void RemoveAllHighlights(Unit unit)
    {
        foreach (var tile in Board.Instance.TileArray)
        {
            tile.ShowHighlight(false, _moveColor);
        }
    }

    private void RemoveHighlightsOnEmptyTileSelected(BoardTile boardTile)
    {
        if (boardTile.UnitOnTile == null)
        {
            RemoveAllHighlights();
        }
    }
}
