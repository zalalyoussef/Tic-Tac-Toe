using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public class GameManager
    {
        private GameBoard board = new GameBoard();
        private Player currentPlayer;
        private Player player1;
        private Player player2;
        private Form1 form;
        private int maxDepth = 3;
        private bool isPlayerVsPlayer = false; 
        private string difficulty;
        /// <summary>
        /// Initializes a new instance of the
        /// GameManager class with the specified difficulty and form.
        /// </summary>
        /// <param name="difficulty"> A string indicating the difficulty level ("Easy", "Hard", or "Medium").</param>
        /// <param name="form">An instance of Form1 used to interact with the UI.</param>
        public GameManager(string difficulty, Form1 form)
        {
            
            this.form = form;
            this.difficulty = difficulty;

            
            if (difficulty == "Easy")
            {
                player1 = new Player("O", false);
                player2 = new Player("X", true);
            }
            else if (difficulty == "Hard")
            {
                player1 = new Player("O", true);
                player2 = new Player("X", true);
            }
            else 
            {
                player1 = new Player("O", false);
                player2 = new Player("X", false);
                isPlayerVsPlayer = true;
            }
            currentPlayer = player1;
            StartGame();
        }
        /// <summary>
        ///  Starts the game by resetting the board, resetting buttons on the UI,
        ///  and setting the status to indicate 
        ///  the current player's turn.
        /// </summary>
        private void StartGame()
        {
            board.Reset();
            form.ResetButtons();
            form.SetStatus($"{currentPlayer.Symbol}'s turn");
            if (currentPlayer.IsAI)
            {
                AIMove();
            }
        }
        /// <summary>
        /// Switches the current player to
        /// the next player and updates the UI status accordingly.
        /// </summary>
        private void SwitchPlayer()
        {
            currentPlayer = currentPlayer == player1 ? player2 : player1;
            form.SetStatus($"{currentPlayer.Symbol}'s turn");
            if (currentPlayer.IsAI)
            {
                AIMove(); 
            }
        }
        /// <summary>
        /// Implements the Minimax algorithm to find the best move for the current player.
        /// </summary>
        /// <param name="board">An instance of GameBoard representing the current state of the game.</param>
        /// <param name="depth">The current depth in the game tree. Controls the depth of the recursive search.</param>
        /// <param name="maximizingPlayer"></param>
        /// <returns> A boolean indicating whether the current player
        /// is maximizing (true) or minimizing (false).</returns>
        private Tuple<int, int, int> Minimax(GameBoard board, int depth, bool maximizingPlayer)
        {
            if (depth == 0 || board.IsGameOver())
                return Tuple.Create(EvaluateBoard(board), -1, -1);

            int bestScore = maximizingPlayer ? int.MinValue : int.MaxValue;
            int bestRow = -1;
            int bestCol = -1;

            foreach (var (row, col) in board.GetAvailableMoves())
            {
                board.MakeMove(row, col, maximizingPlayer ? player2.Symbol : player1.Symbol);
                var score = Minimax(board, depth - 1, !maximizingPlayer).Item1;
                board.UndoMove(row, col);

                if (maximizingPlayer ? score > bestScore : score < bestScore)
                {
                    bestScore = score;
                    bestRow = row;
                    bestCol = col;
                }
            }
            return Tuple.Create(bestScore, bestRow, bestCol);
        }
        /// <summary>
        /// Asynchronous method that allows the
        /// AI player to make a move based on the Minimax algorithm.
        /// </summary>
        private async void AIMove()
        {
            try
            {
                var bestMove = await Task.Run(() => Minimax(board, maxDepth, true));
                form.Invoke(new Action(() =>
                {
                    if (board.MakeMove(bestMove.Item2, bestMove.Item3, currentPlayer.Symbol))
                    {
                        form.UpdateButton(bestMove.Item2, bestMove.Item3, currentPlayer.Symbol);
                        if (!CheckEndGame() && !board.IsGameOver())
                        {
                            SwitchPlayer(); // Switch to the next player after the move
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                form.Invoke(new Action(() => form.SetStatus($"Error: {ex.Message}")));
            }
        }
        /// <summary>
        /// responsible for determining if the game has ended due to either a victory or a draw
        /// </summary>
        /// <returns></returns>
        private bool CheckEndGame()
        {
            bool gameEnded = false;
            if (board.CheckForVictory())
            {
                form.ShowWinningMessage(currentPlayer.Symbol, board.GetWinningCells());
                gameEnded = true;
            }
            if (board.IsDraw())
            {
                form.SetStatus("It's a draw!");
                gameEnded = true;
            }

            if (gameEnded)
            {
                // Perform end game actions if the game has ended
                var scores = CalculateMoveScores();
                form.DisplayEvaluationScores(scores);
                form.DisableAllButtons();
            }

            return gameEnded;
        }


        //public int EvaluateImmediateWin()
        //{
        //    int score = 0;

            
        //    for (int i = 0; i < 3; i++)
        //    {
        //        score += EvaluateLine(new string[] { board.GetCell(i, 0), board.GetCell(i, 1), board.GetCell(i, 2) });
        //        score += EvaluateLine(new string[] { board.GetCell(0, i), board.GetCell(1, i), board.GetCell(2, i) }); 
        //    }

            
        //    score += EvaluateLine(new string[] { board.GetCell(0, 0), board.GetCell(1, 1), board.GetCell(2, 2) });
        //    score += EvaluateLine(new string[] { board.GetCell(0, 2), board.GetCell(1, 1), board.GetCell(2, 0) });

        //    return score;
        //}

        private int EvaluateBoard(GameBoard board)
        {
            
            return board.EvaluateImmediateWin();
        }


        /// <summary>
        /// Manages the process when a player makes a move on the game board
        /// </summary>
        /// <param name="x">coordinate</param>
        /// <param name="y">coordinate</param>
        public void PlayerMove(int x, int y)
        {
            try
            {
                if (board.MakeMove(x, y, currentPlayer.Symbol))
                {
                    form.UpdateButton(x, y, currentPlayer.Symbol);

                    if (board.CheckForVictory())
                    {
                        int[] winningCells = board.GetWinningCells();
                        form.ShowWinningMessage(currentPlayer.Symbol, winningCells);
                        
                    }
                    else if (board.IsDraw())
                    {
                        MessageBox.Show("It's a draw!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        form.DisableAllButtons();
                    }
                    else
                    {
                        SwitchPlayer();
                    }
                }
            }
            catch (Exception ex)
            {
                form.SetStatus($"Error: {ex.Message}");
            }
        }


        /// <summary>
        /// Generates and returns a list of random scores for each available move on the game board.
        /// </summary>
        /// <returns> the list of generated scores.</returns>
        public List<int> CalculateMoveScores()
        {
            List<int> moveScores = new List<int>();
            Random random = new Random();

           
            var moves = board.GetAvailableMoves();
            foreach (var move in moves)
            {
                
                int score = random.Next(-100, 101); 
                moveScores.Add(score);
            }

            return moveScores;
        }



        //private List<Tuple<int, int>> CalculateIdealMoveSequences()
        //{
        //    List<Tuple<int, int>> idealMoves = new List<Tuple<int, int>>();

        //    Random random = new Random();
        //    for (int i = 0; i < 3; i++)
        //    {
        //        int row = random.Next(0, 3);
        //        int col = random.Next(0, 3);
        //        idealMoves.Add(Tuple.Create(row, col));
        //    }

        //    return idealMoves;
        //}


        //private int EvaluateLine(string[] cells)
        //{
        //    int xCount = cells.Count(c => c == "X");
        //    int oCount = cells.Count(c => c == "O");

        //    if (xCount == 3) return 100; 
        //    if (oCount == 3) return -100; 
        //    if (xCount == 0 && oCount > 0) return oCount;
        //    if (oCount == 0 && xCount > 0) return xCount;

        //    return 0;
        //}



        //private int EvaluateDiagonals(GameBoard board)
        //{
        //    return EvaluateLine(new string[] { board.GetCell(0, 0), board.GetCell(1, 1), board.GetCell(2, 2) }) +
        //           EvaluateLine(new string[] { board.GetCell(0, 2), board.GetCell(1, 1), board.GetCell(2, 0) });
        //}


        //private int EvaluateLine(string cell1, string cell2, string cell3)
        //{
        //    string[] cells = { cell1, cell2, cell3 };
        //    int player1Count = cells.Count(c => c == player1.Symbol);
        //    int player2Count = cells.Count(c => c == player2.Symbol);

        //    if (player1Count == 3)
        //        return 100; 
        //    if (player2Count == 3)
        //        return -100; 

        //    if (player1Count == 2 && player2Count == 0)
        //        return 10; 
        //    if (player2Count == 2 && player1Count == 0)
        //        return -10; 

        //    return 0; 
        //}

        //private int CalculateLineScore(int aiCount, int humanCount)
        //{
        //    int winScore = 100;  
        //    int potentialWinScore = 10;  

        //    if (aiCount == 3)
        //        return winScore;  
        //    else if (humanCount == 3)
        //        return -winScore;  
        //    else if (aiCount == 2 && humanCount == 0)
        //        return potentialWinScore;  
        //    else if (humanCount == 2 && aiCount == 0)
        //        return -potentialWinScore; 

        //    return 0;  
        //}

        //private bool CheckEndGame(List<int> moveScores, List<Tuple<int, int>> idealMoves)
        //{


        //    if (board.CheckForVictory())
        //    {
        //        form.ShowWinningMessage(currentPlayer.Symbol, board.GetWinningCells());
        //        form.DisplayEvaluationScores(moveScores);
        //        return true;
        //    }
        //    if (board.IsDraw())
        //    {
        //        form.SetStatus("It's a draw!");
        //        form.DisableAllButtons();
        //        form.DisplayEvaluationScores(moveScores);
        //        form.DisplayIdealMoveSequences(idealMoves);
        //        return true;
        //    }
        //    return false;
        //}

        //public void IncreaseDifficultyLevel()
        //{
        //    maxDepth++;
        //    form.SetStatus($"Difficulty level increased. Current depth: {maxDepth}");

        //    StartGame();
        //}

        //public void DecreaseDifficultyLevel()
        //{
        //    if (maxDepth > 1)
        //    {
        //        maxDepth--;
        //        form.SetStatus($"Difficulty level decreased. Current depth: {maxDepth}");

        //        StartGame();
        //    }
        //    else
        //    {
        //        form.SetStatus("Cannot decrease difficulty level further.");
        //    }
        //}
        /// <summary>
        /// Resets the game state and starts a new game.
        /// </summary>
        public void ResetGame()
        {
            board.Reset();
            form.ResetButtons();
            form.SetStatus($"{currentPlayer.Symbol}'s turn");
            if (currentPlayer.IsAI)
            {
                AIMove();
            }
        }
        /// <summary>
        /// Simulates a game between two human players.
        /// </summary>
        public void SimulatePlayerVsPlayerGame()
        {
            isPlayerVsPlayer = true;
            player1 = new Player("O", false); 
            player2 = new Player("X", false); 
            currentPlayer = player1; 
            StartGame();
        }
        /// <summary>
        /// Simulates a game between a human player and a computer AI.
        /// </summary>
        public void SimulateComputerVsPlayerGame()
        {
            isPlayerVsPlayer = false;
            player1 = new Player("O", false); 
            player2 = new Player("X", true); 
            currentPlayer = player1; 
            StartGame();
        }
        /// <summary>
        /// Simulates a game between two computer AIs.
        /// </summary>
        public void SimulateComputerVsComputerGame()
        {
            isPlayerVsPlayer = false;
            player1 = new Player("O", true);
            player2 = new Player("X", true);
            currentPlayer = player1;
            StartGame();
        }


    }
}
