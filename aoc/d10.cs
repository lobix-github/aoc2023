using System.Collections.Generic;

class d10 : baseD
{
    public void Run()
    {
        var all = new HashSet<(int, int)>();
        var pipesD = new Dictionary<(int, int), char>();
        var diredPipesD = new Dictionary<(int, int), List<(int, int)>>();
        var pipes = new HashSet<Pipe>();
        var lines = File.ReadLines(@"..\..\..\inputs\10.txt").ToArray();
        Pipe start = default;
        for (int y = 0; y < lines.Count(); y++)
        {
            var line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                if (lines[y][x] != '.')
                {
                    var pipe = new Pipe((x, y), line[x]);
                    pipesD[pipe.pos] = pipe.dir;
                    pipes.Add(pipe);
                    if (pipe.dir == 'S') start = pipe;
                }
                all.Add((x, y));
            }
        }

        var q = new Queue<(Pipe, int, List<(int, int)>)>();
        q.Enqueue((start, 0, new List<(int, int)>()));
        var max = -1;
        var loop = new List<(int, int)>();
        while (q.Any())
        {
            var info = q.Dequeue();
            var pipe = info.Item1;
            var hist = info.Item3.ToList();
            if (pipe.dir != 'S') hist.Add(pipe.pos);

            var posL = withL(pipe.pos);
            if (!info.Item3.Contains(posL) && pipesD.TryGetValue(posL, out var dirL))
            {
                var pipeL = new Pipe(posL, dirL);
                if (dirL == 'S')
                {
                    setMax();
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
                    setMax();
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
                    setMax();
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
                    setMax();
                }
                else if (new[] { '|', 'L', 'J' }.Contains(dirD))
                {
                    q.Enqueue(new(pipeD, info.Item2 + 1, hist));
                }
            }

            void setMax()
            {
                if (info.Item2 > max)
                {
                    max = info.Item2;
                    loop = info.Item3;
                    loop.Add(pipe.pos);
                    loop.Add(start.pos);
                }
                max = Math.Max(max, info.Item2);
            }
        }
        Console.WriteLine((int)((max + 1) / 2)); // part 1

        /*
         * Part 2 idea is to trace the loop counted in part 1, and for each pipe element mark the 'inner' side, thus effectively its allowed position (x, y).
         * For corners it will be two allowed positions (x, y). Then for each tile not being a pipe try using BFS if any of elements is allowed (inner) position.
         * If yes - count the whole group and add it to the total sum of 'inners'.
         */
        pipesD[start.pos] = '7'; // handcrafted for input
        var innerDir = 'r'; // guess - either 'l' or 'r' :)

        var startIdx = loop.IndexOf(start.pos);
        for (int ii = startIdx; ii < loop.Count + startIdx; ii++)
        {
            var i = ii % loop.Count;
            var loopPos = loop[i];
            diredPipesD[loopPos] = new List<(int, int)>() { getDirPos(loopPos, innerDir) };

            if (pipesD[loopPos] == '-' || pipesD[loopPos] == '|')
            {
                diredPipesD[loopPos].Add(getDirPos(loopPos, innerDir));
                continue;
            }
            else if (pipesD[loopPos] == '7')
            {
                if      (innerDir == 'd') innerDir = 'l';
                else if (innerDir == 'l') innerDir = 'd';
                else if (innerDir == 'u') innerDir = 'r';
                else if (innerDir == 'r') innerDir = 'u';
            }
            else if (pipesD[loopPos] == 'J')
            {
                if      (innerDir == 'u') innerDir = 'l';
                else if (innerDir == 'l') innerDir = 'u';
                else if (innerDir == 'd') innerDir = 'r';
                else if (innerDir == 'r') innerDir = 'd';
            }
            else if (pipesD[loopPos] == 'L')
            {
                if      (innerDir == 'u') innerDir = 'r';
                else if (innerDir == 'r') innerDir = 'u';
                else if (innerDir == 'd') innerDir = 'l';
                else if (innerDir == 'l') innerDir = 'd';
            }
            else if (pipesD[loopPos] == 'F')
            {
                if      (innerDir == 'd') innerDir = 'r';
                else if (innerDir == 'r') innerDir = 'd';
                else if (innerDir == 'u') innerDir = 'l';
                else if (innerDir == 'l') innerDir = 'u';
            }
            diredPipesD[loopPos].Add(getDirPos(loopPos, innerDir));
        }

        var dots = new HashSet<(int, int)>(all.Except(diredPipesD.Keys));
        max = 0;
        var len = lines[0].Length;
        while (dots.Any())
        {
            var qd = new Queue<(int, int)>();
            qd.Enqueue(dots.First());
            dots.Remove(dots.First());
            var rL = false;
            var rR = false;
            var rU = false;
            var rD = false;
            int count = 0;
            while(qd.Any())
            {
                var dot = qd.Dequeue();
                count++;

                var posL = withL(dot);
                rL = processDot(posL);

                var posR = withR(dot);
                rR = processDot(posR);

                var posU = withU(dot);
                rU = processDot(posU);

                var posD = withD(dot);
                rD = processDot(posD);

                bool processDot((int, int) pos)
                {
                    var result = false;
                    if (diredPipesD.ContainsKey(pos))
                    {
                        result = diredPipesD[pos].Contains(dot);
                    }
                    else if (dots.Contains(pos))
                    {
                        qd.Enqueue(pos);
                        dots.Remove(pos);
                    }

                    return result;
                }
            }

            if (rL | rR | rU | rD) max += count;
        }
        Console.WriteLine(max); // part 2

        (int, int) withL((int, int) pos) => (pos.Item1 - 1, pos.Item2);
        (int, int) withR((int, int) pos) => (pos.Item1 + 1, pos.Item2);
        (int, int) withU((int, int) pos) => (pos.Item1, pos.Item2 - 1);
        (int, int) withD((int, int) pos) => (pos.Item1, pos.Item2 + 1);

        (int, int) getDirPos((int, int) pos, char dir) => dir switch
        {
            'l' => withL(pos),
            'r' => withR(pos),
            'u' => withU(pos),
            'd' => withD(pos),
        };
    }

    record struct Pipe((int, int) pos, char dir);
}
 