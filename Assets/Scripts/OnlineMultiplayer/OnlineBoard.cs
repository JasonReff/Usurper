﻿using Photon.Pun;
using System;
using UnityEngine;

public class OnlineBoard : Board
{
    public static Action<BoardTile> OnTileSelected;

    protected override void OnEnable()
    {
        OnlineUnitCallbacks.OnlineUnitPlaced += AddUnitToList;
        Unit.OnUnitDeath += RemoveUnitFromList;
    }

    protected override void OnDisable()
    {
        OnlineUnitCallbacks.OnlineUnitPlaced -= AddUnitToList;
        Unit.OnUnitDeath -= RemoveUnitFromList;
    }

    public void SelectTile(Vector2 tile)
    {
        OnTileSelected?.Invoke(GetTileAtPosition(tile));
    }
}
