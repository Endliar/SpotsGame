using System;
using System.Collections.Generic;

namespace SpotsGame
{
    public class MixTiles
    {
        private Random _random = new Random();
        private int[,] _tiles;

        public int[,] Shuffle(int size)
        {
            InitializeGame(size);
            for (int moves = 0; moves < 1000; moves++)
            {
                MoveEmptyRandomly(size);
            }

            return (int[,])_tiles.Clone();
        }

        public void SwapTiles(int row1, int col1, int row2, int col2)
        {
            int temp = _tiles[row1, col1];
            _tiles[row1, col1] = _tiles[row2, col2];
            _tiles[row2, col2] = temp;
        }

        public void InitializeGame(int size)
        {
            _tiles = new int[size, size];
            int count = 1;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _tiles[i, j] = count++;
                }
            }
            _tiles[size - 1, size - 1] = 0;
        }

        public int[,] GetShuffledTiles()
        {
            return (int[,])_tiles.Clone();
        }

        private void MoveEmptyRandomly(int size)
        {
            int emptyRow = 0, emptyCol = 0;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (_tiles[i, j] == 0)
                    {
                        emptyRow = i;
                        emptyCol = j;
                    }
                }
            }

            var validMoves = new List<(int, int)>();
            if (emptyRow > 0) validMoves.Add((emptyRow - 1, emptyCol));
            if (emptyRow < size - 1) validMoves.Add((emptyRow + 1, emptyCol));
            if (emptyCol > 0) validMoves.Add((emptyRow, emptyCol - 1));
            if (emptyCol < size - 1) validMoves.Add((emptyRow, emptyCol + 1));

            var move = validMoves[_random.Next(validMoves.Count)];
            SwapTiles(emptyRow, emptyCol, move.Item1, move.Item2);
        }
    }
}
