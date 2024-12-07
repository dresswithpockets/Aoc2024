namespace Aoc;

public class Day05 : ISimpleSolution
{
    public string FilePath => "Inputs/05.txt";
    public string Solution1 => _solution1.ToString();
    public string Solution2 => _solution2.ToString();

    private int _solution1;
    private int _solution2;

    public void Solve(ReadOnlySpan<char> input)
    {
        var inputString = new string(input);
        var lines = inputString.Split('\n');
        
        var ruleStrings = lines.TakeWhile(l => !string.IsNullOrWhiteSpace(l)).ToList();
        var pageStrings = lines.Skip(ruleStrings.Count + 1);

        var rules = ruleStrings.Select(r =>
        {
            var parts = r.Split('|');
            var left = int.Parse(parts[0]);
            var right = int.Parse(parts[1]);
            return (left, right);
        }).ToHashSet();
        
        var correctSum = 0;
        var incorrectSum = 0;
        foreach (var page in pageStrings.Select(r => r.Split(',').Select(int.Parse).ToList()))
        {
            var correct = true;
            var middleIfCorrect = page[page.Count / 2];
            for (var idx = 0; idx < page.Count; idx++)
            for (var jdx = 0; jdx < page.Count; jdx++)
            {
                if (idx == jdx) continue;
                
                var pair = (page[idx], page[jdx]);
                if (!rules.Contains(pair) || idx <= jdx) continue;
                
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