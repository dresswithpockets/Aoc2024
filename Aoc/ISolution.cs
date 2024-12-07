namespace Aoc;

public interface ISolution
{
    string FilePath { get; }
    string Solution1 { get; }
    string Solution2 { get; }
}

interface ISimpleSolution : ISolution {

    void Solve(ReadOnlySpan<char> input);
}

interface IDiscreteSolution : ISolution {

    void ParseInput(ReadOnlySpan<char> input);

    void SolvePart1();

    void SolvePart2();
}