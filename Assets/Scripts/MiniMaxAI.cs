using UnityEngine;

public class MiniMaxAI
{
    public string playerSymbol = "O";

    private CellManager cellManager;
    private string[,] board; // Store the reference to the Board array here.

    public MiniMaxAI(CellManager cellManager)
    {
        this.cellManager = cellManager;
        this.board = cellManager.Board; // Store the reference to the Board array.
    }

    public void MakeMove()
    {
        var (bestScore, bestRow, bestCol) = MiniMax(board, 0, true);

        if (bestRow >= 0 && bestRow < 3 && bestCol >= 0 && bestCol < 3)
        {
            Debug.Log($"Best move - Row: {bestRow}, Col: {bestCol}, Score: {bestScore}");
            cellManager.UpdateBoard(bestRow, bestCol, playerSymbol);
        }
        else
        {
            Debug.LogError("Invalid move coordinates");
        }
    }


    // Recursively check every possible next move and moves after that until the game ends
    private (int score, int row, int col) MiniMax(string[,] boardStatus, int depth, bool isMaximizing)
    {
        int xResult = cellManager.CheckForWin(boardStatus, "X");
        int oResult = cellManager.CheckForWin(boardStatus, "O");

        if (oResult != 0)
        {
            // "O" wins; return a positive score to prioritize this move.
            return (10, -1, -1);
        }
        else if (xResult != 0)
        {
            // "X" wins; return a negative score to avoid this move.
            return (-10, -1, -1);
        }

        if (depth == 9) // Adjust this to your desired maximum depth
        {
            // Reached maximum depth; return 0 or evaluate the board state.
            return (0, -1, -1);
        }

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
        int bestRow = -1;
        int bestCol = -1;

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (boardStatus[x, y] == "")
                {
                    boardStatus[x, y] = isMaximizing ? "O" : "X";
                    var (score, _, _) = MiniMax(boardStatus, depth + 1, !isMaximizing);
                    boardStatus[x, y] = "";

                    if ((isMaximizing && score > bestScore) || (!isMaximizing && score < bestScore))
                    {
                        bestScore = score;
                        bestRow = x;
                        bestCol = y;
                    }
                }
            }
        }

        return (bestScore, bestRow, bestCol);
    }
}
