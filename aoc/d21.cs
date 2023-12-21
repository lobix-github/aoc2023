using System.Drawing;

class d21 : baseD
{
    public void Run()
    {
        var plain = new HashSet<Point>();

        Point start = default;
        var lines = File.ReadLines(@"..\..\..\inputs\21.txt").ToArray();
        for (int y = 0; y < lines.Count(); y++)
        {
            var line = lines[y];
            for (int x = 0; x < lines.Length; x++)
            {
                if (line[x] == '#') plain.Add(new Point(x, y));
                if (line[x] == 'S') start = new Point(x, y);
            }
        }

        var states = new Dictionary<int, HashSet<Point>>() { { 0, new HashSet<Point>() { start } } };
        for (int i = 1; i <= 64; i++)
        {
            states[i] = new HashSet<Point>();
            foreach (var p in states[i - 1])
            {
                var newP = p with { X = p.X + 1 };
                if (!plain.Contains(newP)) states[i].Add(newP);
                newP = p with { X = p.X - 1 };
                if (!plain.Contains(newP)) states[i].Add(newP);
                newP = p with { Y = p.Y + 1 };
                if (!plain.Contains(newP)) states[i].Add(newP);
                newP = p with { Y = p.Y - 1 };
                if (!plain.Contains(newP)) states[i].Add(newP);
            }
        }

        Console.WriteLine(states[64].Count); // part 1
    }
}
