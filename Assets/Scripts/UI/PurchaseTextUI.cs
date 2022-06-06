using TMPro;
using UnityEngine;
using Photon.Pun;

public class PurchaseTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _purchaseText;
    private UnitFaction _faction;
    private void OnEnable()
    {
        GameStateMachine.OnStateChanged += OnPurchaseState;
        OnlinePlayerManager.OnFactionAssigned += SetFaction;
    }

    private void OnDisable()
    {
        GameStateMachine.OnStateChanged -= OnPurchaseState;
        OnlinePlayerManager.OnFactionAssigned -= SetFaction;
    }

    public void SetFaction(UnitFaction faction, string name)
    {
        _faction = faction;
    }

    private void OnPurchaseState(GameState state)
    {
        if (state.Faction != _faction && state.GetType() == typeof(BuyUnitState))
        {
            ShowText();
        }
        else HideText();
    }

    private void ShowText()
    {
        _purchaseText.text = "Opponent purchasing unit...";
    }

    private void HideText()
    {
        _purchaseText.text = "";
    }
}