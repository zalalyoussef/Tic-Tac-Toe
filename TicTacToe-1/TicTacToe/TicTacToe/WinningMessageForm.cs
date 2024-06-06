using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class WinningMessageForm : Form
    {
        private int[] winningCells;
        private Button[,] buttons = new Button[3, 3];
        private Label labelMessage = new Label();

        public WinningMessageForm(string symbol, int[] winningCells)
        {
            InitializeComponent();
            HighlightWinningMoves(winningCells);
            labelMessage.Text = $"{symbol} wins!";
            this.winningCells = winningCells;
        }

        private void HighlightWinningMoves(int[] winningCells)
        {
            foreach (int index in winningCells)
            {
                int i = index / 3; // Assuming a 3x3 grid
                int j = index % 3;
                if (buttons[i, j] != null) // Check if the button at (i, j) is not null
                {
                    buttons[i, j].BackColor = Color.Green; // Highlight winning buttons
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            RemoveColorHighlight();
            this.Close();
        }

        private void RemoveColorHighlight()
        {
            foreach (int index in winningCells)
            {
                int i = index / 3; // Assuming a 3x3 grid
                int j = index % 3;
                buttons[i, j].BackColor = DefaultBackColor; // Reset button background color
            }
        }
        private void WinningMessageForm_Load(object sender, EventArgs e)
        {
            // Add initialization code as needed.
        }
    }
}

