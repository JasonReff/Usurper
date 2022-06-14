using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IUnit
{
    [SerializeField] protected BoardTile _tile;
    [SerializeField] private UnitFaction _faction;
    [SerializeField] private SpriteRenderer _renderer;
    private float _deathTime = 0.3f;
    [SerializeField] protected bool _summoningSickness = true;
    [SerializeField] private UnitData _unitData;
    [SerializeField] private UnitColors _unitColors;

    public BoardTile Tile { get => _tile; set => _tile = value; }
    public UnitFaction Faction { get => _faction; set => _faction = value; }
    public UnitData UnitData { get => _unitData; }
    public bool SummoningSickness { get => _summoningSickness; set => _summoningSickness = value; }

    public static Action OnUnitMoved;
    public static Action<Unit> OnUnitDeath;

    private void OnEnable()
    {
        MoveUnitState.OnMoveStateEnded += RemoveSummoningSickness;
        CharacterManager.OnUnitSelected += SetHighlight;
        BoardVisualizer.OnBoardCreated += ClearHighlight;
    }

    private void OnDisable()
    {
        MoveUnitState.OnMoveStateEnded -= RemoveSummoningSickness;
        CharacterManager.OnUnitSelected -= SetHighlight;
        BoardVisualizer.OnBoardCreated -= ClearHighlight;
    }

    private void Awake()
    {
        if (_summoningSickness)
        {
            _renderer.color = _unitColors.SummoningSicknessColor;
        }
    }

    public virtual void MoveToTile<T>(T tile) where T : IBoardTile
    {
        var boardTile = tile as BoardTile;
        if (_summoningSickness)
            return;
        if (!CanMoveToTile(boardTile))
            return;
        var move = new Move(_unitData, _tile, boardTile);
        move.ExecuteMove(this);
        transform.DOMove((move.NewTile as BoardTile).transform.position, 1);
        OnUnitMoved?.Invoke();
        if (TryGetComponent<OnlineUnitCallbacks>(out var onlineUnit))
        {
            onlineUnit.OnUnitMoved(move.CurrentTile.TilePosition(), move.NewTile.TilePosition());
        }
    }

    public virtual void ForceMoveToTile(IBoardTile tile)
    {
        var move = new Move(_unitData, _tile, tile);
        move.ExecuteMove(this);
        transform.DOMove((move.NewTile as BoardTile).transform.position, 1);
        OnUnitMoved?.Invoke();
    }

    private void RemoveSummoningSickness()
    {
        _summoningSickness = false;
        _renderer.color = _unitColors.NormalColor;
    }

    public void SetSprite(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }

    public void CaptureUnit(Unit unit)
    {
        StartCoroutine(unit.UnitDeath());
    }

    public virtual bool CanMoveToTile(BoardTile tile)
    {
        if (_summoningSickness)
            return false;
        if (!_unitData.CanMoveToTile(this, _tile, tile, Board.Instance))
            return false;
        return true;
    }

    protected virtual IEnumerator UnitDeath()
    {
        OnUnitDeath?.Invoke(this);
        _renderer.DOFade(0, _deathTime);
        yield return new WaitForSeconds(_deathTime);
        Destroy(gameObject);
    }

    public void SetColor(bool summoningSickness)
    {
        if (summoningSickness)
            _renderer.color = _unitColors.SummoningSicknessColor;
        else _renderer.color = _unitColors.NormalColor;
    }

    private void SetHighlight(Unit unit)
    {
        if (unit == this && _summoningSickness == false)
        {
            _renderer.color = _unitColors.HighlightedColor;
        }
        else SetColor(_summoningSickness);
    }

    private void ClearHighlight()
    {
        SetColor(_summoningSickness);
    }
}

public abstract class RangedUnit : Unit
{
    public override void MoveToTile<T>(T tile)
    {
        if (IsRangedAttack(tile))
        {
            var move = new Move(UnitData, _tile, tile);
            move.RangedAttack(this);
            OnUnitMoved?.Invoke();
            if (TryGetComponent<OnlineUnitCallbacks>(out var onlineUnit))
            {
                onlineUnit.OnUnitMoved(move.CurrentTile.TilePosition(), move.NewTile.TilePosition());
            }
            return;
        }
        base.MoveToTile(tile);
    }

    public bool IsRangedAttack(IBoardTile tile)
    {
        if ((UnitData as RangedUnitData).IsRangedAttack(this, _tile, tile, Board.Instance))
            return true;
        return false;
    }

    public override bool CanMoveToTile(BoardTile tile)
    {
        if (_summoningSickness)
            return false;
        if ((UnitData as RangedUnitData).IsRangedAttack(this, _tile, tile, Board.Instance))
            return true;
        if (tile.UnitOnTile != null)
            return false;
        return base.CanMoveToTile(tile);
    }

    public override void ForceMoveToTile(IBoardTile tile)
    {
        if ((UnitData as RangedUnitData).IsRangedAttack(this, _tile, tile, Board.Instance))
        {
            var move = new Move(UnitData, _tile, tile);
            move.RangedAttack(this);
            OnUnitMoved?.Invoke();
            return;
        }
        base.ForceMoveToTile(tile);
    }
}

[Serializable]
public enum UnitFaction
{
    Player = 0,
    Enemy = 1,
}

public interface IUnit
{
    public UnitFaction Faction { get; }
    public UnitData UnitData { get; }
    public bool SummoningSickness { get; }

    public void MoveToTile<T>(T tile) where T : IBoardTile;
}
