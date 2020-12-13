using System;

namespace ConwaysGameOfLife.Shared.Models
{
    public class GameState
    {
        private readonly Random _random = new Random();


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

        public void Clear()
        {
            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    Cells[row, column] = CellState.Dead;
                }
            }
        }

        private CellState[,] _nextGeneration = null;

        public void Tick()
        {
            _nextGeneration = _nextGeneration ?? new CellState[Size, Size];

            for (int row = 0; row < Size; row++)
            {
                for (int column = 0; column < Size; column++)
                {
                    var aliveNeighbors = CountAliveNeighbors(row, column);
                    var currentState = Cells[row, column];

                    CellState nextState = Cells[row, column];

                    if (currentState == CellState.Alive && aliveNeighbors < 2)
                    {
                        // Rule 1 - Underpopulation
                        nextState = CellState.Dead;
                    }
                    else if (
                        currentState == CellState.Alive && 
                        (aliveNeighbors == 2 || aliveNeighbors <= 3))
                    {
                        // Rule 2
                        nextState = CellState.Alive;
                    }
                    else if (currentState == CellState.Alive && aliveNeighbors > 3)
                    {
                        // Rule 3 - Overpopulation
                        nextState = CellState.Dead;
                    }
                    else if (currentState == CellState.Dead && aliveNeighbors == 3)
                    {
                        // Rule 4 - Reproduction
                        nextState = CellState.Alive;
                    }

                    _nextGeneration[row, column] = nextState;
                }
            }

            // Swap!
            (Cells, _nextGeneration) = (_nextGeneration, Cells);
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
