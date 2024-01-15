using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SpotsGame
{
    public partial class MainWindow : Window
    {
        private MixTiles _mixTiles = new MixTiles();
        private int _moveCount = 0;
        public MainWindow()
        {
            InitializeComponent();
            var shuffledState = _mixTiles.Shuffle(4);
            InitializeTiles(shuffledState);
        }

        private void SpotsButton_click (object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            int row = Grid.GetRow(clickedButton);
            int column = Grid.GetColumn(clickedButton);

            if (IsMovable(row, column))
            {
                Button emptyButton = FindEmptyButton();
                SwapButtons(clickedButton, emptyButton);
                _moveCount++;

                if (IsGameComplete())
                {
                    MessageBox.Show($"Поздравляем! Игра завершена.\nКоличество ходов: {_moveCount}", "Игра завершена");
                }
                
            }
        }

        private bool IsGameComplete()
        {
            int size = 4;
            int expectedValue = 1;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == size - 1 && j == size - 1)
                    {
                        return _mixTiles.GetShuffledTiles()[i, j] == 0;
                    }

                    if (_mixTiles.GetShuffledTiles()[i, j] != expectedValue)
                    {
                        return false;
                    }

                    expectedValue++;
                }
            }
            return true;
        }

        private void InitializeTiles(int[,] shuffledState)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var button = (Button)GameGrid.Children.Cast<UIElement>().First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);
                    button.Content = shuffledState[i, j] == 0 ? "" : shuffledState[i, j].ToString();
                }
            }
        }

        private bool IsMovable(int row, int column)
        {
            Button emptyButton = FindEmptyButton();
            int emptyRow = Grid.GetRow(emptyButton);
            int emptyCol = Grid.GetColumn(emptyButton);

            return (row == emptyRow && Math.Abs(column - emptyCol) == 1) || (column == emptyCol && Math.Abs(row - emptyRow) == 1);
        }

        private void SwapButtons (Button clickedButton, Button emptyButton)
        {
            int clickedRow = Grid.GetRow(clickedButton);
            int clickedColumn = Grid.GetColumn(clickedButton);
            int emptyRow = Grid.GetRow(emptyButton);
            int emptyColumn = Grid.GetColumn(emptyButton);

            object clickedContent = clickedButton.Content;
            clickedButton.Content = emptyButton.Content;
            emptyButton.Content = clickedContent;

            Grid.SetRow(clickedButton, emptyRow);
            Grid.SetColumn(clickedButton, emptyColumn);
            Grid.SetRow(emptyButton, clickedRow);
            Grid.SetColumn(emptyButton, clickedColumn);

            _mixTiles.SwapTiles(clickedRow, clickedColumn, emptyRow, emptyColumn);
            InitializeTiles(_mixTiles.GetShuffledTiles());
        }

        private Button FindEmptyButton()
        {
            foreach (UIElement element in GameGrid.Children)
            {
                Button button = element as Button;
                if (button != null && button.Content.ToString() == "")
                {
                    return button;
                }
            }
            return null;
        }
    }
}
