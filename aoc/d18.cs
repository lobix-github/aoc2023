using System.Drawing;

class d18 : baseD
{
    public void Run()
    {
        var lines = File.ReadLines(@"..\..\..\inputs\18.txt");

        var plain = getPlain(lines, GetDirAndStep1);
        var sum = calculateInners(plain);
        Console.WriteLine(sum + plain.Count()); // part 1

        plain = getPlain(lines, GetDirAndStep2);
        sum = calculateInners(plain);
        Console.WriteLine(sum + plain.Count()); // part 2
    }

    private (Dirs, int) GetDirAndStep1(string line) => (toDir(line.Split(' ')[0][0]), ToInt(line.Split(' ')[1]));
    private (Dirs, int) GetDirAndStep2(string line) => (toDir(line.Split(' ')[2][^2]), ToIntFromHex(line.Split(' ')[2].Trim('(', '#')[..5]));

    private HashSet<Point> getPlain(IEnumerable<string> lines, Func<string, (Dirs, int)> getDirAndSteps)
    {
        var result = new HashSet<Point>();
        var cur = new Point(0, 0);
        result.Add(cur);
        foreach (var line in lines)
        {
            (var dir, var steps) = getDirAndSteps(line);
            for (int i = 0; i < steps; i++)
            {
                cur = dir switch
                {
                    Dirs.E => cur with { X = cur.X + 1 },
                    Dirs.W => cur with { X = cur.X - 1 },
                    Dirs.N => cur with { Y = cur.Y - 1 },
                    Dirs.S => cur with { Y = cur.Y + 1 },
                };
                result.Add(cur);
            }
        }
        return result;
    }

    private Dirs toDir(char d) => d switch
    {
        'U' or '3' => Dirs.N,
        'R' or '0' => Dirs.E,
        'D' or '1' => Dirs.S,
        'L' or '2' => Dirs.W,
    }; 
}