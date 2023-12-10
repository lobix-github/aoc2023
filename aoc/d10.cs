class d10 : baseD
{
    public void Run()
    {
        var pipesD = new Dictionary<(int, int), char>();
        var pipes = new HashSet<Pipe>();
        var visited = new Dictionary<Pipe, HashSet<Pipe>>();
        var lines = File.ReadLines(@"..\..\..\inputs\10.txt").ToArray();
        Pipe start = default;
        for (int y = 0; y < lines.Count(); y++)
        {
            var line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                var pipe = new Pipe((x, y), line[x]);
                if (pipe.dir == '.') continue;
                pipesD[pipe.pos] = pipe.dir;
                pipes.Add(pipe);
                if (pipe.dir == 'S') start = pipe;
            }
        }

        var q = new Queue<(Pipe, int, HashSet<(int, int)>)>();
        q.Enqueue((start, 0, new HashSet<(int, int)>()));
        var max = -1;
        while(q.Any())
        {
            //Console.WriteLine(q.Count());
            var info = q.Dequeue();
            var pipe = info.Item1;
            var hist = info.Item3.ToHashSet();
            if (pipe.dir != 'S') hist.Add(pipe.pos);

            var posL = withL(pipe.pos);
            if (!info.Item3.Contains(posL) && pipesD.TryGetValue(posL, out var dirL))
            {
                var pipeL = new Pipe(posL, dirL);
                if (dirL == 'S')
                {
                    max = Math.Max(max, info.Item2);
                }
                else if (new[] { '-', 'L', 'F' }.Contains(dirL))
                {
                    q.Enqueue(new(pipeL, info.Item2 + 1, hist));
                }
            }

            var posR = withR(pipe.pos);
            if (!info.Item3.Contains(posR) && pipesD.TryGetValue(posR, out var dirR))
            {
                var pipeR = new Pipe(posR, dirR);
                if (dirR == 'S')
                {
                    max = Math.Max(max, info.Item2);
                }
                else if (new[] { '-', 'J', '7' }.Contains(dirR))
                {
                    q.Enqueue(new(pipeR, info.Item2 + 1, hist));
                }
            }

            var posU = withU(pipe.pos);
            if (!info.Item3.Contains(posU) && pipesD.TryGetValue(posU, out var dirU))
            {
                var pipeU = new Pipe(posU, dirU);
                if (dirU == 'S')
                {
                    max = Math.Max(max, info.Item2);
                }
                else if (new[] { '|', '7', 'F' }.Contains(dirU))
                {
                    q.Enqueue(new(pipeU, info.Item2 + 1, hist));
                }
            }

            var posD = withD(pipe.pos);
            if (!info.Item3.Contains(posD) && pipesD.TryGetValue(posD, out var dirD))
            {
                var pipeD = new Pipe(posD, dirD);
                if (dirD == 'S')
                {
                    max = Math.Max(max, info.Item2);
                }
                else if (new[] { '|', 'L', 'J' }.Contains(dirD))
                {
                    q.Enqueue(new(pipeD, info.Item2 + 1, hist));
                }
            }
        }
        Console.WriteLine((int)((max + 1) / 2)); // part 1

        (int, int) withL((int, int) pos) => (pos.Item1 - 1, pos.Item2);
        (int, int) withR((int, int) pos) => (pos.Item1 + 1, pos.Item2);
        (int, int) withU((int, int) pos) => (pos.Item1, pos.Item2 - 1);
        (int, int) withD((int, int) pos) => (pos.Item1, pos.Item2 + 1);
    }

    record struct Pipe((int, int) pos, char dir);
}
 