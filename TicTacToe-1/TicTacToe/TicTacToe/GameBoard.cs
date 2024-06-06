using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class GameBoard
    {
        private string[,] board = new string[3, 3];
        private string[,] cells = new string[3, 3];

        /// <summary>
        /// Attempts to make a move at the specified 
        /// coordinates on the game board with the given symbol
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public bool MakeMove(int x, int y, string symbol)
        {
            if (string.IsNullOrEmpty(board[x, y]))
            {
                board[x, y] = symbol;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Retrieves the content of the cell at the specified coordinates
        /// </summary>
        /// <param name="x">coordinate-x</param>
        /// <param name="y">coordinate-y</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string GetCell(int x, int y)
        {
            if (x < 0 || x >= 3 || y < 0 || y >= 3)
                throw new ArgumentOutOfRangeException("Cell coordinates are out of the board's bounds.");
            return board[x, y];
        }
        /// <summary>
        ///  Clears the content of the cell
        ///  at the specified coordinates,
        ///  effectively undoing the last move.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void UndoMove(int x, int y)
        {
            board[x, y] = null;
        }
        /// <summary>
        ///  Checks if there is a winning line (row, column, or diagonal) on the board
        /// </summary>
        /// <returns></returns>
        public bool CheckForVictory()
        {
            for (int i = 0; i < 3; i++)
            {
                if (CheckLine(0, i, 1, i, 2, i) || CheckLine(i, 0, i, 1, i, 2))
                    return true;
            }
            return CheckLine(0, 0, 1, 1, 2, 2) || CheckLine(0, 2, 1, 1, 2, 0);
        }
        /// <summary>
        /// Checks if the game is a draw
        /// </summary>
        /// <returns>true or false</returns>
        public bool IsDraw()
        {
            return GetAvailableMoves().Count == 0 && !CheckForVictory();
        }
        /// <summary>
        /// Checks if the game is over.
        /// </summary>
        /// <returns></returns>
        public bool IsGameOver()
        {
            return CheckForVictory() || IsDraw();

        }
        /// <summary>
        /// Determines the indices of
        /// the cells that form a winning line on the Tic-Tac-Toe board.
        /// </summary>
        /// <returns>an array containing the indices 
        /// of the winning cells, or an empty array if there is no winning line.</returns>
        public int[] GetWinningCells()
        {
            
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] != null && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                    return new int[] { i * 3, i * 3 + 1, i * 3 + 2 };
            }

            
            for (int j = 0; j < 3; j++)
            {
                if (board[0, j] != null && board[0, j] == board[1, j] && board[1, j] == board[2, j])
                    return new int[] { j, 3 + j, 6 + j };
            }

            
            if (board[0, 0] != null && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                return new int[] { 0, 4, 8 };

            if (board[0, 2] != null && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                return new int[] { 2, 4, 6 };

            return new int[0]; 
        }


        /// <summary>
        ///  Checks if the symbols in three specified cells on the board form a winning line.
        ///  (x1, y1), (x2, y2), (x3, y3): Coordinates of the cells to check.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        /// <returns>true or false</returns>
        private bool CheckLine(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            string a = board[x1, y1], b = board[x2, y2], c = board[x3, y3];
            return !string.IsNullOrEmpty(a) && a == b && b == c;
        }
        /// <summary>
        /// Retrieves a list of coordinates for all empty cells on the board.
        /// </summary>
        /// <returns>A list of tuples representing the coordinates of available moves.</returns>
        public List<(int, int)> GetAvailableMoves()
        {
            List<(int, int)> availableMoves = new List<(int, int)>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (string.IsNullOrEmpty(board[i, j]))
                        availableMoves.Add((i, j));
                }
            }
            return availableMoves;
        }
        /// <summary>
        /// Evaluates the current state of the board to determine the immediate win potential.
        /// </summary>
        /// <returns>The score representing the immediate win potential.</returns>
        public int EvaluateImmediateWin()
        {
            int score = 0;

          
            for (int i = 0; i < 3; i++)
            {
                score += EvaluateLine(new string[] { GetCell(i, 0), GetCell(i, 1), GetCell(i, 2) }); 
                score += EvaluateLine(new string[] { GetCell(0, i), GetCell(1, i), GetCell(2, i) }); 
            }

           
            score += EvaluateLine(new string[] { GetCell(0, 0), GetCell(1, 1), GetCell(2, 2) });
            score += EvaluateLine(new string[] { GetCell(0, 2), GetCell(1, 1), GetCell(2, 0) });

            return score;
        }
        /// <summary>
        /// Resets the board to its initial state.
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = null;
                }
            }
        }
        /// <summary>
        ///  Evaluates a line (row, column, or diagonal) on the Tic-Tac-Toe
        ///  board to determine its score based on the symbols present.
        /// </summary>
        /// <param name="cells"></param>
        /// <returns>The score of the line</returns>
        private int EvaluateLine(string[] cells)
        {
            int xCount = cells.Count(c => c == "X");
            int oCount = cells.Count(c => c == "O");

            if (xCount == 3) return 100; 
            if (oCount == 3) return -100; 
            if (xCount == 0 && oCount > 0) return oCount;
            if (oCount == 0 && xCount > 0) return xCount;

            return 0; 
        }
    }
    /// <summary>
    /// Description:a class which Represents a player in the Tic-Tac-Toe game
    /// </summary>
    public class Player
    {
        public string Symbol { get; private set; }
        public bool IsAI { get; private set; }

        public Player(string symbol, bool isAI)
        {
            Symbol = symbol;
            IsAI = isAI;
        }
    }
}
