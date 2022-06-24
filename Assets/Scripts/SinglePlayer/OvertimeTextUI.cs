using TMPro;
using UnityEngine;

public class OvertimeTextUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textbox;
    private void OnEnable()
    {
        BoardTile.OnMouseOver += ShowOvertimeText;
    }

    private void OnDisable()
    {
        BoardTile.OnMouseOver -= ShowOvertimeText;
    }

    private void ShowOvertimeText(BoardTile tile)
    {
        if (tile.IsTargeted)
        {
            _textbox.enabled = true;
        }
        else _textbox.enabled = false;
    }
}