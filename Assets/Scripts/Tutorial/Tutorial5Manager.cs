using System.Collections;
using TMPro;
using UnityEngine;

public class Tutorial5Manager : TutorialManager
{
    [SerializeField] private TextMeshProUGUI _turnTimer;
    [SerializeField] private OvertimeManager _overtimeManager;
    [SerializeField] private GameObject _sectionFailedPanel;
    [SerializeField] private Unit _kingUnit;

    private void OnEnable()
    {
        Unit.OnUnitMoved += OnUnitMoved;
    }

    private void OnDisable()
    {
        Unit.OnUnitMoved -= OnUnitMoved;
    }

    public override void StartTutorial()
    {
        _turnTimer.text = "Turn : 40";
        _overtimeManager.ForceSinkhole();
        GameStateMachine.Instance.ChangeState(new MoveUnitState(GameStateMachine.Instance, UnitFaction.White));
    }

    private void OnUnitMoved()
    {
        _turnTimer.text = "Turn : 41";
        if (_kingUnit.Tile.IsTargeted)
        {
            SectionFailed();
        }
        else TutorialComplete();
    }

    private void SectionFailed()
    {
        StartCoroutine(FailedCoroutine());

        IEnumerator FailedCoroutine()
        {
            yield return new WaitForSeconds(1f);
            _sectionFailedPanel.SetActive(true);
        }

    }
}