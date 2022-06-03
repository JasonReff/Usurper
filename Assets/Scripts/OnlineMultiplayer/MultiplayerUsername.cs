using UnityEngine;
using TMPro;
using Photon.Pun;

public class MultiplayerUsername : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _textbox;
    [SerializeField] private UnitFaction _faction;
    private void OnEnable()
    {
        OnlinePlayerManager.OnFactionAssigned += SetNameText;
    }

    private void OnDisable()
    {
        OnlinePlayerManager.OnFactionAssigned -= SetNameText;
    }

    private void SetNameText(UnitFaction faction, string name)
    {
        if (faction == _faction && name != null)
            _textbox.text = name;
        this.photonView.RPC("NameSetRPC", RpcTarget.Others, new object[] { faction, name });
    }

    [PunRPC]
    private void NameSetRPC(UnitFaction faction, string name)
    {
        SetNameText(faction, name);
    }
}