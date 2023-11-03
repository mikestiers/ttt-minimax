using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class CellManager : MonoBehaviour
{
    public string[,] Board = new string[3, 3]
{
    { "", "", "" },
    { "", "", "" },
    { "", "", "" }
};
    public GameObject cellPrefab;
    public Transform gridTransform;
    public Sprite xSprite;
    public Sprite oSprite;
    private bool isPlayerXTurn = true;
    public MiniMaxAI miniMaxAI;

    void Start()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                GameObject cellObject = Instantiate(cellPrefab, gridTransform);
                Cell cell = cellObject.GetComponent<Cell>();
                cell.Setup(row, column, this, "X"); // Pass the player symbol as "X" here.
            }
        }
        // Create an instance of MinimaxAI and pass the reference to the Board array.
        miniMaxAI = new MiniMaxAI(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetBoard();
        }
    }

    public void ResetBoard()
    {
        Board = new string[3, 3]
        {
            { "", "", "" },
            { "", "", "" },
            { "", "", "" }
        };

        foreach (Transform child in gridTransform)
        {
            Cell cell = child.GetComponent<Cell>();
            if (cell != null)
            {
                cell.ResetCell();
            }
        }

        isPlayerXTurn = true;
    }

    public void UpdateBoard(int row, int column, string playerSymbol)
    {
        if (row >= 0 && row < 3 && column >= 0 && column < 3)
        {
            if (string.IsNullOrEmpty(Board[row, column]))
            {
                Board[row, column] = playerSymbol;
                Cell cell = GetCell(row, column);
                if (cell != null)
                {
                    cell.SetSprite(playerSymbol == "X" ? xSprite : oSprite);
                }
                isPlayerXTurn = !isPlayerXTurn;

                int gameState = CheckForWin(Board, playerSymbol); // Pass the current player's symbol
                if (gameState == -10)
                {
                    Debug.Log("Player O Wins!"); // Update the message for "O" win
                }
                else if (gameState == 10)
                {
                    Debug.Log("Player X Wins!"); // Update the message for "X" win
                }
                else if (IsBoardFull())
                {
                    Debug.Log("It's a draw!");
                }
                else
                {
                    if (playerSymbol == "X")
                    {
                        Debug.Log("Calling MiniMaxAI.MakeMove() for the AI's move.");
                        miniMaxAI.MakeMove();
                    }
                }
            }
            else
            {
                Debug.Log($"Cell is already taken! {row} {column}");
            }
        }
        else
        {
            Debug.Log($"Invalid row or column indices! {row} {column}");
        }
    }




    public int CheckForWin(string[,] board, string currentPlayer)
    {
        // Check rows and columns for a win
        for (int i = 0; i < 3; i++)
        {
            if ((board[i, 0] == currentPlayer && board[i, 1] == currentPlayer && board[i, 2] == currentPlayer) ||
                (board[0, i] == currentPlayer && board[1, i] == currentPlayer && board[2, i] == currentPlayer))
            {
                return (currentPlayer == "O") ? 10 : -10; // Return 10 for "O" win and -10 for "X" win
            }
        }

        // Check diagonals for a win
        if ((board[0, 0] == currentPlayer && board[1, 1] == currentPlayer && board[2, 2] == currentPlayer) ||
            (board[0, 2] == currentPlayer && board[1, 1] == currentPlayer && board[2, 0] == currentPlayer))
        {
            return (currentPlayer == "O") ? 10 : -10; // Return 10 for "O" win and -10 for "X" win
        }

        // No winner yet
        return 0;
    }

    private bool IsBoardFull()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                if (string.IsNullOrEmpty(Board[row, column]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private Cell GetCell(int row, int column)
    {
        foreach (Transform child in gridTransform)
        {
            Cell cell = child.GetComponent<Cell>();
            if (cell != null && cell.row == row && cell.column == column)
            {
                return cell;
            }
        }
        return null;
    }

}
