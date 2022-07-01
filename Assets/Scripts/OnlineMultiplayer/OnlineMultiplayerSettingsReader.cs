using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class OnlineMultiplayerSettingsReader : MonoBehaviourPunCallbacks
{
    [SerializeField] private PrivateMatchSettings _matchSettings;
    public static Action<int> OnTimeSet;
    public static Action<bool, UnitFaction> OnFactionPreferenceSet;

    public void ReadSettings()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            if (_matchSettings.IsPrivateMatch)
            {
                this.photonView.RPC("OnSettingsReadCallback", RpcTarget.All, new object[] { _matchSettings.TimePerPlayer, _matchSettings.IsFactionPreferred, _matchSettings.PreferredFaction});
            }
        }
    }

    [PunRPC]
    private void OnSettingsReadCallback(int timePerPlayer, bool factionPreferred, UnitFaction preferredFaction)
    {
        OnTimeSet?.Invoke(timePerPlayer);
        OnFactionPreferenceSet?.Invoke(factionPreferred, preferredFaction);
    }
}
