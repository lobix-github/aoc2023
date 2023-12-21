using System.Drawing;
using System.Numerics;

abstract class baseD
{
    protected int ToInt(string val) => Convert.ToInt32(val);
    protected int ToIntFromHex(string val) => Convert.ToInt32(val, 16);
    protected long ToLong(string val) => Convert.ToInt64(val);
    protected BigInteger ToBigInt(string val) => BigInteger.Parse(val);

    protected List<string> rotate(List<string> list)
    {
        var result = new List<string>(list[0].Length);
        result.AddRange(Enumerable.Repeat(string.Empty, list[0].Length));
        for (int y = 0; y < list.Count; y++)
        {
            var line = list[y];
            for (int x = 0; x < line.Length; x++)
            {
                result[x] += line[x];
            }
        }

        return result;
    }

    protected void loopCycle<T>(int count, Func<int, T> jobReturningCacheKey)
    {
        var cache = new Dictionary<T, int>();
        var cycle = 1;
        while (cycle <= count)
        {
            var id = jobReturningCacheKey(cycle);

            if (cache.TryGetValue(id, out var cached))
            {
                var remaining = count - cycle;
                var loop = cycle - cached;

                var loopRemaining = remaining % loop;
                cycle = count - loopRemaining;
            }

            cache[id] = cycle++;
        }
    }

    protected long calculateInners(HashSet<Point> plain)
    {
        var minX = plain.Select(x => x.X).Min();
        var minY = plain.Select(x => x.Y).Min();
        var maxX = plain.Select(x => x.X).Max();
        var maxY = plain.Select(x => x.Y).Max();

        var gr = plain.GroupBy(x => x.Y).OrderBy(g => g.Key).ToArray();
        long sum = 0;
        if (maxY - minY + 1 != gr.Count()) throw new Exception("sie zesralo");
        for (var i = 0; i < gr.Count(); i++)
        {
            var g = gr[i];
            var inner = true;
            var xs = g.OrderBy(p => p.X).ToArray();
            if (xs.Length > 1)
            {
                int? firstXInSeries = null;
                for (var idx = 1; idx < xs.Length; idx++)
                {
                    var prev = xs[idx - 1].X;
                    var cur = xs[idx].X;

                    if (prev == cur - 1)
                    {
                        if (firstXInSeries == null) firstXInSeries = prev;
                        continue;
                    }

                    if (firstXInSeries != null)
                    {
                        var differentDir = plain.Contains(new Point(firstXInSeries.Value, g.Key + 1)) ^ plain.Contains(new Point(prev, g.Key + 1));
                        if (!differentDir) inner = !inner;
                        firstXInSeries = null;
                    }

                    if (inner)
                    {
                        sum += cur - prev - 1;
                    }
                    inner = !inner;
                }
            }
        }
        return sum;
    }
}

public class DCache<TKey, TValue>
{
    private readonly Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();
    public TValue Get(TKey key, Func<TValue> getValue)
    {
        if (dict.TryGetValue(key, out var value))
        {
            return value;
        }
        
        value = getValue();
        dict[key] = value;
        return value;
    }
}

public class HashedHashSet<T> : HashSet<T>
{
    public override int GetHashCode()
    {
        var hash = 0;
        foreach (var item in this)
        {
            hash ^= item.GetHashCode();
        }
        return hash;
    }
}

public enum Dirs
{
    N = 0,
    E = 1,
    S = 2,
    W = 3
}