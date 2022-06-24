using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAIManager : CharacterManager
{
    private Move _chosenMove;
    [SerializeField] private EnemyShopManager _shop;
    [SerializeField] protected KingUnit _kingUnit;
    [SerializeField] private int _maximumPawns = 4, _maximumMoney = 4, _worstBoard = 2;
    [SerializeField] private float _blunderChance = 0.1f;
    [SerializeField] private CardPool _pool;
    [SerializeField] private SinglePlayerStats _stats;

    public EnemyShopManager Shop { get => _shop; set => _shop = value; }

    public override void AddCharacterEvents(GameState state)
    {
        if (state.GetType() == typeof(StartGameState))
            return;
        if (!Board.Instance.EnemyUnits.Any(t => t.UnitData.IsKing))
        {
            GameStateMachine.Instance.ChangeState(new PlayerWonState(GameStateMachine.Instance, UnitFaction.Player));
            return;
        }
        else if (!Board.Instance.PlayerUnits.Any(t => t.UnitData.IsKing))
        {
            GameStateMachine.Instance.ChangeState(new PlayerWonState(GameStateMachine.Instance, UnitFaction.Enemy));
            return;
        }
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

    protected virtual IEnumerator PlaceUnitCoroutine()
    {
        if (!Board.Instance.EnemyUnits.Any(t => t.UnitData.IsKing))
            yield break;
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

    protected virtual void Awake()
    {
        _blunderChance = 0.1f - (0.01f * _stats.Round);
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
        virtualBoards = ReorderMovementBoards(random, virtualBoards);
        var checkmateBoard = PossibleCheckmate(virtualBoards);
        if (checkmateBoard == null)
            GetFirstPossibleBoard(virtualBoards);
        else _chosenMove = checkmateBoard.Move;
    }

    private List<VirtualBoard> ReorderMovementBoards(System.Random random, List<VirtualBoard> boards)
    {
        var reorderedBoards = boards.OrderBy(t => t.KingInCheck).
            ThenBy(t => t.IsKingNextToHangingBomb()).
            ThenByDescending(t => t.IsCaptureFree()).
            ThenByDescending(t => t.CaptureValue()).
            ThenBy(t => t.IsPieceHanging()).
            ThenByDescending(t => t.Evaluation).
            ThenBy(t => random.Next()).ToList();
        return reorderedBoards;
    }

    private List<VirtualBoard> ReorderPlacementBoards(System.Random random, List<VirtualBoard> boards)
    {
        var reorderedBoards = boards.OrderBy(t => t.IsKingNextToHangingBomb()).
            ThenByDescending(t => t.UnitsAttackedByPlacedPiece()).
            ThenBy(t => t.IsPieceHanging()).
            ThenByDescending(t => t.Evaluation).ToList();
        return reorderedBoards;
    }

    private void GetFirstPossibleBoard(List<VirtualBoard> virtualBoards)
    {
        var random = new System.Random();
        virtualBoards = RemoveImpossibleBoards(virtualBoards);
        virtualBoards = ReorderMovementBoards(random, virtualBoards);
        var chosenBoard = GetBoardAtIndex(virtualBoards);
        _chosenMove = chosenBoard.Move;
    }

    private List<VirtualBoard> RemoveImpossibleBoards(List<VirtualBoard> virtualBoards)
    {
        var boards = virtualBoards.Where(t => (Board.Instance.GetTileAtPosition(t.Move.CurrentTile.TilePosition()).UnitOnTile as Unit).
            CanMoveToTile(Board.Instance.GetTileAtPosition(t.Move.NewTile.TilePosition()))).ToList();
        return boards;
    }

    private VirtualBoard GetBoardAtIndex(List<VirtualBoard> boards)
    {
        var randomBlunderChance = UnityEngine.Random.Range(0, 1f);
        if (randomBlunderChance > _blunderChance || boards.Count == 1)
            return boards[0];
        var index = UnityEngine.Random.Range(0, boards.Count);
        if (index > _worstBoard)
            index = _worstBoard;
        Debug.Log($"Blunder: {index}");
        return boards[index];
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
    }

    protected void SetupShop()
    {
        _shop.SetupShop();
    }

    protected void GetBestUnitPlacement()
    {
        var random = new System.Random();
        var currentBoardState = new VirtualBoard(Board.Instance);
        var cardsInHand = _shop.GetPurchaseableCardsInHand();
        var numberOfPawns = Board.Instance.EnemyUnits.Where(t => _pool.PawnPool.Contains(t.UnitData)).Count();
        if (numberOfPawns >= _maximumPawns && _maximumPawns != 0)
            cardsInHand.RemoveAll(t => _pool.PawnPool.Contains(t));
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
            if (virtualBoards == null)
                return;
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
        var virtualBoards = ReorderPlacementBoards(random, boards);
        if (virtualBoards.Count == 0)
            return null;
        var chosenBoard = virtualBoards.First();
        while (Board.Instance.GetTileAtPosition(chosenBoard.Move.NewTile.TilePosition()).UnitOnTile != null)
        {
            virtualBoards.Remove(chosenBoard);
            if (virtualBoards.Count == 0)
                return null;
            chosenBoard = virtualBoards.First();
        }
        return chosenBoard;
    }

    protected void PlaceUnit()
    {
        if (SaveMoney())
        {
            _chosenMove = null;
            _shop.SkipShopPhase();
            return;
        }
        if (_chosenMove as UnitPlacement == null)
            return;
        else
        {
            _shop.SelectedUnit = (_chosenMove as UnitPlacement).Unit.UnitData;
        }
        var tile = Board.Instance.GetTileAtPosition(_chosenMove.NewTile.TilePosition());
        _shop.PurchaseAndPlaceUnit(tile);

    }

    private bool SaveMoney()
    {
        var save = UnityEngine.Random.Range(0, 4);
        if (_shop.Money < save)
            return true;
        else return false;
    }

    private void DebugMessages(VirtualBoard board)
    {
        Debug.Log($"Is King in Check? {board.KingInCheck}");
        Debug.Log($"Is Capture Free? {board.IsCaptureFree()}");
        Debug.Log($"Capture value: {board.CaptureValue()}");
        Debug.Log($"Is Piece Hanging? {board.IsPieceHanging()}");
    }

    public override void RemoveCharacterEvents(GameState state)
    {
        
    }
}
