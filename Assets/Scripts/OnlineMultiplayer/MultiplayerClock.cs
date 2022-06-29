using System.Collections;
using UnityEngine;

public class MultiplayerClock : MonoBehaviour
{
    [SerializeField] private int _startingTime = 600;
    [SerializeField] private UnitFaction _faction;
    private int _currentTime;
    private bool _isClockOn, _isClockStarted;
    private Coroutine _clock;

    public int CurrentTime { get => _currentTime; }


    private void OnEnable()
    {
        GameStateMachine.OnStateChanged += OnStateChange;
        OnlineMultiplayerSettingsReader.OnTimeSet += SetTime;
    }

    private void OnDisable()
    {
        GameStateMachine.OnStateChanged -= OnStateChange;
        OnlineMultiplayerSettingsReader.OnTimeSet -= SetTime;
    }

    private void Start()
    {
        if (_startingTime == 0)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    private void OnStateChange(GameState state)
    {
        if (state.GetType() != typeof(StartGameState) && state.GetType() != typeof(PlayerWonState) && state.GetType() != typeof(PlayerLeftState) && state.Faction == _faction)
        {
            ResumeClock();
        }
        else StopClock();
    }

    private void SetTime(int time)
    {
        _startingTime = time;
        if (_startingTime == 0)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    private void StartClock()
    {
        _currentTime = _startingTime;
        _isClockStarted = true;
    }

    private void ResumeClock()
    {
        if (_isClockStarted == false)
            StartClock();
        if (_isClockOn)
            return;
        _isClockOn = true;
        _clock = StartCoroutine(ClockCoroutine());
    }

    private IEnumerator ClockCoroutine()
    {
        yield return new WaitForSeconds(1);
        if (_currentTime > 0)
        {
            _currentTime--;
            _clock = StartCoroutine(ClockCoroutine());
        }
        else
        {
            PlayerLossFromTime();
        }
    }

    private void StopClock()
    {
        _isClockOn = false;
        if (_clock != null)
            StopCoroutine(_clock);
    }

    private void PlayerLossFromTime()
    {
        GameStateMachine.Instance.ChangeState(new PlayerWonState(GameStateMachine.Instance, _faction.GetOpposite()));
    }

}
