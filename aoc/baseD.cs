abstract class baseD
{
    protected int ToInt(string val) => Convert.ToInt32(val);
    protected long ToLong(string val) => Convert.ToInt64(val);
}

record struct DPoint(int x, int y);
