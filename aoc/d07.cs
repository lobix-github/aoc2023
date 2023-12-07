class d07 : baseD
{
    public void Run()
    {
        var result = count('B');
        Console.WriteLine(result); // part 1

        result = count('1', max);
        Console.WriteLine(result); // part 2
    }

    int count(char replacement, Func<string, string> transform = null)
    {
        transform ??= (s) => s;

        var list = File.ReadLines(@"..\..\..\inputs\07.txt")
            .Select(x => (transform(x.Split(' ')[0]),
                x.Split(' ')[1],
                x.Split(' ')[0]
                    .Replace('A', 'E')
                    .Replace('K', 'D')
                    .Replace('Q', 'C')
                    .Replace('J', replacement)
                    .Replace('T', 'A'))
            ).ToList();
        sort(list);

        var result = 0;
        for (int i = 0; i < list.Count; i++)
        {
            result += (i + 1) * ToInt(list[i].Item2);
        }
        return result;
    }

    string max(string val)
    {
        var highestVal = val.Replace("J", "").GroupBy(x => x).OrderByDescending(x => x.Count()).ThenByDescending(x => x.Key).Select(x => x.Key).FirstOrDefault('E');
        return val.Replace('J', highestVal);
    }

    bool isFive(string val) => val.Distinct().Count() == 1;
    bool isFour(string val) => val.GroupBy(x => x).Count() == 2 && val.GroupBy(x => x).Any(x => x.Count() == 4);
    bool isFullHouse(string val) => val.GroupBy(x => x).Count() == 2 && val.GroupBy(x => x).Any(x => x.Count() == 3);
    bool isThree(string val) => val.GroupBy(x => x).Count() == 3 && val.GroupBy(x => x).Any(x => x.Count() == 3);
    bool isTwoPair(string val) => val.GroupBy(x => x).Count() == 3 && val.GroupBy(x => x).Count(x => x.Count() == 2) == 2;
    bool isOnePair(string val) => val.Distinct().Count() == 4;
    bool isHigh(string val) => val.Distinct().Count() == 5;

    void sort(List<(string, string, string)> list)
    {
        list.Sort(new Comparison<(string, string, string)>((x, y) =>
        {
            if (isFive(x.Item1) || isFive(y.Item1))
            {
                if (isFive(x.Item1) && isFive(y.Item1)) return x.Item3.CompareTo(y.Item3);
                return isFive(x.Item1) ? 1 : -1;
            }
            if (isFour(x.Item1) || isFour(y.Item1))
            {
                if (isFour(x.Item1) && isFour(y.Item1)) return x.Item3.CompareTo(y.Item3);
                return isFour(x.Item1) ? 1 : -1;
            }
            if (isFullHouse(x.Item1) || isFullHouse(y.Item1))
            {
                if (isFullHouse(x.Item1) && isFullHouse(y.Item1)) return x.Item3.CompareTo(y.Item3);
                return isFullHouse(x.Item1) ? 1 : -1;
            }
            if (isThree(x.Item1) || isThree(y.Item1))
            {
                if (isThree(x.Item1) && isThree(y.Item1)) return x.Item3.CompareTo(y.Item3);
                return isThree(x.Item1) ? 1 : -1;
            }
            if (isTwoPair(x.Item1) || isTwoPair(y.Item1))
            {
                if (isTwoPair(x.Item1) && isTwoPair(y.Item1)) return x.Item3.CompareTo(y.Item3);
                return isTwoPair(x.Item1) ? 1 : -1;
            }
            if (isOnePair(x.Item1) || isOnePair(y.Item1))
            {
                if (isOnePair(x.Item1) && isOnePair(y.Item1)) return x.Item3.CompareTo(y.Item3);
                return isOnePair(x.Item1) ? 1 : -1;
            }
            if (isHigh(x.Item1) || isHigh(y.Item1))
            {
                if (isHigh(x.Item1) && isHigh(y.Item1)) return x.Item3.CompareTo(y.Item3);
                return isHigh(x.Item1) ? 1 : -1;
            }

            return x.Item1.CompareTo(y.Item1);
        }));
    }
}
