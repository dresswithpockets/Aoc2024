
namespace Aoc;

public class Day01Fast : ISimpleSolution
{
    public string FilePath => "Inputs/Day01.txt";

    public string Solution1 => _solution1.ToString();

    public string Solution2 => _solution2.ToString();

    private int _solution1;
    private int _solution2;

    public void Solve(ReadOnlySpan<char> input)
    {
        var leftNumbers = new List<int>();
        var rightNumbers = new List<int>();
        var counts = new Dictionary<int, int>();
        int left;
        int right;
        for (int idx = 0; idx < input.Length; idx++)
        {
            var c = input[idx];
            // left number
            left = 0;
            while (c != ' ')
            {
                left *= 10;
                left += c - '0';

                c = input[++idx];
            }

            // spaces
            while (c == ' ')
                c = input[++idx];
            
            // right number
            right = 0;
            while (c != '\n')
            {
                right *= 10;
                right += c - '0';
                
                idx++;
                if (idx == input.Length)
                    break;

                c = input[idx];
            }

            if (!counts.TryGetValue(right, out var rightCount))
                counts[right] = 0;
            counts[right] = rightCount + 1;

            leftNumbers.AddSorted(left);
            rightNumbers.AddSorted(right);
        }

        var sumDifference = 0;
        var similarity = 0;
        for (var idx = 0; idx < leftNumbers.Count; idx++)
        {
            var leftNumber = leftNumbers[idx];
            sumDifference += Math.Abs(leftNumber - rightNumbers[idx]);
            similarity += counts.TryGetValue(leftNumber, out var value) ? leftNumber * value : 0;
        }

        _solution1 = sumDifference;
        _solution2 = similarity;
    }
}