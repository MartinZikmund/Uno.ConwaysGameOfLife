using ConwaysGameOfLife.Shared.Models;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ConwaysGameOfLife
{
    public sealed partial class MainPage : Page
    {
        private readonly List<Image> _cells = new List<Image>();
        private readonly DispatcherTimer _timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromSeconds(1)
        };

        private GameState _gameState;
        private GameTheme _currentTheme = GameTheme.Classic;

        private BitmapImage _aliveBitmap;
        private BitmapImage _deadBitmap;

        public MainPage()
        {
            this.InitializeComponent();
            UpdateBitmaps();
            StartNewGame();
            _timer.Tick += RedrawTimer_Tick;
        }

        public GameTheme[] Themes { get; } = new GameTheme[]
        {
            GameTheme.Classic,
            GameTheme.Bacteria,
            GameTheme.CatMouse,
            GameTheme.FireWater
        };

        public GameTheme CurrentTheme
        {
            get => _currentTheme;
            set
            {
                _currentTheme = value;
                UpdateBitmaps();
                RedrawBoard();
            }
        }

        private void AutoPlayToggled()
        {
            if (AutoPlayToggleSwitch.IsOn)
            {
                _timer.Start();
            }
            else
            {
                _timer.Stop();
            }
        }

        private void StartNewGame()
        {
            var boardSize = (int)BoardSizeNumberBox.Value;
            if (_gameState?.Size != boardSize)
            {
                _gameState = new GameState(boardSize);        
                PrepareGrid();
                LayoutGameBoard();
            }
            _gameState.Randomize();
            RedrawBoard();
        }

        private void Clear()
        {
            _gameState?.Clear();
            RedrawBoard();
        }

        private void NextGeneration()
        {
            _gameState?.Tick();
            RedrawBoard();
        }

        private void RedrawBoard()
        {
            for (var row = 0; row < _gameState.Size; row++)
            {
                for (var column = 0; column < _gameState.Size; column++)
                {
                    var cell = GetCell(row, column);
                    cell.Source = _gameState.Cells[row, column] == CellState.Alive ?
                        _aliveBitmap : _deadBitmap;
                }
            }
        }

        private void RedrawTimer_Tick(object sender, object e)
        {
            _gameState?.Tick();
            RedrawBoard();
        }

        private void PrepareGrid()
        {
            GameCanvas.Children.Clear();

            for (var row = 0; row < _gameState.Size; row++)
            {
                for (var column = 0; column < _gameState.Size; column++)
                {
                    var cell = GetCell(row, column);
                    cell.Source = _gameState.Cells[row, column] == CellState.Alive ?
                        _aliveBitmap : _deadBitmap;
                    GameCanvas.Children.Add(cell);
                }
            }
        }

        private Image GetCell(int row, int column)
        {
            var index = row * _gameState.Size + column;
            return GetCell(index);
        }

        private Image GetCell(int order)
        {
            while (_cells.Count <= order)
            {
                _cells.Add(CreateCell());
            }

            return _cells[order];
        }

        private Image CreateCell()
        {
            var cell = new Image();
            cell.Tapped += OnCellTapped;
            return cell;
        }

        private void LayoutGameBoard()
        {
            var minDimension = Math.Min(GameCanvasContainer.ActualHeight, GameCanvasContainer.ActualWidth);
            var cellSize = (int)minDimension / _gameState.Size;
            // make cell size a multiple of 4 for proper scaling
            cellSize = (cellSize / 4) * 4;

            if (cellSize <= 0)
            {
                cellSize = 4;
            }

            GameCanvas.Height = GameCanvas.Width = cellSize * _gameState.Size;

            for (var row = 0; row < _gameState.Size; row++)
            {
                for (var column = 0; column < _gameState.Size; column++)
                {
                    var cell = GetCell(row, column);
                    cell.SetValue(Canvas.LeftProperty, (double)(column * cellSize));
                    cell.SetValue(Canvas.TopProperty, (double)(row * cellSize));
                    cell.Height = cellSize - 1;
                    cell.Width = cellSize - 1;
                }
            }
        }

        private void UpdateBitmaps()
        {
            _aliveBitmap = new BitmapImage(new Uri($"ms-appx:///Assets/{_currentTheme}_alive.png"));
            _deadBitmap = new BitmapImage(new Uri($"ms-appx:///Assets/{_currentTheme}_dead.png"));
        }        

        private void OnCellTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var index = _cells.IndexOf((Image)sender);

            int row = index / _gameState.Size;
            int column = index % _gameState.Size;

            _gameState.Cells[row, column] = _gameState.Cells[row, column] == CellState.Alive ?
                    CellState.Dead : CellState.Alive;
            RedrawBoard();
        }
    }
}
