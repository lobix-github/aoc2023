class d15 : baseD
{
    public void Run()
    {
        var lines = string.Join(Environment.NewLine, File.ReadLines(@"..\..\..\inputs\15.txt")).Split(',');

        var sum = lines.Select(getHash).Sum();
        Console.WriteLine(sum); // part 1

        var cache = new DCache<string, int>();
        var boxes = new List<List<(string, int)>>();
        for (int i = 0; i < 256; i++) boxes.Add(new List<(string, int)>());
        foreach (var line in lines)
        {
            var minus = line.Last() == '-';
            var label = minus ? line[..^1] : line.Split('=')[0];
            var idx = cache.Get(label, () => getHash(label));

            if (minus)
            {
                boxes[idx].RemoveIfExists(x => x.Item1 == label);
            }
            else
            {
                var lens = ToInt(line.Split('=')[1]);
                boxes[idx].AddReplace((label, lens), x => x.Item1 == label);
            }
        }
        sum = boxes.Select((box, idx) => box.Select((x, i) => (idx + 1) * (i + 1) * x.Item2).Sum()).Sum();
        Console.WriteLine(sum); // part 2

        int getHash(string val) => val.Aggregate(0, (hash, c) => hash = ((hash + c) * 17) % 256);
    }
}
