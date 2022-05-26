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
        BoardTile.OnMouseExit += HidePreview;
    }

    private void OnDisable()
    {
        BoardTile.OnMouseOver -= ShowPreview;
        BoardTile.OnMouseExit -= HidePreview;
    }

    private void HidePreview()
    {
        _moveset.enabled = false;
        _unitName.text = "";
    }

    private void ShowPreview(Unit unit)
    {
        _moveset.enabled = true;
        _moveset.sprite = unit.UnitData.Moveset;
        if (unit.Faction == UnitFaction.Enemy)
        {
            _moveset.transform.localScale = new Vector3(_scale.x, _scale.y * -1, _scale.z);
        }
        else _moveset.transform.localScale = _scale;
        _unitName.text = unit.UnitData.UnitName;
    }
}