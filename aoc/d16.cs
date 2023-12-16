class d16 : baseD
{
    char[][] plane = File.ReadLines(@"..\..\..\inputs\16.txt").Select(x => x.ToCharArray()).ToArray();
    
    public void Run()
    {
        var seen = new HashSet<D16Point>();
        var start = new D16Point(-1, 0, Dirs.E);
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

        var sum = seen.Select(x => new DPoint(x.x, x.y)).Distinct().Count() - 1;
        Console.WriteLine(sum); // part 1
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

