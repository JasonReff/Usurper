using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    [SerializeField] private MultiplayerClock _clock;
    [SerializeField] private TextMeshProUGUI _textbox;

    private void Update()
    {
        int minutes = _clock.CurrentTime / 60;
        int seconds = _clock.CurrentTime % 60;
        _textbox.text = $"{TimeToString(minutes)} : {TimeToString(seconds)}";
    }

    private string TimeToString(int time)
    {
        string timeString = time.ToString();
        if (time < 10)
        {
            timeString.Insert(0, "0");
        }
        return timeString;
    }
}