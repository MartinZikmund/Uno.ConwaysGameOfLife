using System;

namespace ConwaysGameOfLife.Shared.Models
{
    public class GameState
    {
        private Random _random = new Random();

        public GameState(int size)
        {
            Cells = new CellState[size, size];
            Size = size;
        }

        public int Size { get; }

        public CellState[,] Cells { get; set; }

        public void Randomize()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    Cells[row, column] = (CellState)_random.Next(0, 2);
                }
            }
        }

        public void Tick()
        {
            var nextGeneration = new CellState[Size, Size];

            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    var aliveNeighbors = CountAliveNeighbors(row, column);

                    if (aliveNeighbors < 2)
                    {
                        nextGeneration[row, column] = CellState.Dead;
                    }
                    else if (aliveNeighbors == 3)
                    {
                        nextGeneration[row, column] = CellState.Alive;
                    }
                    else if (aliveNeighbors > 3)
                    {
                        nextGeneration[row, column] = CellState.Dead;
                    }
                    else
                    {
                        nextGeneration[row, column] = Cells[row, column];
                    }
                }
            }

            Cells = nextGeneration;
        }

        private int CountAliveNeighbors(int row, int column)
        {
            int aliveCount = 0;
            for (var rowOffset = -1; rowOffset <= 1; rowOffset++)
            {
                for (var columnOffset = -1; columnOffset <= 1; columnOffset++)
                {
                    if (rowOffset == 0 && columnOffset == 0)
                    {
                        continue;
                    }
                    var targetRow = row + rowOffset;
                    var targetColumn = column + columnOffset;
                    if (targetRow >= 0 && targetColumn >= 0 &&
                        targetRow < Size && targetColumn < Size)
                    {
                        aliveCount += (int)Cells[targetRow, targetColumn];
                    }
                }
            }

            return aliveCount;
        }
    }
}
