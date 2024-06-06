using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        private Button[,] buttons = new Button[3, 3];
        private Label statusLabel = new Label(); 
        private GameManager gameManager;
        private string selectedDifficulty;
        private Button playerVsPlayerButton = new Button();
        private Button computerVsplayerButton = new Button();
        private Button computerVsComputerButton = new Button();


        private Button resetButton = new Button();
        private Label minScoreLabel = new Label();
        private Label maxScoreLabel = new Label();
        private Stack<Tuple<int, int>> moveHistory = new Stack<Tuple<int, int>>();

        public Form1()
        {
            InitializeComponent();
            InitializeResetButton();
            ShowDifficultyScreen(); 
            InitializeGameModeButtons();
            InitializeGameButtons();

        }
        /// <summary>
        /// This ShowDifficultyScreen method creates a form
        /// that allows the user to select a difficulty level for
        /// the Tic-Tac-Toe game.
        /// </summary>
        private void ShowDifficultyScreen()
        {
            var difficultyForm = new Form
            {
                Text = "Select Difficulty",
                Size = new Size(400, 300),
                FormBorderStyle = FormBorderStyle.FixedSingle,
                MaximizeBox = false,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label label = new Label
            {
                Text = "Select Difficulty Level:",
                AutoSize = true,
                Location = new Point(10, 10)
            };

            Button easyButton = CreateDifficultyButton("Easy", 50, 40, difficultyForm);
            Button mediumButton = CreateDifficultyButton("Medium", 50, 70, difficultyForm);
            Button hardButton = CreateDifficultyButton("Hard", 50, 100, difficultyForm);

            difficultyForm.Controls.AddRange(new Control[] { label, easyButton, mediumButton, hardButton });
            difficultyForm.ShowDialog();

            InitializeGame(selectedDifficulty); 
        }
        /// <summary>
        /// creates a Button control with specified properties and
        /// attaches a Click event handler to it
        /// </summary>
        /// <param name="difficulty"></param>
        /// <param name="x">Set the button's position</param>
        /// <param name="y">Set the button's size</param>
        /// <param name="parentForm"></param>
        /// <returns>Return the created button</returns>
        private Button CreateDifficultyButton(string difficulty, int x, int y, Form parentForm)
        {
            var button = new Button
            {
                Text = difficulty,
                Location = new Point(x, y),
                Size = new Size(75, 30)
            };
            button.Click += (sender, e) => { selectedDifficulty = difficulty; parentForm.Close(); };
            return button;
        }
        /// <summary>
        /// method is responsible for setting up the game 
        /// environment and initializing the necessary components
        /// for a game of Tic-Tac-Toe
        /// </summary>
        /// <param name="difficulty">the chosen difficulty by the user</param>
        private void InitializeGame(string difficulty)
        {
            gameManager = new GameManager(difficulty, this);
            InitializeGameBoard();
            InitializeScoreLabels();
            InitializeStatusLabel();
            this.Text = "Tic-Tac-Toe";
            this.Size = new Size(300, 530);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }
        /// <summary>
        /// is responsible for creating and
        /// setting up the Tic-Tac-Toe grid (game board) with buttons.
        /// </summary>
        private void InitializeGameBoard()
        {
            int btnSize = 80;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    buttons[i, j] = new Button
                    {
                        Size = new Size(btnSize, btnSize),
                        Location = new Point(j * btnSize, i * btnSize),
                        Font = new Font("Arial", 20F, FontStyle.Bold),
                        BackColor = Color.AliceBlue
                    };
                    buttons[i, j].Click += Button_Click; 
                    this.Controls.Add(buttons[i, j]);
                }
            }
        }
        /// <summary>
        /// method is responsible for setting
        /// up the game mode selection buttons on the form
        /// </summary>
        private void InitializeGameModeButtons()
        {
        
            playerVsPlayerButton.Text = "Player vs Player";
            playerVsPlayerButton.Size = new Size(150, 30);
            playerVsPlayerButton.Location = new Point(125, 430);
            playerVsPlayerButton.Click += PlayerVsPlayerButton_Click;
            this.Controls.Add(playerVsPlayerButton);

          
            computerVsplayerButton.Text = "Computer vs Player";
            computerVsplayerButton.Size = new Size(150, 30);
            computerVsplayerButton.Location = new Point(125, 470);
            computerVsplayerButton.Click += computerVsplayerButton_Click;
            this.Controls.Add(computerVsplayerButton);

            computerVsComputerButton.Text = "Computer vs Computer";
            computerVsComputerButton.Size = new Size(150, 30);
            computerVsComputerButton.Location = new Point(125, 510); // Incorrect Y coordinate
            computerVsComputerButton.Click += ComputerVsComputerButton_Click;
            this.Controls.Add(computerVsComputerButton);
        }
        /// <summary>
        /// method is responsible for positioning the game mode buttons
        /// (playerVsPlayerButton, computerVsplayerButton,
        /// and computerVsComputerButton) relative to the resetButton.
        /// </summary>
        private void InitializeGameButtons()
        {
            int buttonSpacing = 10;

            playerVsPlayerButton.Location = new Point(resetButton.Location.X, resetButton.Location.Y + resetButton.Height + buttonSpacing);
            computerVsplayerButton.Location = new Point(resetButton.Location.X, playerVsPlayerButton.Location.Y + playerVsPlayerButton.Height + buttonSpacing);
            computerVsComputerButton.Location = new Point(resetButton.Location.X, computerVsplayerButton.Location.Y + computerVsplayerButton.Height + buttonSpacing); // Adjusted Y coordinate
        }



        private void PlayerVsPlayerButton_Click(object sender, EventArgs e)
        {
            gameManager.SimulatePlayerVsPlayerGame();
        }

        private void computerVsplayerButton_Click(object sender, EventArgs e)
        {
            gameManager.SimulateComputerVsPlayerGame();
        }
        private void ComputerVsComputerButton_Click(object sender, EventArgs e)
        {
            gameManager.SimulateComputerVsComputerGame();
        }
        /// <summary>
        /// method is responsible for creating
        /// and positioning the reset button (resetButton) on the form.
        /// </summary>
        private void InitializeResetButton()
        {
            int buttonWidth = 100;
            int buttonHeight = 30;
            int bottomOfGrid = 240;
            int marginBetweenGridAndButton = 40;

            int centerX = (this.ClientSize.Width - buttonWidth) / 25;

            resetButton.Text = "Reset Game";
            resetButton.Size = new Size(buttonWidth, buttonHeight);
            resetButton.Location = new Point(centerX, bottomOfGrid + marginBetweenGridAndButton);
            resetButton.Click += ResetButton_Click;

            this.Controls.Add(resetButton);
        }

        /// <summary>
        /// method is an event handler for the click event of the
        /// reset button (resetButton). When the reset button
        /// is clicked, this method is called to reset the game state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButton_Click(object sender, EventArgs e)
        {
            gameManager.ResetGame();
            moveHistory.Clear();
            foreach (var button in buttons)
                button.Text = "";
        }
        /// <summary>
        ///  method initializes and sets up the score
        ///  labels (minScoreLabel and maxScoreLabel) on
        ///  the form
        /// </summary>
        private void InitializeScoreLabels()
        {
            minScoreLabel.Size = new Size(150, 20);
            minScoreLabel.Location = new Point(0, 260);
            minScoreLabel.Font = new Font("Arial", 10F, FontStyle.Regular);
            minScoreLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(minScoreLabel);

            maxScoreLabel.Size = new Size(150, 20);
            maxScoreLabel.Location = new Point(150, 260);
            maxScoreLabel.Font = new Font("Arial", 10F, FontStyle.Regular);
            maxScoreLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(maxScoreLabel);
        }
        /// <summary>
        /// This method effectively displays the evaluation
        /// scores both in the console and on the UI,
        /// and provides functionality to clear the scores
        /// when the labels are clicked.
        /// </summary>
        /// <param name="moveScores"></param>
        public void DisplayEvaluationScores(List<int> moveScores)
        {
            Console.WriteLine("Evaluation Scores:");
            if (moveScores.Any())
            {
                foreach (var score in moveScores)
                {
                    Console.WriteLine($"Score: {score}");
                }

                var minScore = moveScores.Min();
                var maxScore = moveScores.Max();

                minScoreLabel.Text = $"Min Score: {minScore}";
                maxScoreLabel.Text = $"Max Score: {maxScore}";
            }
            else
            {
           
                Console.WriteLine("No scores to evaluate.");
                minScoreLabel.Text = "Min Score: N/A";
                maxScoreLabel.Text = "Max Score: N/A";
            }

            
            minScoreLabel.Click += ClearScoreLabel;
            maxScoreLabel.Click += ClearScoreLabel;
        }



        /// <summary>
        /// Clears the text of the label that triggered the event.
        /// This method is an event handler that resets the text of a label when it's clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearScoreLabel(object sender, EventArgs e)
            {
                var label = (Label)sender;
                label.Text = "";
            }
        /// <summary>
        ///  Initializes the status label with specific size,
        ///  location, font, and alignment, and adds
        ///  it to the form's controls.
        /// </summary>
        private void InitializeStatusLabel()
            {
                statusLabel.Size = new Size(300, 20);
                statusLabel.Location = new Point(0, 240);
                statusLabel.Font = new Font("Arial", 10F, FontStyle.Regular);
                statusLabel.TextAlign = ContentAlignment.MiddleCenter;
                this.Controls.Add(statusLabel);
            }
        /// <summary>
        ///  Handles the click event for buttons on the game board.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, EventArgs e)
            {
                Button clickedButton = sender as Button;
                int i = clickedButton.Location.Y / clickedButton.Height;
                int j = clickedButton.Location.X / clickedButton.Width;
                gameManager.PlayerMove(i, j);
                moveHistory.Push(new Tuple<int, int>(i, j));
            }


        /// <summary>
        /// Updates the text of the status label.
        /// </summary>
        /// <param name="text"></param>
        public void SetStatus(string text)
            {
                statusLabel.Text = text;
            }
        /// <summary>
        ///  Updates the appearance and state (text and enabled state) 
        ///  of a button at position (x, y) on the game board.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="symbol"></param>
        public void UpdateButton(int x, int y, string symbol)
            {
                if (buttons[x, y].InvokeRequired)
                {
                    buttons[x, y].Invoke(new MethodInvoker(() => {
                        UpdateButton(x, y, symbol);
                    }));
                    return;
                }

                if (buttons[x, y].Enabled)
                {
                    buttons[x, y].Text = symbol;
                    buttons[x, y].Enabled = false;
                }
            }


        //public void DisableButton(int i, int j)
        //    {
        //        if (buttons[i, j].Enabled)
        //        {
        //            buttons[i, j].Enabled = false;
        //        }
        //    }
        /// <summary>
        /// Disables all buttons on the game board.
        /// </summary>
        public void DisableAllButtons()
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        buttons[i, j].Enabled = false;
                    }
                }
            }
        /// <summary>
        ///  Resets all buttons on the game board to their default state.
        /// </summary>
        public void ResetButtons()
        {
            if (buttons != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (buttons[i, j] != null)
                        {
                            buttons[i, j].Text = "";
                            buttons[i, j].Enabled = true;
                        }
                    }
                }
            }
        }


        //private void ResizeGameBoard(int size)
        //    {
        //        this.SuspendLayout();
        //        foreach (Button btn in buttons)
        //        {
        //            this.Controls.Remove(btn);
        //        }
        //        buttons = new Button[size, size];
        //        InitializeGameBoard();
        //        this.ResumeLayout();
        //    }
        /// <summary>
        /// Displays a message box indicating the winner
        /// and highlights the winning cells on the game board.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="winningCells"></param>
        public void ShowWinningMessage(string symbol, int[] winningCells)
        {
            HighlightWinningMoves(winningCells);
            MessageBox.Show($"{symbol} wins!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RemoveColorHighlight(winningCells);
            DisableAllButtons();
        }
        /// <summary>
        ///  Highlights the winning cells on the game
        ///  board by changing their background color to green.

        /// </summary>
        /// <param name="winningCells"></param>
        private void HighlightWinningMoves(int[] winningCells)
        {
            foreach (int index in winningCells)
            {
                int i = index / 3; 
                int j = index % 3;
                buttons[i, j].BackColor = Color.Green;
            }
        }
        /// <summary>
        /// Removes the color highlight from the previously highlighted winning cells.
        /// </summary>
        /// <param name="winningCells"></param>
        private void RemoveColorHighlight(int[] winningCells)
        {
            foreach (int index in winningCells)
            {
                int i = index / 3; 
                int j = index % 3;
                buttons[i, j].BackColor = DefaultBackColor;
            }
        }



        //private void UndoLastMove()
        //    {
        //        if (moveHistory.Count > 0)
        //        {
        //            var lastMove = moveHistory.Pop();
        //            int i = lastMove.Item1;
        //            int j = lastMove.Item2;
        //            buttons[i, j].Text = "";
        //            buttons[i, j].BackgroundImage = null;
        //            buttons[i, j].Enabled = true;
        //        }
        //    }

            //private void ResizeBoardMenuItem_Click(object sender, EventArgs e)
            //{
            //    int newSize = 0;
            //    ResizeGameBoard(newSize);
            //}

            //private void UndoLastMoveMenuItem_Click(object sender, EventArgs e)
            //{
            //    UndoLastMove();
            //}



            //public void DisplayIdealMoveSequences(List<Tuple<int, int>> idealMoves)
            //{
            //    Console.WriteLine("Ideal Move Sequences:");
            //    foreach (var move in idealMoves)
            //    {
            //        Console.WriteLine($"Move: ({move.Item1}, {move.Item2})");
            //    }
            //}

            private void Form1_Load(object sender, EventArgs e)
            {
                // Add initialization code as needed.
            }
        }
    
}
