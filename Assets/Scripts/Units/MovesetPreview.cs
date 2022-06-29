using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MovesetPreview : MonoBehaviour
{
    [SerializeField] private Image _moveset;
    [SerializeField] private TextMeshProUGUI _unitName;
    private Vector3 _scale;

    private void Awake()
    {
        _scale = transform.localScale;
    }
    private void OnEnable()
    {
        BoardTile.OnMouseOver += ShowPreview;
        BackgroundHover.OnBackgroundHover += HidePreview;
    }

    private void OnDisable()
    {
        BoardTile.OnMouseOver -= ShowPreview;
        BackgroundHover.OnBackgroundHover -= HidePreview;
    }

    private void HidePreview()
    {
        _moveset.enabled = false;
        _unitName.text = "";
    }

    private void ShowPreview(BoardTile boardTile)
    {
        if (boardTile.UnitOnTile == null)
        {
            HidePreview();
            return;
        }
        _moveset.enabled = true;
        _moveset.sprite = boardTile.UnitOnTile.UnitData.Moveset;
        if (boardTile.UnitOnTile.Faction == UnitFaction.Enemy)
        {
            _moveset.transform.localScale = new Vector3(_scale.x, _scale.y * -1, _scale.z);
        }
        else _moveset.transform.localScale = _scale;
        _unitName.text = boardTile.UnitOnTile.UnitData.UnitName;
    }
}
