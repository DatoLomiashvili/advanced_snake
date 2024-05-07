using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace AdvancedSnake;

public partial class MainWindow : Window
{
    private readonly Dictionary<GridValue, Image> gridValToImage = new()
    {
        { GridValue.Empty, new Image() {Source = Images.Empty } },
        { GridValue.Snake, new Image() {Source = Images.Body } },
        { GridValue.Food, new Image() {Source = Images.Food } },
    };

    private readonly Dictionary<Direction, int> dirToRotation = new()
    {
        { Direction.Up, 0 },
        { Direction.Right, 90 },
        { Direction.Down, 180 },
        { Direction.Left, 270 }
    };
    
    private readonly int rows = 15, cols = 15;
    private readonly Image[,] gridImages;
    private GameState gameState;
    private bool gameRunning;
    public MainWindow()
    {
        InitializeComponent();
        gridImages = SetupGrid();
        gameState = new GameState(rows, cols);
    }
    
    private async Task Rungame()
    {
        Draw();
        await ShowCountDown();
        Overlay.IsVisible = false;
        await GameLoop();
        await ShowGameOver();
        gameState = new GameState(rows, cols);
    }
    
    private async void Pointer_Pressed(object sender, PointerPressedEventArgs e)
    {
        if (Overlay.IsVisible)
        {
            e.Handled = true;
        }

        if (!gameRunning)
        {
            gameRunning = true;
            await Rungame();
            gameRunning = false;
        }
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (gameState.GameOver)
        {
            return;
        }

        switch (e.Key)
        {
            case Key.Left:
                gameState.ChangeDirection(Direction.Left);
                break;
            case Key.Right:
                gameState.ChangeDirection(Direction.Right);
                break;
            case Key.Up:
                gameState.ChangeDirection(Direction.Up);
                break;
            case Key.Down:
                gameState.ChangeDirection(Direction.Down);
                break;
        }
    }

    private async Task GameLoop()
    {
        while (!gameState.GameOver)
        {
            await Task.Delay(200);
            gameState.Move();
            Draw();
        }
    }
    private Image[,] SetupGrid()
    {
        Image[,] images = new Image[rows, cols];
        GameGrid.Rows = rows;
        GameGrid.Columns = cols;
        GameGrid.Width = GameGrid.Height * (cols / (double)rows);
        
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Image image = new()
                {
                    Source = Images.Empty,
                };
                images[r, c] = image;
                GameGrid.Children.Add(image);
            }
        }

        return images;
    }

    private void Draw()
    {
        DrawGrid();
        DrawSnakeHead();
        ScoreText.Text = $"Score {gameState.Score}";
    }

    private void DrawGrid()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                GridValue gridVal = gameState.Grid[r, c];
                gridImages[r, c].Source = gridValToImage[gridVal].Source;
            }
        }
    }

    private void DrawSnakeHead()
    {
        Position headPos = gameState.HeadPosition();
        Image image = gridImages[headPos.Row, headPos.Col];
        image.Source = Images.Head;

        int rotation = dirToRotation[gameState.Dir];
        image.RenderTransform = new RotateTransform(rotation);
    }

    private async Task DrawDeadSnake()
    {
        List<Position> positions = new List<Position>(gameState.snakePositions());
        for (int i = 0; i < positions.Count; i++)
        {
            Position pos = positions[i];
            Bitmap bitmap = (i == 0) ? Images.DeadHead : Images.DeadBody;
            gridImages[pos.Row, pos.Col].Source = bitmap;
            await Task.Delay(50);
        }
    }
    private async Task ShowCountDown()
    {
        for (int i = 3; i >= 1; i--)
        {
            OverlayText.Text = i.ToString();
            await Task.Delay(500);
        }
    }

    private async Task ShowGameOver()
    {
        await DrawDeadSnake();
        await Task.Delay(1000);
        Overlay.IsVisible = true;
        OverlayText.Text = "PRESS THE MOUSE TO START";
    }
    
}