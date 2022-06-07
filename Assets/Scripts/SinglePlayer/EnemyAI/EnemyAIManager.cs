using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAIManager : CharacterManager
{
    private Move _chosenMove;
    [SerializeField] private EnemyShopManager _shop;
    [SerializeField] private KingUnit _kingUnit;
    [SerializeField] private int _maximumPawns = 4;
    [SerializeField] private CardPool _pool;
    public override void AddCharacterEvents(GameState state)
    {
        if (IsGameStateCorrectFaction(state) && state.GetType() == typeof(MoveUnitState))
        {
            SelectBoardState();
            MovePiece();
        }
        if (IsGameStateCorrectFaction(state) && state.GetType() == typeof(BuyUnitState))
        {
            SetupShop();
            StartCoroutine(PlaceUnitCoroutine());
        }
    }

    private IEnumerator PlaceUnitCoroutine()
    {
        yield return new WaitForSeconds(1);
        GetBestUnitPlacement();
        PlaceUnit();
        yield return new WaitForSeconds(1);
        GameStateMachine.Instance.ChangeState(new MoveUnitState(GameStateMachine.Instance, _faction));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        BoardTile.OnUnitPlaced += SetKing;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        BoardTile.OnUnitPlaced -= SetKing;
    }

    public override void RemoveCharacterEvents(GameState state)
    {

    }

    private void SetKing(Unit unit)
    {
        if (unit.GetType() == typeof(KingUnit) && unit.Faction == _faction)
            _kingUnit = (KingUnit)unit;
    }

    private void SelectBoardState()
    {
        var random = new System.Random();
        var currentBoardState = new VirtualBoard(Board.Instance);
        List<VirtualBoard> virtualBoards = currentBoardState.GetAllPossibleBoards(_faction, currentBoardState);
        foreach (var board in virtualBoards)
            board.CalculateEvaluation();
        virtualBoards = virtualBoards.OrderBy(t => t.KingInCheck).ThenByDescending(t => t.Evaluation).ThenBy(t => random.Next()).ToList();
        var checkmateBoard = PossibleCheckmate(virtualBoards);
        if (checkmateBoard == null)
            GetFirstPossibleBoard(virtualBoards);
        else _chosenMove = checkmateBoard.Move;
    }

    private void GetFirstPossibleBoard(List<VirtualBoard> virtualBoards)
    {
        var random = new System.Random();
        virtualBoards = virtualBoards.OrderByDescending(t => t.Evaluation).ThenBy(t => random.Next()).ToList();
        var chosenBoard = virtualBoards.First();
        var unit = Board.Instance.GetTileAtPosition(chosenBoard.Move.CurrentTile.TilePosition()).UnitOnTile;
        while (!unit.UnitData.CanMoveToTile(unit, Board.Instance.GetTileAtPosition(chosenBoard.Move.CurrentTile.TilePosition()), 
            Board.Instance.GetTileAtPosition(chosenBoard.Move.NewTile.TilePosition()), Board.Instance))
        {
            virtualBoards.Remove(chosenBoard);
            chosenBoard = virtualBoards.First();
            unit = Board.Instance.GetTileAtPosition(chosenBoard.Move.CurrentTile.TilePosition()).UnitOnTile;
        }
        _chosenMove = chosenBoard.Move;
    }

    private VirtualBoard? PossibleCheckmate(List<VirtualBoard> boards)
    {
        var checkmatingBoards = boards.Where(t => !t.VirtualUnits.Any(u => u.UnitData.GetType() == typeof(KingUnitData) && u.Faction == UnitFaction.Player)).ToList();
        if (checkmatingBoards.Count == 0)
            return null;
        var board = checkmatingBoards.First();
        var unit = Board.Instance.GetTileAtPosition(board.Move.CurrentTile.TilePosition()).UnitOnTile;
        if (!unit.UnitData.CanMoveToTile(unit, Board.Instance.GetTileAtPosition(board.Move.CurrentTile.TilePosition()),
            Board.Instance.GetTileAtPosition(board.Move.NewTile.TilePosition()), Board.Instance))
            return null;
        return board;
    }

    private void MovePiece()
    {
        var unit = Board.Instance.GetTileAtPosition(_chosenMove.CurrentTile.TilePosition()).UnitOnTile;
        unit.MoveToTile(Board.Instance.GetTileAtPosition(_chosenMove.NewTile.TilePosition()));
        Debug.Log($"Unit : {unit.UnitData.UnitName} to {_chosenMove.NewTile.TilePosition()}");
        Debug.Log($"Unit present at position: {_chosenMove.NewTile.UnitOnTile}");
        //OnUnitMoved?.Invoke();
    }

    private void SetupShop()
    {
        _shop.SetupShop();
    }

    private void GetBestUnitPlacement()
    {
        var random = new System.Random();
        var currentBoardState = new VirtualBoard(Board.Instance);
        var cardsInHand = _shop.GetPurchaseableCardsInHand();
        var numberOfPawns = Board.Instance.EnemyUnits.Where(t => _pool.PawnPool.Contains(t.UnitData)).Count();
        if (numberOfPawns >= _maximumPawns)
            cardsInHand.RemoveAll(t => t.Unit.GetType() == typeof(PawnUnit) || t.Unit.GetType() == typeof(ShogiPawnUnit));
        var boards = new List<VirtualBoard>();
        if (cardsInHand.Count < 1)
        {
            _chosenMove = null;
            _shop.SkipShopPhase();
            return;
        }
        foreach (var card in cardsInHand)
        {
            List<VirtualBoard> virtualBoards = currentBoardState.GetPossibleBoardsAfterUnitPlacement(card.Unit, _faction, currentBoardState);
            boards.AddRange(virtualBoards);
        }
        var firstPossibleBoard = GetFirstPossiblePlacementBoard(boards);
        if (firstPossibleBoard == null)
            _chosenMove = null;
        else 
            _chosenMove = firstPossibleBoard.Move;
    }

    private VirtualBoard GetFirstPossiblePlacementBoard(List<VirtualBoard> boards)
    {
        var random = new System.Random();
        var virtualBoards = boards.OrderByDescending(t => t.Evaluation).ThenBy(t => random.Next()).ToList();
        var chosenBoard = virtualBoards.First();
        while (Board.Instance.GetTileAtPosition(chosenBoard.Move.NewTile.TilePosition()).UnitOnTile != null)
        {
            virtualBoards.Remove(chosenBoard);
            chosenBoard = virtualBoards.First();
        }
        return chosenBoard;
    }

    private void PlaceUnit()
    {
        if (_chosenMove as UnitPlacement == null)
            return;
        else
        {
            _shop.SelectedUnit = (_chosenMove as UnitPlacement).Unit.UnitData;
        }
        var tile = Board.Instance.GetTileAtPosition(_chosenMove.NewTile.TilePosition());
        _shop.PurchaseAndPlaceUnit(tile);
    }
}