using System.Drawing;
using System.Linq;

class d23 : baseD
{
    public void Run()
    {

        var plain = File.ReadLines(@"..\..\..\inputs\23.txt").SelectMany((line, y) => line.Select((c, x) => new D23Point(new DPoint(x, y), c)).ToArray()).ToDictionary(x => x.xy, v => v.c);
        var start = plain.Single(x => x.Key.Y == 0 && x.Value == '.').Key;
        var dists = new Dictionary<DPoint, int>();
        var hist = new Dictionary<DPoint, HashSet<DPoint>>();
        var maxY = plain.Keys.Select(x => x.Y).Max();

        var max = 0;
        var q = new Queue<(DPoint, int, CopyableHashedHashSet<DPoint>)>();
        q.Enqueue((start, 0, new CopyableHashedHashSet<DPoint>()));
        while(q.Any())
        {
            (var cur, var dist, var seen) = q.Dequeue();
            if (seen.Contains(cur)) continue;
            seen.Add(cur);
            if (cur.Y == maxY)
            {
                max = Math.Max(max, dist);
            }

            var paths = dirs(cur, seen);
            var r = withAll(cur).Where(x => plain.ContainsKey(x) && plain[x] != '#');
            var r2 = r.Except(seen);
            if (paths.Count() == 1)
            {
                q.Enqueue((paths[0], dist + 1, seen));
            }
            else
            {
                var d = dists.ContainsKey(cur) ? dists[cur] : 0;
                if (dist > d)
                {
                    dists[cur] = dist;
                    hist[cur] = seen.Copy();

                    foreach (var p in paths)
                    {
                        q.Enqueue((p, dists[cur] + 1, seen.Copy()));
                    }
                }
            }
        }

        Console.WriteLine(max); // part 1

        DPoint[] dirs(DPoint p, HashSet<DPoint> seen) => plain[p] switch
        {
            '.' => withAll(p).Where(x => plain.ContainsKey(x) && plain[x] != '#').Except(seen).ToArray(),
            '>' => new[] { withE(p) },
            '<' => new[] { withW(p) },
            '^' => new[] { withN(p) },
            'v' => new[] { withS(p) },
        };

        DPoint withE(DPoint p) => p with { X = p.X + 1 };
        DPoint withW(DPoint p) => p with { X = p.X - 1 };
        DPoint withS(DPoint p) => p with { Y = p.Y + 1 };
        DPoint withN(DPoint p) => p with { Y = p.Y - 1 };
        DPoint[] withAll(DPoint p) => new[] { withE(p), withW(p), withS(p), withN(p) };
    }
}

public record D23Point(DPoint xy, char c);

