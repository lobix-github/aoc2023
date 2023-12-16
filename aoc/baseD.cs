using System.Numerics;

abstract class baseD
{
    protected int ToInt(string val) => Convert.ToInt32(val);
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

    protected void loopCycle<T>(int count, Func<T> jobReturningCacheKey)
    {
        var cache = new Dictionary<T, int>();
        var cycle = 1;
        while (cycle <= count)
        {
            var id = jobReturningCacheKey();

            if (cache.TryGetValue(id, out var cached))
            {
                var remaining = count - cycle - 1;
                var loop = cycle - cached;

                var loopRemaining = remaining % loop;
                cycle = count - loopRemaining - 1;
            }

            cache[id] = cycle++;
        }
    }
}

public record struct DPoint(int x, int y);

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

public enum Dirs
{
    N = 0,
    E = 1,
    S = 2,
    W = 3
}