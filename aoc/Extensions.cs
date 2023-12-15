public static class DExts
{
    public static void AddReplace<T>(this List<T> list, T value, Predicate<T> match)
    {
        var pos = list.FindIndex(match);
        if (pos != -1)
            list[pos] = value;
        else
            list.Add(value);
    }

    public static void RemoveIfExists<T>(this List<T> list, Predicate<T> match)
    {
        var pos = list.FindIndex(match);
        if (pos != -1)
            list.RemoveAt(pos);
    }
}