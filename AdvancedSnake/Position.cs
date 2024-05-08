using System;
using System.Collections.Generic;

namespace AdvancedSnake;

public class Position
{
    public int Row { get; }
    public int Col { get; }

    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public Position Translate(Direction direction)
    {
        int rowOffSet, colOffSet;
        if (Row + direction.RowOffSet < 0)
        {
            rowOffSet = 14;
        }
        else if(Row + direction.RowOffSet > 14)
        {
            rowOffSet = 0;
        }
        else
        {
            rowOffSet = Row + direction.RowOffSet;
        }
        
        if (Col + direction.ColOffSet < 0)
        {
            colOffSet = 14;
        }
        else if(Col + direction.ColOffSet > 14)
        {
            colOffSet = 0;
        }
        else
        {
            colOffSet = Col + direction.ColOffSet;
        }
        return new Position(rowOffSet, colOffSet);
    }
    
    public override bool Equals(object obj)
    {
        return obj is Position position &&
               Row == position.Row &&
               Col == position.Col;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Col);
    }

    public static bool operator ==(Position left, Position right)
    {
        return EqualityComparer<Position>.Default.Equals(left, right);
    }
    
    public static bool operator !=(Position left, Position right)
    {
        return !(left == right);
    }
}