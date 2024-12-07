namespace Aoc;

public class Day05V3 : ISimpleSolution
{
    public string FilePath => "Inputs/05.txt";
    public string Solution1 => _solution1.ToString();
    public string Solution2 => _solution2.ToString();

    private int _solution1;
    private int _solution2;

    public void Solve(ReadOnlySpan<char> input)
    {
        var rules = new HashSet<(int Left, int Right)>();

        var inputIdx = 0;
        while (input[inputIdx] != '\n')
        {
            var left = int.Parse(input.Slice(inputIdx, 2));
            var right = int.Parse(input.Slice(inputIdx + 3, 2));
            inputIdx += 6;
            rules.Add((left, right));
        }

        inputIdx += 1;

        var correctSum = 0;
        var incorrectSum = 0;
        while (inputIdx < input.Length)
        {
            var page = new List<int>(32);
            while (input[inputIdx] != '\n')
            {
                page.Add(int.Parse(input.Slice(inputIdx, 2)));
                inputIdx += 2;
                if (inputIdx >= input.Length)
                    break;
                if (input[inputIdx] == ',')
                    inputIdx += 1;
            }

            inputIdx += 1;

            var correct = true;
            var middleIfCorrect = page[page.Count / 2];
            for (var idx = 0; idx < page.Count; idx++)
            for (var jdx = 0; jdx < idx; jdx++)
            {
                var pair = (page[idx], page[jdx]);
                if (!rules.Contains(pair)) continue;

                (page[jdx], page[idx]) = (page[idx], page[jdx]);
                correct = false;
            }

            if (correct)
                correctSum += middleIfCorrect;
            else
                incorrectSum += page[page.Count / 2];
        }

        _solution1 = correctSum;
        _solution2 = incorrectSum;
    }
}