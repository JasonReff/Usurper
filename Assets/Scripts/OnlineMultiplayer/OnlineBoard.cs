using Photon.Pun;
using System;
using UnityEngine;

public class OnlineBoard : Board
{
    public static Action<BoardTile> OnTileSelected;

    public void SelectTile(Vector2 tile)
    {
        OnTileSelected?.Invoke(GetTileAtPosition(tile));
    }
}
