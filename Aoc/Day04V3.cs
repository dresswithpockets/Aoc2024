using System.Diagnostics;
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;

namespace Aoc;


public class Day04V3 : ISimpleSolution
{
    public string FilePath => "Inputs/04.txt";
    public string Solution1 => _solution1.ToString();
    public string Solution2 => _solution2.ToString();

    private int _solution1;
    private int _solution2;

    public unsafe void Solve(ReadOnlySpan<char> input)
    {
        if (input.IsEmpty)
            return;

        var lineLength = 0;
        while (lineLength < input.Length && input[lineLength] != '\n')
            lineLength++;
        lineLength++;

        var lineCount = input.Length / lineLength;
        var rotation0 = input.AsSpan2D(0, lineCount, lineLength - 1, 1);
        // var rotation90Array = stackalloc char[input.Length];
        // var rotation90 = new Span2D<char>(rotation90Array, lineCount, lineLength, 1);
        // var rotation180Array = stackalloc char[input.Length];
        // var rotation180 = new Span2D<char>(rotation180Array, lineCount, lineLength, 1);
        // var rotation270Array = stackalloc char[input.Length];
        // var rotation270 = new Span2D<char>(rotation270Array, lineCount, lineLength, 1);
        var rotation90 = new Memory2D<char>(new char[lineCount, lineLength - 1]);
        var rotation180 = new Memory2D<char>(new char[lineCount, lineLength - 1]);
        var rotation270 = new Memory2D<char>(new char[lineCount, lineLength - 1]);
        
        for (var x = 0; x < rotation0.Width; x++)
        for (var y = 0; y < rotation0.Height; y++)
        {
            var newX = y;
            var newY = rotation0.Height - (x + 1);
            rotation90.Span[newX, newY] = rotation0[x, y];

            var prevX = newX;
            newX = newY;
            newY = rotation0.Height - (prevX + 1);
            rotation180.Span[newX, newY] = rotation0[x, y];

            prevX = newX;
            newX = newY;
            newY = rotation0.Height - (prevX + 1);
            rotation270.Span[newX, newY] = rotation0[x, y];
        }

        var part1Count = 0;
        var part2Count = 0;
        for (var x = 0; x < rotation0.Width; x++)
        for (var y = 0; y < rotation0.Height; y++)
        {
            switch (rotation0[x, y])
            {
                case 'X':
                    // left
                    if (x >= 3 && rotation0[x - 1, y] == 'M' && rotation0[x - 2, y] == 'A' &&
                        rotation0[x - 3, y] == 'S') part1Count++;
                    // up-left
                    if (y >= 3 && x >= 3 && rotation0[x - 1, y - 1] == 'M' && rotation0[x - 2, y - 2] == 'A' &&
                        rotation0[x - 3, y - 3] == 'S') part1Count++;
                    break;
                case 'A' when x > 0 && x < rotation0.Width - 1 && y > 0 && y < rotation0.Height - 1:
                    var topLeft = rotation0[x - 1, y - 1];
                    var topRight = rotation0[x + 1, y - 1];
                    var bottomLeft = rotation0[x - 1, y + 1];
                    var bottomRight = rotation0[x + 1, y + 1];
                    var currentCount = 0;
                    if (topLeft == 'M' && bottomRight == 'S') currentCount++;
                    if (topRight == 'M' && bottomLeft == 'S') currentCount++;
                    if (bottomLeft == 'M' && topRight == 'S') currentCount++;
                    if (bottomRight == 'M' && topLeft == 'S') currentCount++;
                    if (currentCount == 2) part2Count++;
                    break;
            }
        }

        {
            var span = rotation90.Span;
            for (var x = 0; x < span.Width; x++)
            for (var y = 0; y < span.Height; y++)
            {
                if (span[x, y] != 'X') continue;
                // left
                if (x >= 3 && span[x - 1, y] == 'M' && span[x - 2, y] == 'A' &&
                    span[x - 3, y] == 'S') part1Count++;
                // up-left
                if (y >= 3 && x >= 3 && span[x - 1, y - 1] == 'M' && span[x - 2, y - 2] == 'A' &&
                    span[x - 3, y - 3] == 'S') part1Count++;
                break;
            }
        }
        
        {
            var span = rotation180.Span;
            for (var x = 0; x < span.Width; x++)
            for (var y = 0; y < span.Height; y++)
            {
                if (span[x, y] != 'X') continue;
                // left
                if (x >= 3 && span[x - 1, y] == 'M' && span[x - 2, y] == 'A' &&
                    span[x - 3, y] == 'S') part1Count++;
                // up-left
                if (y >= 3 && x >= 3 && span[x - 1, y - 1] == 'M' && span[x - 2, y - 2] == 'A' &&
                    span[x - 3, y - 3] == 'S') part1Count++;
                break;
            }
        }
        
        {
            var span = rotation270.Span;
            for (var x = 0; x < span.Width; x++)
            for (var y = 0; y < span.Height; y++)
            {
                if (span[x, y] != 'X') continue;
                // left
                if (x >= 3 && span[x - 1, y] == 'M' && span[x - 2, y] == 'A' &&
                    span[x - 3, y] == 'S') part1Count++;
                // up-left
                if (y >= 3 && x >= 3 && span[x - 1, y - 1] == 'M' && span[x - 2, y - 2] == 'A' &&
                    span[x - 3, y - 3] == 'S') part1Count++;
                break;
            }
        }

        _solution1 = part1Count;
        _solution2 = part2Count;
    }
}