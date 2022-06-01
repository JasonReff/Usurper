using Photon.Pun;
using System;
using UnityEngine;

public class OnlineBoard : Board
{
    public static Action<BoardTile> OnTileSelected;

    public void SelectTile(Vector2 tile)
    {
        this.photonView.RPC("OnTileSelectedCallback", RpcTarget.All, tile as object);
    }

    [PunRPC]
    private void OnTileSelectedCallback(Vector2 tilePosition)
    {
        OnTileSelected?.Invoke(GetTileAtPosition(tilePosition));
    }
}
