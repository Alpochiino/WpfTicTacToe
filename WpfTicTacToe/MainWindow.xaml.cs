namespace WpfTicTacToe
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Linq;

    public partial class MainWindow : Window
    {
        private string[,] board = new string[3, 3];
        private int playerScore = 0;
        private int computerScore = 0;
        private bool gameOver = false;

        public MainWindow()
        {
            InitializeComponent();
            ResetGame();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gameOver) return;

            Button clickedButton = sender as Button;
            int row = Grid.GetRow(clickedButton);
            int col = Grid.GetColumn(clickedButton);

            if (board[row, col] != null) return;

            MakeMove(row, col, "X", Brushes.Blue);

            if (CheckWinner("X"))
            {
                playerScore++;
                return;
            }

            if (!IsBoardFull())
            {
                ComputerMove();
            }
            else
            {
                ShowResult("Oavgjort!");
            }
        }

        private void ComputerMove()
        {
            if (MakeStrategicMove("X")) return;
            if (MakeStrategicMove("O")) return;

            MakeRandomMove();
        }

        private bool MakeStrategicMove(string opponent)
        {
            string player = opponent == "X" ? "O" : "X";
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == null)
                    {
                        board[row, col] = player;
                        if (CheckWinner(player))
                        {
                            MakeMove(row, col, "O", Brushes.Red);
                            return true;
                        }
                        board[row, col] = null;
                    }
                }
            }
            return false;
        }

        private void MakeRandomMove()
        {
            var random = new Random();
            while (true)
            {
                int row = random.Next(0, 3);
                int col = random.Next(0, 3);
                if (board[row, col] == null)
                {
                    MakeMove(row, col, "O", Brushes.Red);
                    break;
                }
            }
        }

        private void MakeMove(int row, int col, string content, Brush color)
        {
            board[row, col] = content;
            UpdateButton(row, col, content, color);
            if (CheckWinner(content))
            {
                if (content == "O") computerScore++;
                else playerScore++;
                UpdateScoreDisplay();
                ShowResult(content == "O" ? "Datorn vinner!" : "Du vinner!");
                gameOver = true;
            }
        }

        private void UpdateButton(int row, int col, string content, Brush color)
        {
            string buttonName = $"Btn{row}{col}";
            Button btn = (Button)this.FindName(buttonName);
            btn.Content = content;
            btn.Background = color;
        }

        private bool CheckWinner(string player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) return true;
                if (board[0, i] == player && board[1, i] == player && board[2, i] == player) return true;
            }

            if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) return true;
            if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player) return true;

            return false;
        }

        private bool IsBoardFull() => Enumerable.Cast<string>(board).All(cell => cell != null);

        private void ResetGame()
        {
            board = new string[3, 3];
            foreach (var child in GameGrid.Children)
            {
                if (child is Button button)
                {
                    button.Content = "";
                    button.Background = Brushes.LightGray;
                }
            }
            gameOver = false;
        }

        private void ShowResult(string message)
        {
            MessageBox.Show(message);
            UpdateScoreDisplay();
        }

        private void UpdateScoreDisplay()
        {
            PlayerScoreText.Text = playerScore.ToString();
            ComputerScoreText.Text = computerScore.ToString();
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e) => ResetGame();
    }
}
