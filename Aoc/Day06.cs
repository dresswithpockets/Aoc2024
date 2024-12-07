using System.Text;
using CommunityToolkit.HighPerformance;

namespace Aoc;

public class Day06 : ISimpleSolution
{
    public string FilePath => "Inputs/06.txt";
    public string Solution1 => _solution1.ToString();
    public string Solution2 => _solution2.ToString();
    private int _solution1;
    private int _solution2;

    public void Solve(ReadOnlySpan<char> input)
    {
        Span<char> mutableInput = stackalloc char[input.Length];
        input.CopyTo(mutableInput);

        var width = mutableInput.IndexOf('\n');
        var height = (mutableInput.Length + 1) / (width + 1);

        var grid = mutableInput.AsSpan2D(0, width, height, 1);
        var startingPosition = new Vector2I(0, 0);
        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            if (grid[y, x] == '^')
            {
                startingPosition = new Vector2I(x, y);
                break;
            }
        }

        var currentPosition = startingPosition;
        // 0: North, 1: East, 2: South, 3: West
        var currentDirection = 0;
        
        var positionsTravelled = new HashSet<Vector2I>();

        // positions until guard leaves grid
        while (true)
        {
            positionsTravelled.Add(currentPosition);
            if (Iterate(ref grid, ref currentPosition, ref currentDirection)) continue;

            positionsTravelled.Add(currentPosition);
            break;
        }

        _solution1 = positionsTravelled.Count;
        
        // positions until guard loops with obstacle
        positionsTravelled.Remove(startingPosition);
        var loopCount = 0;
        var positionDirectionTravelled = new HashSet<PositionDirection>();
        foreach (var testPosition in positionsTravelled)
        {
            grid[testPosition.Y, testPosition.X] = '#';
            
            currentPosition = startingPosition;
            currentDirection = 0;

            positionDirectionTravelled.Clear();
            while (true)
            {
                if (!positionDirectionTravelled.Add(new PositionDirection(currentPosition, currentDirection)))
                {
                    loopCount += 1;
                    break;
                }

                if (Iterate(ref grid, ref currentPosition, ref currentDirection))
                    continue;
                // we left the grid, no loop
                break; 
            }

            grid[testPosition.Y, testPosition.X] = '.';
        }
        
        _solution2 = loopCount;
    }

    private static bool Iterate(ref Span2D<char> grid, ref Vector2I position, ref int direction)
    {
        var move = direction switch
        {
            0 => new Vector2I(0, -1),
            1 => new Vector2I(1, 0),
            2 => new Vector2I(0, 1),
            3 => new Vector2I(-1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
        
        var nextPosition = new Vector2I(position.X + move.X, position.Y + move.Y);
        if (nextPosition.X < 0 || nextPosition.X >= grid.Width || nextPosition.Y < 0 || nextPosition.Y >= grid.Height)
            return false;

        if (grid[nextPosition.Y, nextPosition.X] == '#')
            direction = (direction + 1) % 4;
        else
            position = nextPosition;
        
        // PrintGrid(ref grid, ref position, ref direction);
        
        return true;
    }

    private static void PrintGrid(ref Span2D<char> grid, ref Vector2I position, ref int direction)
    {
        Console.Clear();

        var c = direction switch
        {
            0 => '^',
            1 => '>',
            2 => 'v',
            3 => '<',
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

        var sb = new StringBuilder();
        for (var y = 0; y < grid.Height; y++)
        {
            for (var x = 0; x < grid.Width; x++)
            {
                if (x == position.X && y == position.Y)
                    sb.Append(c);
                else if (grid[y, x] != '#' && grid[y, x] != '.')
                    sb.Append('.');
                else
                    sb.Append(grid[y, x]);
            }

            sb.AppendLine();
        }

        Console.Write(sb);
    }
}

public record struct Vector2I(int X, int Y);
public record struct PositionDirection(Vector2I Position, int Direction);