using System.IO;
using System.Text;

class d14 : baseD
{
    public void Run()
    {
        var lines = File.ReadLines(@"..\..\..\inputs\14.txt").ToList();

        moveU(lines);
        var sum = getSum(lines);
        Console.WriteLine(sum); // part 1

        lines = File.ReadLines(@"..\..\..\inputs\14.txt").ToList();
        loopCycle(1_000_000_000, () =>
        {
            doCycle();
            return getPoints(lines).GetHash();
        });
        sum = getSum(lines);
        Console.WriteLine(sum); // part 2

        void doCycle()
        {
            moveU(lines);
            moveL(lines);
            moveD(lines);
            moveR(lines);
        }

        void moveU(List<string> lines)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                var col = lines.Select(l => l[x]).ToArray();
                var freeSpots = new List<int>();
                for (int y = 0; y < col.Length; y++)
                {
                    if (col[y] == '.')
                    {
                        freeSpots.Add(y);
                    }
                    else if (col[y] == '#')
                    {
                        freeSpots.Clear();
                    }
                    else if (col[y] == 'O')
                    {
                        if (freeSpots.Any())
                        {
                            var sb = new StringBuilder(new string(col));
                            sb[y] = '.';
                            sb[freeSpots[0]] = 'O';
                            freeSpots.RemoveAt(0);
                            freeSpots.Add(y);
                            col = sb.ToString().ToCharArray();
                        }
                    }
                }
                for (int y = 0; y < col.Length; y++)
                {
                    var sb = new StringBuilder(lines[y]);
                    sb[x] = col[y];
                    lines[y] = sb.ToString();
                }
            }
        }

        void moveD(List<string> lines)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                var col = lines.Select(l => l[x]).ToArray();
                var freeSpots = new List<int>();
                for (int y = col.Length - 1; y >= 0; y--)
                {
                    if (col[y] == '.')
                    {
                        freeSpots.Add(y);
                    }
                    else if (col[y] == '#')
                    {
                        freeSpots.Clear();
                    }
                    else if (col[y] == 'O')
                    {
                        if (freeSpots.Any())
                        {
                            var sb = new StringBuilder(new string(col));
                            sb[y] = '.';
                            sb[freeSpots[0]] = 'O';
                            freeSpots.RemoveAt(0);
                            freeSpots.Add(y);
                            col = sb.ToString().ToCharArray();
                        }
                    }
                }
                for (int y = 0; y < col.Length; y++)
                {
                    var sb = new StringBuilder(lines[y]);
                    sb[x] = col[y];
                    lines[y] = sb.ToString();
                }
            }
        }

        void moveL(List<string> lines)
        {
            for (int y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                var freeSpots = new List<int>();
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '.')
                    {
                        freeSpots.Add(i);
                    }
                    else if (line[i] == '#')
                    {
                        freeSpots.Clear();
                    }
                    else if (line[i] == 'O')
                    {
                        if (freeSpots.Any())
                        {
                            var sb = new StringBuilder(line);
                            sb[i] = '.';
                            sb[freeSpots[0]] = 'O';
                            freeSpots.RemoveAt(0);
                            freeSpots.Add(i);
                            line = sb.ToString();
                        }
                    }
                }
                lines[y] = line;
            }
        }

        void moveR(List<string> lines)
        {
            for (int y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                var freeSpots = new List<int>();
                for (int i = line.Length - 1; i >= 0 ; i--)
                {
                    if (line[i] == '.')
                    {
                        freeSpots.Add(i);
                    }
                    else if (line[i] == '#')
                    {
                        freeSpots.Clear();
                    }
                    else if (line[i] == 'O')
                    {
                        if (freeSpots.Any())
                        {
                            var sb = new StringBuilder(line);
                            sb[i] = '.';
                            sb[freeSpots[0]] = 'O';
                            freeSpots.RemoveAt(0);
                            freeSpots.Add(i);
                            line = sb.ToString();
                        }
                    }
                }
                lines[y] = line;
            }
        }

        HashSet<DPoint> getPoints(List<string> lines)
        {
            var result = new HashSet<DPoint>();
            for (int y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == 'O')
                    {
                        result.Add(new DPoint(x, y));
                    }
                }
            }
            return result;
        }

        int getSum(List<string> lines)
        {
            int sum = 0;
            for (var i = 0; i < lines.Count; i++)
            {
                sum += lines[i].Count(c => c == 'O') * (lines.Count - i);
            }
            return sum;
        }
    }
}

public static class ExtsD14
{
    public static int GetHash(this HashSet<DPoint> points)
    {
        var hash = 0;
        foreach (var point in points)
        {
            hash ^= point.GetHashCode();
        }
        return hash;
    }
}