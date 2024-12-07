namespace Aoc;

public class Dya05VSpiritov : ISimpleSolution
{
    public string FilePath => "Inputs/05.txt";
    public string Solution1 => _solution1.ToString();
    public string Solution2 => _solution2.ToString();
    private int _solution1;
    private int _solution2;

    public void Solve(ReadOnlySpan<char> input)
    {
        var stringInput = new string(input);
        var lines = stringInput.Split('\n').ToList();
        PopulateDictionary(lines);
        Part1();
        Part2();
    }
 
    private readonly List<List<int>> updates = new(100); //list of int update lists
    private Dictionary<int, List<int>> rules = new();
    private readonly List<List<int>> outOfOrderUpdates = new();

    public void PopulateDictionary(List<string> input)
    {
        bool sortedRules = false;
        foreach (string s in input)
        {
            if (!sortedRules) //do rules
            {
                if (s == "")
                {
                    sortedRules = true;
                }
                else
                {
                    int keyBuffer = int.Parse(s.Substring(0, 2));
                    int ValueBuffer = int.Parse(s.Substring(3, 2));

                    if (!rules.TryGetValue(keyBuffer, out var ints)) //no pair
                    {
                        rules[keyBuffer] = new List<int> { ValueBuffer };

                    }
                    else
                    {
                        rules[keyBuffer].Add(ValueBuffer);
                    }

                }
            }
            else //do updates
            {
                string[] updateString = s.Split(',');
                List<int> updateInts = updateString.Select(int.Parse).ToList();
                updates.Add(updateInts);
            }

        }
    }

    public void Part1()
    {
        int sum = 0;
        foreach (List<int> ints in updates)
        {
            bool isOrdered = true;

            for (int i = 1; i < ints.Count; i++) //nothing is before the first key
            {
                for (int j = 0; j < i; j++) //only look at keys to the left
                {
                    if (rules.TryGetValue(ints[i], out List<int> values))
                    {
                        values = rules[ints[i]];
                        foreach (int k in values)
                        {
                            if (ints[j] == k)
                            {
                                isOrdered = false;
                                int pageBuffer = ints[i];
                                ints[i] = ints[j];
                                ints[j] = pageBuffer;
                                outOfOrderUpdates.Add(ints);
                            }
                        }
                    }
                }
            }
            if (isOrdered)
            {
                sum += ints[ints.Count / 2];
            }
            else
            {
                outOfOrderUpdates.Add(ints);
            }
        }
        Console.WriteLine(sum);
    }

    public void Part2()
    {
        int sum = 0;
        foreach (List<int> ints in outOfOrderUpdates.Distinct()) //duplicates of last ints item are in this list
        {
            sum += ints[ints.Count / 2];
        }
        Console.WriteLine(sum);
    }
}