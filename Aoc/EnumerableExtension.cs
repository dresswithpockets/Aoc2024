public static class EnumerableExtension {
    public static IEnumerable<(T? Previous, T Current)> SelectPrevious<T>(this IEnumerable<T> source) where T : struct
    {
        T? previous = null;
        foreach (var item in source)
        {
            yield return (previous, item);
            previous = item;
        }
    }

    public static void AddSorted(this List<int> list, int value)
    {
        if (list.Count == 0)
        {
            list.Add(value);
            return;
        }

        var idx = list.BinarySearch(value);
        if (idx < 0)
            idx = ~idx;

        list.Insert(idx, value);
    }
}