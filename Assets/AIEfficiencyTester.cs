using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using UnityEngine;
using System.Linq;

public class AIEfficiencyTester : MonoBehaviour
{
    [SerializeField] private Board _board;
    private void Start()
    {
        Board.Instance = _board;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var virtualBoard = new VirtualBoard(_board);
        var boards = CreateBoards(5000);
        var organizedBoards = OrganizeBoards(boards);
        stopwatch.Stop();
        UnityEngine.Debug.Log(stopwatch.ElapsedMilliseconds);
    }

    //93 for 5000 boards without units
    //153 for 5000 boards with rook
    //152 with list
    //237 with eval
    //250 with eval and organization
    private List<VirtualBoard> CreateBoards(int numberOfBoards)
    {
        List<VirtualBoard> boards = new List<VirtualBoard>();
        for (int i = 0; i < numberOfBoards; i++)
        {
            var virtualBoard = new VirtualBoard(_board);
            virtualBoard.CalculateEvaluation();
            boards.Add(virtualBoard);
        }
        return boards;
    }

    //60 for 5000 boards without units
    //101 for 5000 boards with rook
    //
    private void CreateBoardsFromVirtual(int numberOfBoards, VirtualBoard virtualBoard)
    {
        List<VirtualBoard> boards = new List<VirtualBoard>();
        for (int i = 0; i < numberOfBoards; i++)
        {
            var newVirtualBoard = new VirtualBoard(virtualBoard);
            boards.Add(newVirtualBoard);
        }
    }


    private List<VirtualBoard> OrganizeBoards(List<VirtualBoard> virtualBoards)
    {
        return virtualBoards.OrderBy(t => t.Evaluation).ToList();
    }
}
