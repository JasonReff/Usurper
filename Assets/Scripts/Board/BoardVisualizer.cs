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

    public static Action OnBoardCreated;
    public static Action OnBoardHidden;

    public int TurnNumber { get => _turnNumber; set {
            _turnNumber = value;
            _turnNumberText.text = $"Turn : {_turnNumber}";
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
    }

    private void OnDisable()
    {
        Unit.OnUnitMoved -= OnUnitMoved;
        Board.OnUnitPlaced -= OnUnitPlaced;
    }

    private void OnUnitMoved()
    {
        CreateBoard();
        TurnNumber = _virtualBoards.Count - 1;
    }

    private void OnUnitPlaced()
    {
        CreateBoard();
        TurnNumber = _virtualBoards.Count - 1;
    }

    public void GetLastBoard()
    {
        Board.Instance.gameObject.SetActive(false);
        TurnNumber = _virtualBoards.Count - 1;
        ShowBoardOnTurn();
    }

    public void IncreaseTurnNumber()
    {
        if (Board.Instance.gameObject.activeInHierarchy && TurnNumber == _virtualBoards.Count - 2)
        {
            TurnNumber++;
            HideBoard();
            return;
        }
        if (TurnNumber < _virtualBoards.Count - 1)
        {
            TurnNumber++;
            ShowBoardOnTurn();
        }
    }

    public void DecreaseTurnNumber()
    {
        if (_demoBoard.gameObject.activeInHierarchy == false)
        {
            TurnNumber = _virtualBoards.Count - 1;
        }
        if (TurnNumber > 1)
        {
            TurnNumber--;
            ShowBoardOnTurn();
        }
    }

    public void GoToLastMove()
    {
        TurnNumber = _virtualBoards.Count - 1;
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
        VisualizeBoard(_virtualBoards[TurnNumber]);
        
    }

    private void HideBoard()
    {
        _demoBoard.ClearBoard();
        _demoBoard.gameObject.SetActive(false);
        Board.Instance.gameObject.SetActive(true);
        OnBoardHidden?.Invoke();
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