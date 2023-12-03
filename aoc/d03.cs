﻿using System.Text.RegularExpressions;

abstract class d03 : baseD
{
    protected List<string> plane = new List<string>();
    protected HashSet<DPoint> symbols = new HashSet<DPoint>();

    public void Run()
    {
        var sum = Count();
        Console.WriteLine(sum);
    }

    abstract protected int Count();

    protected bool IsDigit(char c) => !new[] { '*', '.' }.Contains(c);
    protected bool IsOK(int x, int y, IEnumerable<DPoint> second) => new[] {
        new DPoint(x - 1, y),
        new DPoint(x + 1, y),
        new DPoint(x, y - 1),
        new DPoint(x, y + 1),
        new DPoint(x - 1, y - 1),
        new DPoint(x - 1, y + 1),
        new DPoint(x + 1, y - 1),
        new DPoint(x + 1, y + 1),
    }.Intersect(second).Any();
}

class d03_1 : d03
{
    protected override int Count()
    {
        var sum = 0;
        plane = File.ReadLines(@"..\..\..\inputs\03.txt").Select(x => Regex.Replace(x, "[^0-9.]+", x => "*")).ToList();
        var N = plane.Count;

        for (int y = 0; y < N; y++)
        {
            for (int x = 0; x < N; x++)
            {
                if (plane[y][x] == '*')
                {
                    symbols.Add(new DPoint(x, y));
                }
            }
        }

        for (int y = 0; y < N; y++)
        {
            for (int x = 0; x < N; x++)
            {
                if (IsDigit(plane[y][x]))
                {
                    var isOk = IsOK(x, y, symbols);
                    string num = string.Empty;
                    num += plane[y][x];
                    while (++x < N && IsDigit(plane[y][x]))
                    {
                        num += plane[y][x];
                        isOk |= IsOK(x, y, symbols);
                    }
                    if (isOk)
                    {
                        sum += ToInt(num);
                    }
                    x--;
                }
            }
        }

        return sum;
    }
}

class d03_2 : d03
{
    protected override int Count()
    {
        Dictionary<string, HashSet<DPoint>> numbers = new Dictionary<string, HashSet<DPoint>>();
        var sum = 0;
        plane = File.ReadLines(@"..\..\..\inputs\03.txt").Select(x => Regex.Replace(x, "[^0-9.*]+", x => ".")).ToList();
        var N = plane.Count;

        for (int y = 0; y < N; y++)
        {
            for (int x = 0; x < N; x++)
            {
                if (plane[y][x] == '*')
                {
                    symbols.Add(new DPoint(x, y));
                }
            }
        }

        for (int y = 0; y < N; y++)
        {
            for (int x = 0; x < N; x++)
            {
                if (IsDigit(plane[y][x]))
                {
                    string num = string.Empty;
                    num += plane[y][x];
                    var set = new HashSet<DPoint> { new DPoint(x, y) };
                    while (++x < N && IsDigit(plane[y][x]))
                    {
                        num += plane[y][x];
                        set.Add(new DPoint(x, y));
                    }
                    var id = $"{Guid.NewGuid()}_{num}";
                    numbers[id] = set;
                    x--;
                }
            }
        }

        foreach (var symbol in symbols)
        {
            var res = numbers.Select(num => IsOK(symbol.x, symbol.y, num.Value) ? num : new KeyValuePair<string, HashSet<DPoint>>("bad", null)).Where(x => x.Key != "bad").ToList();
            if (res.Count() == 2)
            {
                sum += ToInt(res[0].Key.Substring(res[0].Key.IndexOf('_') + 1)) * ToInt(res[1].Key.Substring(res[1].Key.IndexOf('_') + 1));
            }
        }

        return sum;
    }
}
