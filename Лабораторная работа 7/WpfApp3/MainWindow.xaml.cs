using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLife
{
    public partial class MainWindow : Window
    {
        private const int CellSize = 10;
        private const int Rows = 60;
        private const int Cols = 60;
        private bool[,] grid = new bool[Rows, Cols];
        private bool isRunning = false;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            InitTimer();
            DrawGrid();
        }

        private void InitTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isRunning)
            {
                UpdateGrid();
                DrawGrid();
            }
        }

        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            isRunning = !isRunning;
            StartStopButton.Content = isRunning ? "Stop" : "Start";

            if (isRunning)
                timer.Start();
            else
                timer.Stop();
        }

        private void UpdateGrid()
        {
            bool[,] newGrid = new bool[Rows, Cols];

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    int neighbors = CountNeighbors(x, y);

                    if (grid[y, x])
                    {
                        newGrid[y, x] = neighbors == 2 || neighbors == 3;
                    }
                    else
                    {
                        newGrid[y, x] = neighbors == 3;
                    }
                }
            }

            grid = newGrid;
        }

        private int CountNeighbors(int x, int y)
        {
            int count = 0;

            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0) continue;

                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx >= 0 && nx < Cols && ny >= 0 && ny < Rows)
                    {
                        if (grid[ny, nx]) count++;
                    }
                }
            }

            return count;
        }

        private void DrawGrid()
        {
            GameCanvas.Children.Clear();

            for (int y = 0; y < Rows; y++)
            {
                for (int x = 0; x < Cols; x++)
                {
                    if (grid[y, x])
                    {
                        Rectangle rect = new Rectangle
                        {
                            Width = CellSize - 1,
                            Height = CellSize - 1,
                            Fill = Brushes.Black
                        };
                        Canvas.SetLeft(rect, x * CellSize);
                        Canvas.SetTop(rect, y * CellSize);
                        GameCanvas.Children.Add(rect);
                    }
                }
            }
        }

        private void GameCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ToggleCell(e.GetPosition(GameCanvas));
        }

        private void GameCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                ToggleCell(e.GetPosition(GameCanvas));
            }
        }

        private void ToggleCell(Point position)
        {
            int x = (int)(position.X / CellSize);
            int y = (int)(position.Y / CellSize);

            if (x >= 0 && x < Cols && y >= 0 && y < Rows)
            {
                grid[y, x] = true;
                DrawGrid();
            }
        }
    }
}
