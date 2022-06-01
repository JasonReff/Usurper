public class OnlinePurchaseableUnit : PurchaseableUnit
{
    public override void SelectUnit()
    {
        if (!photonView.IsMine)
            return;
        base.SelectUnit();
    }
}