using System.Drawing;

class d16 : baseD
{
    char[][] plane = File.ReadLines(@"..\..\..\inputs\16.txt").Select(x => x.ToCharArray()).ToArray();

    public void Run()
    {
        var sum = RunSimulation(new D16Point(-1, 0, Dirs.E));
        Console.WriteLine(sum); // part 1

        var max1 = Enumerable.Range(0, plane.Length - 1).Select(y => RunSimulation(new D16Point(-1, y, Dirs.E))).Max();
        var max2 = Enumerable.Range(0, plane.Length - 1).Select(y => RunSimulation(new D16Point(plane[0].Length, y, Dirs.W))).Max();
        var max3 = Enumerable.Range(0, plane[0].Length - 1).Select(x => RunSimulation(new D16Point(x, -1, Dirs.S))).Max();
        var max4 = Enumerable.Range(0, plane[0].Length - 1).Select(x => RunSimulation(new D16Point(x, plane.Length, Dirs.N))).Max();
        Console.WriteLine(new[] { max1, max2, max3, max4 }.Max()); // part 2
    }

    public int RunSimulation(D16Point start)
    {
        var seen = new HashSet<D16Point>();
        var curs = new Queue<D16Point>();
        curs.Enqueue(start);

        while (curs.Any())
        {
            var cur = curs.Dequeue();
            if (!seen.Contains(cur))
            {
                seen.Add(cur);
                foreach (var next in nextPos(cur))
                {
                    curs.Enqueue(next);
                }
            }
        }

        return seen.Select(x => new Point(x.x, x.y)).Distinct().Count() - 1;
    }

    IEnumerable<D16Point> nextPos(D16Point point)
    {
        var next = point.dir switch
        {
            Dirs.N => point with { y = point.y - 1 },
            Dirs.E => point with { x = point.x + 1 },
            Dirs.S => point with { y = point.y + 1 },
            Dirs.W => point with { x = point.x - 1 },
        };

        if (next.x >= 0 && next.x < plane[0].Length && next.y >= 0 && next.y < plane.Length)
        {
            var p = plane[next.y][next.x];
            if (p == '.') yield return next;
            else if (next.dir == Dirs.N || next.dir == Dirs.S)
            {
                if (p == '|') yield return next;
                else if (p == '-')
                {
                    yield return next with { dir = Dirs.W }; ;
                    yield return next with { dir = Dirs.E }; ;
                }
                else
                {
                    // must be either / or \
                    yield return next with { dir = getReflectedDir(next.dir, p) };
                }
            }
            else if (next.dir == Dirs.E || next.dir == Dirs.W)
            {
                if (p == '-') yield return next;
                else if (p == '|')
                {
                    yield return next with { dir = Dirs.N }; ;
                    yield return next with { dir = Dirs.S }; ;
                }
                else
                {
                    // must be either / or \
                    yield return next with { dir = getReflectedDir(next.dir, p) };
                }
            }
        }
    }

    Dirs getReflectedDir(Dirs dir, char c)
    {
        return dir switch
        {
            Dirs.N => c == '/' ? Dirs.E : Dirs.W,
            Dirs.S => c == '/' ? Dirs.W : Dirs.E,
            Dirs.E => c == '/' ? Dirs.N : Dirs.S,
            Dirs.W => c == '/' ? Dirs.S : Dirs.N,
        };
    }
}

public record struct D16Point(int x, int y, Dirs dir);

