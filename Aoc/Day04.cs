namespace Aoc;

public class Day04 : IDiscreteSolution
{
    public string FilePath => "Inputs/04.txt";
    public string Solution1 => _solution1.ToString();
    public string Solution2 => _solution2.ToString();

    private int _solution1;
    private int _solution2;

    private char[,] _input = null!;

    public void ParseInput(ReadOnlySpan<char> input)
    {
        var inputString = new string(input);
        var lines = inputString.TrimEnd().Split("\n");

        _input = new char[lines.Length, lines[0].Length];
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i].ToCharArray();
            for (var j = 0; j < line.Length; j++)
                _input[i, j] = line[j];
        }
    }

    public void SolvePart1()
    {
        var count = 0;
        for (var y = 0; y < _input.GetLength(0); y++)
        for (var x = 0; x < _input.GetLength(1); x++)
        {
            if (_input[y, x] != 'X') continue;
            // left
            if (x >= 3 && _input[y, x - 1] == 'M' && _input[y, x - 2] == 'A' && _input[y, x - 3] == 'S') count++;
            // right
            if (x < _input.GetLength(1) - 3 && _input[y, x + 1] == 'M' && _input[y, x + 2] == 'A' && _input[y, x + 3] == 'S') count++;
            // up
            if (y >= 3 && _input[y - 1, x] == 'M' && _input[y - 2, x] == 'A' && _input[y - 3, x] == 'S') count++;
            // down
            if (y < _input.GetLength(0) - 3 && _input[y + 1, x] == 'M' && _input[y + 2, x] == 'A' && _input[y + 3, x] == 'S') count++;
            // up-left
            if (y >= 3 && x >= 3 && _input[y - 1, x - 1] == 'M' && _input[y - 2, x - 2] == 'A' && _input[y - 3, x - 3] == 'S') count++;
            // up-right
            if (y >= 3 && x < _input.GetLength(1) - 3 && _input[y - 1, x + 1] == 'M' && _input[y - 2, x + 2] == 'A' && _input[y - 3, x + 3] == 'S') count++;
            // down-left
            if (y < _input.GetLength(0) - 3 && x >= 3 && _input[y + 1, x - 1] == 'M' && _input[y + 2, x - 2] == 'A' && _input[y + 3, x - 3] == 'S') count++;
            // down-right
            if (y < _input.GetLength(0) - 3 && x < _input.GetLength(1) - 3 && _input[y + 1, x + 1] == 'M' && _input[y + 2, x + 2] == 'A' && _input[y + 3, x + 3] == 'S') count++;
        }

        _solution1 = count;
    }

    public void SolvePart2()
    {
        var count = 0;
        for (var y = 1; y < _input.GetLength(0) - 1; y++)
        for (var x = 1; x < _input.GetLength(1) - 1; x++)
        {
            if (_input[y, x] != 'A') continue;
            var topLeft = _input[y - 1, x - 1];
            var topRight = _input[y - 1, x + 1];
            var bottomLeft = _input[y + 1, x - 1];
            var bottomRight = _input[y + 1, x + 1];
            var currentCount = 0;
            if (topLeft == 'M' && bottomRight == 'S') currentCount++;
            if (topRight == 'M' && bottomLeft == 'S') currentCount++;
            if (bottomLeft == 'M' && topRight == 'S') currentCount++;
            if (bottomRight == 'M' && topLeft == 'S') currentCount++;
            if (currentCount == 2) count++;
        }

        _solution2 = count;
    }
}