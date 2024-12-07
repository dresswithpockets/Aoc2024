using System.Diagnostics;
using System.Reflection;
using Spectre.Console;

namespace Aoc;

public static class Solver {
    public class Configuration {
        public List<Assembly> ProblemAssemblies { get; } = [];
        public bool ClearConsole { get; } = true;
        public string? ElapsedTimeFormatSpecifier { get; set; }
        public VerticalOverflow VerticalOverflow { get; set; } = VerticalOverflow.Ellipsis;
        public VerticalOverflowCropping VerticalOverflowCropping { get; set; } = VerticalOverflowCropping.Top;
        public bool ShowTotalElapsedTimePerDay { get; set; } = true;
        public List<ISolution> Solutions { get; } = [];

        public void AddSolution<T>() where T : ISolution, new() => Solutions.Add(new T());
    }

    private static readonly bool IsInteractiveEnvironment = Environment.UserInteractive && !Console.IsOutputRedirected;

    public static Configuration PopulateConfiguration(Action<Configuration>? configure) {
        var configuration = new Configuration();
        configure?.Invoke(configuration);
        if (configuration.ProblemAssemblies.Count == 0)
            configuration.ProblemAssemblies.Add(Assembly.GetEntryAssembly()!);
        return configuration;
    }
    
    // public static IEnumerable<Type> QuerySolutions<T>(Configuration configuration)
    //     => configuration.ProblemAssemblies
    //         .SelectMany(a => a.GetTypes()
    //             .Where(t => t.GetInterfaces().Contains(typeof(T))))
    //         .OrderBy(t => t.FullName);
    
    private static Table GetTable()
    {
        return new Table()
            .AddColumns(
                "[bold]Day[/]",
                "[bold]Part[/]",
                "[bold]Solution[/]",
                "[bold]Elapsed time[/]")
            .RoundedBorder()
            .BorderColor(Color.Grey);
    }

    public static void SolveAll(Action<Configuration>? configure = null) {
        var configuration = PopulateConfiguration(configure);

        if (IsInteractiveEnvironment && configuration.ClearConsole)
        {
            AnsiConsole.Clear();
        }

        var table = GetTable();

        // var allSolutionTypes = QuerySolutions<ISimpleSolution>(configuration)
        //     .Union(QuerySolutions<IDiscreteSolution>(configuration));

        var allSolutionInstances = configuration.Solutions;
        
        AnsiConsole.Live(table)
            .AutoClear(false)
            .Overflow(configuration.VerticalOverflow)
            .Cropping(configuration.VerticalOverflowCropping)
            .Start(ctx =>
            {
                var stopwatch = new Stopwatch();
                foreach (var instance in allSolutionInstances)
                {
                    switch (instance)
                    {
                        case ISimpleSolution simpleSolution:
                        {
                            var input = File.ReadAllText(simpleSolution.FilePath);

                            //RuntimeHelpers.PrepareMethod(simpleSolution.GetType().GetMethod("Solve")!.MethodHandle);
                            
                            stopwatch.Start();
                            simpleSolution.Solve(input);
                            stopwatch.Stop();

                            var elapsedTime = CalculateMilliseconds(stopwatch);
                        
                            SolveProblemSimple(simpleSolution, table, elapsedTime, configuration);
                            ctx.Refresh();
                            break;
                        }
                        case IDiscreteSolution discreteSolution:
                        {
                            var input = File.ReadAllText(discreteSolution.FilePath);

                            stopwatch.Start();
                            discreteSolution.ParseInput(input);
                            stopwatch.Stop();

                            var parseTime = CalculateMilliseconds(stopwatch);

                            stopwatch.Restart();
                            discreteSolution.SolvePart1();
                            stopwatch.Stop();

                            var part1Time = CalculateMilliseconds(stopwatch);

                            stopwatch.Restart();
                            discreteSolution.SolvePart2();
                            stopwatch.Stop();

                            var part2Time = CalculateMilliseconds(stopwatch);
                        
                            SolveProblemDiscrete(discreteSolution, table, parseTime, part1Time, part2Time, configuration);
                            ctx.Refresh();
                            break;
                        }
                    }
                    stopwatch.Reset();
                }
            });
    }
    
    public static void SolveLast(Action<Configuration>? configure = null) {
        var configuration = PopulateConfiguration(configure);

        if (IsInteractiveEnvironment && configuration.ClearConsole)
        {
            AnsiConsole.Clear();
        }

        var table = GetTable();

        // var allSolutionTypes = QuerySolutions<ISimpleSolution>(configuration)
        //     .Union(QuerySolutions<IDiscreteSolution>(configuration));

        // var lastSolutionType = allSolutionTypes.Last();
        var instance = configuration.Solutions.Last();
        
        AnsiConsole.Live(table)
            .AutoClear(false)
            .Overflow(configuration.VerticalOverflow)
            .Cropping(configuration.VerticalOverflowCropping)
            .Start(ctx =>
            {
                var stopwatch = new Stopwatch();
                // var instance = Activator.CreateInstance(lastSolutionType);
                switch (instance)
                {
                    case ISimpleSolution simpleSolution:
                    {
                        var input = File.ReadAllText(simpleSolution.FilePath);

                        //RuntimeHelpers.PrepareMethod(simpleSolution.GetType().GetMethod("Solve")!.MethodHandle);
                        
                        stopwatch.Start();
                        simpleSolution.Solve(input);
                        stopwatch.Stop();

                        var elapsedTime = CalculateMilliseconds(stopwatch);
                    
                        SolveProblemSimple(simpleSolution, table, elapsedTime, configuration);
                        ctx.Refresh();
                        break;
                    }
                    case IDiscreteSolution discreteSolution:
                    {
                        var input = File.ReadAllText(discreteSolution.FilePath);

                        stopwatch.Start();
                        discreteSolution.ParseInput(input);
                        stopwatch.Stop();

                        var parseTime = CalculateMilliseconds(stopwatch);

                        stopwatch.Restart();
                        discreteSolution.SolvePart1();
                        stopwatch.Stop();

                        var part1Time = CalculateMilliseconds(stopwatch);

                        stopwatch.Restart();
                        discreteSolution.SolvePart2();
                        stopwatch.Stop();

                        var part2Time = CalculateMilliseconds(stopwatch);
                    
                        SolveProblemDiscrete(discreteSolution, table, parseTime, part1Time, part2Time, configuration);
                        ctx.Refresh();
                        break;
                    }
                }
            });
    }

    private static void SolveProblemSimple(
        ISimpleSolution solution,
        Table table,
        decimal elapsedMs,
        Configuration configuration)
    {
        var problemTitle = solution.GetType().Name;
        
        RenderRow(table, $"[bold]{problemTitle}[/]", "Part 1", solution.Solution1, null, configuration);
        RenderRow(table, "", "Part 2", solution.Solution2, null, configuration);
        RenderRow(table, "", "[bold]Total[/]", "", elapsedMs, configuration);
        
        table.AddEmptyRow();
    }
    
    private static void RenderRow(Table table, string problemTitle, string part, string solution, decimal? elapsedMilliseconds, Configuration configuration)
    {
        if (elapsedMilliseconds is null)
        {
            table.AddRow(problemTitle, part, solution.EscapeMarkup(), "");
            return;
        }
        var formattedTime = FormatTime(elapsedMilliseconds.Value, configuration);
        table.AddRow(problemTitle, part, solution.EscapeMarkup(), formattedTime);
    }

    private static void SolveProblemDiscrete(
        IDiscreteSolution solution,
        Table table,
        decimal parseTime,
        decimal part1Time,
        decimal part2Time,
        Configuration configuration)
    {
        var problemTitle = solution.GetType().Name;
        
        // render parse time
        RenderRow(table, $"[bold]{problemTitle}[/]", $"Parse", "", parseTime, configuration);
        
        // render part 1 time
        RenderRow(table, "", "Part 1", solution.Solution1, part1Time, configuration);
        
        // render part 2 time
        RenderRow(table, "", "Part 2", solution.Solution2, part2Time, configuration);
        
        if (configuration.ShowTotalElapsedTimePerDay)
        {
            RenderRow(table, "", "[bold]Total[/]", "", parseTime + part1Time + part2Time, configuration);
        }
        
        table.AddEmptyRow();
    }

    private static decimal CalculateMilliseconds(Stopwatch stopwatch)
        => 1000m * stopwatch.ElapsedTicks / Stopwatch.Frequency;
    
    private static string FormatTime(decimal elapsedMilliseconds, Configuration configuration, bool useColor = true)
    {
        var customFormatSpecifier = configuration.ElapsedTimeFormatSpecifier;

        var message = customFormatSpecifier is null
            ? elapsedMilliseconds switch
            {
                < 1 => $"{elapsedMilliseconds:F3} ms",
                < 1_000 => $"{elapsedMilliseconds:F3} ms",
                < 60_000 => $"{0.001m * elapsedMilliseconds:F} s",
                _ => $"{Math.Floor(elapsedMilliseconds / 60_000)} min {Math.Round(0.001m * (elapsedMilliseconds % 60_000))} s",
            }
            : elapsedMilliseconds switch
            {
                < 1 => $"{elapsedMilliseconds.ToString(customFormatSpecifier)} ms",
                < 1_000 => $"{elapsedMilliseconds.ToString(customFormatSpecifier)} ms",
                < 60_000 => $"{(0.001m * elapsedMilliseconds).ToString(customFormatSpecifier)} s",
                _ => $"{elapsedMilliseconds / 60_000} min {(0.001m * (elapsedMilliseconds % 60_000)).ToString(customFormatSpecifier)} s",
            };

        if (useColor)
        {
            var color = elapsedMilliseconds switch
            {
                < 1 => Color.Blue,
                < 10 => Color.Green1,
                < 100 => Color.Lime,
                < 500 => Color.GreenYellow,
                < 1_000 => Color.Yellow1,
                < 10_000 => Color.OrangeRed1,
                _ => Color.Red1
            };

            return $"[{color}]{message}[/]";
        }
        else
        {
            return message;
        }
    }
}
