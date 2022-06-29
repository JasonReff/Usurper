using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnlineOvertimeManager : OvertimeManager
{

    protected override void SetTurn(GameState state)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            base.SetTurn(state);
        }
    }

    protected override void ChooseOvertime()
    {
        _chosenOvertime = _overtimes.Rand();
        if (_chosenOvertime is OvertimeShift)
            photonView.RPC("OvertimeChosenRPC", RpcTarget.Others, new object[] { _chosenOvertime.OvertimeName, (_chosenOvertime as OvertimeShift).SelectedWall });
        else photonView.RPC("OvertimeChosenRPC", RpcTarget.Others, new object[] { _chosenOvertime.OvertimeName, 0 });
        ChooseTiles();
    }

    protected override void ChooseTiles()
    {
        base.ChooseTiles();
        photonView.RPC("OnTilesChosenRPC", RpcTarget.Others, new object[] { _chosenOvertime.GetTilePositions().ToArray()});
    }

    protected override IEnumerator DisplayText()
    {
        photonView.RPC("OnTextDisplayedRPC", RpcTarget.Others);
        return base.DisplayText();
    }

    protected override void UpdateText()
    {
        int turnsUntilDestruction = _destroyDelay - (_turnsSinceOvertime % _destroyDelay);
        _turnsUntilDestructionTextbox.text = $"Turns until tiles destroyed: {turnsUntilDestruction / 2}";
        photonView.RPC("OnTileTextUpdatedRPC", RpcTarget.Others, new object[] { turnsUntilDestruction });
    }

    protected override void OvertimeEffect()
    {
        base.OvertimeEffect();
        photonView.RPC("OnOvertimeEffectRPC", RpcTarget.Others);
    }

    [PunRPC]
    private void OvertimeChosenRPC(string overtimeName, int wall = 0)
    {
        _chosenOvertime = _overtimes.First(t => t.OvertimeName == overtimeName);
        if (overtimeName == "Overtime: Shift")
        {
            var shift = _chosenOvertime as OvertimeShift;
            shift.SelectWall(wall);
        }
    }

    [PunRPC]
    private void OnTilesChosenRPC(Vector2[] tilePositions)
    {
        _chosenOvertime.SetTiles(tilePositions.ToList());
    }

    [PunRPC]
    private void OnTextDisplayedRPC()
    {
        StartCoroutine(base.DisplayText());
    }

    [PunRPC]
    private void OnTileTextUpdatedRPC(int turnsUntilTileDestruction)
    {
        _turnsUntilDestructionTextbox.text = $"Turns until tiles destroyed: {turnsUntilTileDestruction / 2}";
    }

    [PunRPC]
    private void OnOvertimeEffectRPC()
    {
        _chosenOvertime.DestroyTiles();
    }
}