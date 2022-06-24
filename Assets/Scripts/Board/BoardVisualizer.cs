using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoardVisualizer : MonoBehaviour
{
    public static BoardVisualizer Instance;
    private List<VirtualBoard> _virtualBoards = new List<VirtualBoard>();
    private int _turnNumber = 0;
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private Board _demoBoard;
    [SerializeField] private TextMeshProUGUI _turnNumberText;
    [SerializeField] private GameObject _tileTarget, _destroyedTile;
    private List<GameObject> _targetedTiles = new List<GameObject>(), _destroyedTiles = new List<GameObject>();

    public static Action OnBoardCreated;
    public static Action OnBoardHidden;

    public int TurnNumber { get => _turnNumber; set {
            _turnNumber = value;
            _turnNumberText.text = GetTurnNumberText(_turnNumber); ;
        } }

    public Board DemoBoard { get => _demoBoard; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Unit.OnUnitMoved += OnUnitMoved;
        Board.OnUnitPlaced += OnUnitPlaced;
        ShopManager.OnShopPhaseSkipped += OnShopPhaseSkipped;
        EnemyShopManager.EnemyShopPhaseSkipped += OnShopPhaseSkipped;
        GameStateMachine.OnStateChanged += HideBoardOnStateChange;
    }

    private void OnDisable()
    {
        Unit.OnUnitMoved -= OnUnitMoved;
        Board.OnUnitPlaced -= OnUnitPlaced;
        ShopManager.OnShopPhaseSkipped -= OnShopPhaseSkipped;
        EnemyShopManager.EnemyShopPhaseSkipped -= OnShopPhaseSkipped;
        GameStateMachine.OnStateChanged -= HideBoardOnStateChange;
    }

    private string GetTurnNumberText(int turnNumber)
    {
        string phaseString = ", Phase 1";
        if (turnNumber % 2 == 0)
        {
            phaseString = ", Phase 2";
        }
        return "Turn : " + ((turnNumber - 1)/ 2 + 1).ToString() + phaseString;
    }

    private void OnUnitMoved()
    {
        CreateBoard();
        TurnNumber = _virtualBoards.Count;
    }

    private void OnUnitPlaced(Unit unit)
    {
        if (unit.UnitData.IsKing)
            return;
        CreateBoard();
        TurnNumber = _virtualBoards.Count;
    }

    private void OnShopPhaseSkipped()
    {
        CreateBoard();
        TurnNumber = _virtualBoards.Count;
    }

    public void GetLastBoard()
    {
        Board.Instance.gameObject.SetActive(false);
        TurnNumber = _virtualBoards.Count;
        ShowBoardOnTurn();
    }

    public void IncreaseTurnNumber()
    {
        if (Board.Instance.gameObject.activeInHierarchy && TurnNumber == _virtualBoards.Count - 1)
        {
            TurnNumber++;
            HideBoard();
            return;
        }
        if (TurnNumber < _virtualBoards.Count)
        {
            TurnNumber++;
            ShowBoardOnTurn();
        }
    }

    public void DecreaseTurnNumber()
    {
        if (_demoBoard.gameObject.activeInHierarchy == false && TurnNumber > 1)
        {
            TurnNumber = _virtualBoards.Count;
        }
        if (TurnNumber > 1)
        {
            TurnNumber--;
            ShowBoardOnTurn();
        }
    }

    public void GoToLastMove()
    {
        TurnNumber = _virtualBoards.Count;
        if (Board.Instance.gameObject.activeInHierarchy)
        {
            HideBoard();
        }
        else ShowBoardOnTurn();
    }

    private void ShowBoardOnTurn()
    {
        if (_demoBoard.gameObject.activeInHierarchy == false)
            OnBoardCreated?.Invoke();
        _demoBoard.gameObject.SetActive(true);
        VisualizeBoard(_virtualBoards[TurnNumber - 1]);
        ShowDestroyedAndTargetedTiles();
    }

    private void ShowDestroyedAndTargetedTiles()
    {
        foreach (var target in _targetedTiles)
        {
            Destroy(target);
        }
        _targetedTiles.Clear();
        foreach (var destroyedTile in _destroyedTiles)
            Destroy(destroyedTile);
        _destroyedTiles.Clear();
        foreach (var tile in _virtualBoards[TurnNumber - 1].TileArray)
        {
            if (tile.IsBlocked)
            {
                var destroyedTile = Instantiate(_destroyedTile, Board.Instance.GetTileAtPosition(tile.TilePosition()).transform.position, Quaternion.identity);
                _destroyedTiles.Add(destroyedTile);
            }
            else if (tile.IsTargeted)
            {
                var targetedTile = Instantiate(_tileTarget, Board.Instance.GetTileAtPosition(tile.TilePosition()).transform.position, Quaternion.identity);
                _targetedTiles.Add(targetedTile);
            }
        }
    }

    private void HideBoard()
    {
        _demoBoard.ClearBoard();
        _demoBoard.gameObject.SetActive(false);
        Board.Instance.gameObject.SetActive(true);
        OnBoardHidden?.Invoke();
    }

    private void HideBoardOnStateChange(GameState state)
    {
        _demoBoard.ClearBoard();
        _demoBoard.gameObject.SetActive(false);
        Board.Instance.gameObject.SetActive(true);
    }

    private void CreateBoard()
    {
        var board = new VirtualBoard(Board.Instance);
        _virtualBoards.Add(board);
    }

    private void VisualizeBoard(VirtualBoard board)
    {
        _demoBoard.ClearBoard();
        foreach (var unit in board.VirtualUnits)
        {
            var position = unit.Tile.TilePosition();
            var boardTile = _demoBoard.GetTileAtPosition(position);
            var unitModel = Instantiate(_unitPrefab, boardTile.transform.position, Quaternion.identity);
            SetModel(unit, unitModel);
            _demoBoard.PlayerUnits.Add(unitModel);
        }
    }

    private void SetModel(VirtualUnit unit, Unit unitModel)
    {
        unitModel.SetSprite(unit.UnitData.GetSprite(unit.Faction));
        unitModel.SetColor(unit.SummoningSickness);
    }
}