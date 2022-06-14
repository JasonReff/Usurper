using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Movement")]
public class KingMovementUpgrade : SinglePlayerUpgrade
{
    public List<Vector2> AdditionalMovementSpaces;
    [SerializeField] private KingUnitData _kingUnitData;
    public override void StartUpgrade()
    {
        _kingUnitData.AddAdditionalMovement(AdditionalMovementSpaces);
    }

    public override void EndUpgrade()
    {
        _kingUnitData.ClearAdditionalMovementSpaces();
    }
}
