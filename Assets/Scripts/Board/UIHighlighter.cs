using System.Collections.Generic;
using UnityEngine;

public class UIHighlighter : MonoBehaviour
{
    private void OnEnable()
    {
        CharacterManager.OnUnitSelected += HighlightMovableTiles;
        ShopManager.OnUnitSelected += HighlightPlaceableTiles;
        ShopManager.OnShopPhaseSkipped += RemoveAllHighlights;
        Unit.OnUnitMoved += RemoveAllHighlights;
        BoardTile.OnUnitPlaced += RemoveAllHighlights;
        BoardVisualizer.OnBoardCreated += RemoveAllHighlights;
    }

    private void OnDisable()
    {
        CharacterManager.OnUnitSelected -= HighlightMovableTiles;
        ShopManager.OnUnitSelected -= HighlightPlaceableTiles;
        ShopManager.OnShopPhaseSkipped -= RemoveAllHighlights;
        Unit.OnUnitMoved -= RemoveAllHighlights;
        BoardTile.OnUnitPlaced -= RemoveAllHighlights;
        BoardVisualizer.OnBoardCreated -= RemoveAllHighlights;
    }

    private void HighlightMovableTiles(Unit unit)
    {
        if (unit == null)
            return;
        foreach (var tile in Board.Instance.TileArray)
        {
            if (unit.CanMoveToTile(tile))
            {
                tile.ShowHighlight(true);
            }
            else tile.ShowHighlight(false);
        }
    }

    private void HighlightPlaceableTiles(List<BoardTile> placeableTiles)
    {
        foreach (var tile in Board.Instance.TileArray)
        {
            if (placeableTiles.Contains(tile))
                tile.ShowHighlight(true);
            else tile.ShowHighlight(false);
        }
    }

    private void RemoveAllHighlights()
    {
        foreach (var tile in Board.Instance.TileArray)
        {
            tile.ShowHighlight(false);
        }
    }

    private void RemoveAllHighlights(Unit unit)
    {
        foreach (var tile in Board.Instance.TileArray)
        {
            tile.ShowHighlight(false);
        }
    }
}
