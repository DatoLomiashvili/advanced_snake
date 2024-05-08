using System;
using System.Collections.Generic;

namespace AdvancedSnake;

public class GameState
{
    public int Rows { get; }
    public int Cols { get; }
    public GridValue[,] Grid { get; }
    public Direction Dir { get; private set; }
    public int Score { get; private set; }
    public bool GameOver { get; private set; }

    private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();
    private readonly LinkedList<Position> SnakePositions = new LinkedList<Position>();
    private readonly Random rnd = new Random();
    

    public GameState(int rows, int cols)
    {
        Rows = rows;
        Cols = cols;
        Grid = new GridValue[rows, cols];
        Dir = Direction.Right;
        
        AddSnake();
        AddFood();
    }

    private void AddSnake()
    {
        int r = Rows / 2;
        for (int c = 1; c <= 3; c++)
        {
            Grid[r, c] = GridValue.Snake;
            SnakePositions.AddFirst(new Position(r, c));
        }
    }

    private IEnumerable<Position> EmptyPositions()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Cols; c++)
            {
                if (Grid[r, c] == GridValue.Empty)
                {
                    yield return new Position(r, c);
                }
            }
        }
    }

    private void AddFood()
    {
        List<Position> emptyPositions = new List<Position>(EmptyPositions());

        if (emptyPositions.Count == 0)
        {
            return;
        }

        Position pos = emptyPositions[rnd.Next(emptyPositions.Count)];

        Grid[pos.Row, pos.Col] = GridValue.Food;
    }

    public Position HeadPosition()
    {
        return SnakePositions.First!.Value;
    }
    
    public Position TailPosition()
    {
        return SnakePositions.Last!.Value;
    }

    public IEnumerable<Position> snakePositions()
    {
        return SnakePositions;
    }

    private void AddHead(Position pos)
    {
        SnakePositions.AddFirst(pos);
        Grid[pos.Row, pos.Col] = GridValue.Snake;
    }
    
    private void RemoveTail(Position pos)
    {
        Position tailPosition = SnakePositions.Last!.Value;
        SnakePositions.RemoveLast();
        Grid[tailPosition.Row, tailPosition.Col] = GridValue.Empty;
    }

    private Direction GetLastDirection()
    {
        if (dirChanges.Count == 0)
        {
            return Dir;
        }

        return dirChanges.Last.Value;
    }

    private bool CanChangeDirection(Direction newDir)
    {
        if (dirChanges.Count == 2)
        {
            return false;
        }

        Direction lastDir = GetLastDirection();
        return newDir != lastDir && newDir != lastDir.Opposite();
    }
    public void ChangeDirection(Direction direction)
    {
        if (CanChangeDirection(direction))
        {
            dirChanges.AddLast(direction);
        }
    }

    private bool OutsideGrid(Position pos)
    {
        return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;
    }

    private GridValue WillHit(Position newHeadPos)
    {
        if (OutsideGrid(newHeadPos))
        {
            return GridValue.Outside;
        }
        
        if (newHeadPos == TailPosition())
        {
            return GridValue.Empty;
        }
        
        return Grid[newHeadPos.Row, newHeadPos.Col];
    }

    public void Move()
    {
        if (dirChanges.Count > 0)
        {
            Dir = dirChanges.First.Value;
            dirChanges.RemoveFirst();
        }
        Position newHeadPos = HeadPosition().Translate(Dir);
        GridValue hit = WillHit(newHeadPos);

        switch (hit)
        {
            case GridValue.Snake:
                GameOver = true;
                break;
            case GridValue.Outside:
            case GridValue.Empty:
                RemoveTail(TailPosition());
                AddHead(newHeadPos);
                break;
            case GridValue.Food:
                AddHead(newHeadPos);
                Score++;
                AddFood();
                break;
        }
    }
}