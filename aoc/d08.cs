class d08 : baseD
{
    public void Run()
    {
        var lines = File.ReadLines(@"..\..\..\inputs\08.txt")
            .Select(x => x.Replace("=", ""))
            .Select(x => x.Replace("(", ""))
            .Select(x => x.Replace(",", ""))
            .Select(x => x.Replace(")", ""))
            .ToArray();

        var nodes = lines[2..]
            .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToDictionary(x => x[0], x => (x[1], x[2]));

        long count = trace(nodes["AAA"], x => x == "ZZZ");
        Console.WriteLine(count); // part 1

        var keysA = nodes.Keys.Where(k => k.EndsWith('A'));
        var paths = new List<long>();
        foreach (var keyA in keysA)
        {
            count = trace(nodes[keyA], x => x.EndsWith('Z'));
            paths.Add(count);
        }
        count = Numerics.Lcm(paths);
        Console.WriteLine(count); // part 2

        long trace((string, string) node, Func<string, bool> stop)
        {
            long count = 0;
            while (true)
            {
                var next = lines[0][(int)count++ % lines[0].Length] == 'L' ? node.Item1 : node.Item2;
                if (stop(next)) break;
                node = nodes[next];
            }
            return count;
        }
    }
}
