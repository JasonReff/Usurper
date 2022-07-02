using Photon.Pun;

public class OnlineCheckUIManager : CheckUIManager
{

    protected override void OnEnable()
    {
        base.OnEnable();
        OnlineUnitCallbacks.OnlineUnitMoved += LookForChecks;
        OnlineUnitCallbacks.OnlineUnitPlaced += LookForChecks;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        OnlineUnitCallbacks.OnlineUnitMoved -= LookForChecks;
        OnlineUnitCallbacks.OnlineUnitPlaced -= LookForChecks;
    }

    protected override void LookForChecks()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != 1)
            return;
        base.LookForChecks();
    }

    private void LookForChecks(Unit unit)
    {
        LookForChecks();
    }

    protected override void DisplayChecks(bool isKingInCheck)
    {
        base.DisplayChecks(isKingInCheck);
        photonView.RPC("OnCheckDisplayedRPC", RpcTarget.Others, new object[] { isKingInCheck });
    }

    [PunRPC]
    private void OnCheckDisplayedRPC(bool isKingInCheck)
    {
        base.DisplayChecks(isKingInCheck);
    }
}