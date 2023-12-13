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
}

record struct DPoint(int x, int y);
