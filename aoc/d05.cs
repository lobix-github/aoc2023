class d05 : baseD
{
    List<Transformer> transformers = new List<Transformer>();

    public void Run()
    {
        var lines = File.ReadLines(@"..\..\..\inputs\05.txt").ToArray();
        var seeds = lines[0].Split(new char[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(ToLong).ToList();
        for (int i = 2; i < lines.Length; i++)
        {
            var line = lines[i];
            var transformer = new Transformer();
            while (line != "") 
            {
                if (line.Contains(':'))
                {
                    transformers.Add(transformer);
                }
                else
                {
                    var info = line.Split(' ').Select(ToLong).ToArray();
                    transformer.Add(new TrInfo(info[0], info[1], info[2]));
                }
                if (++i < lines.Length)
                    line = lines[i];
                else 
                    break;
            }
        }

        var mins = new List<long>();
        Count(seeds, mins);
        Console.WriteLine(mins.Min()); // part 1

        mins = new List<long>();
        for (int i = 0; i < seeds.Count / 2; i++)
        {
            var moreSeeds = new List<long>();
            var transformedSeeds = new List<long>();
            var start = seeds[i * 2];
            var count = seeds[i * 2 + 1];
            for (long j = start; j < start + count; j++)
            {
                moreSeeds.Add(j);
            }
            Count(moreSeeds, transformedSeeds, $"iteration {i} of {seeds.Count / 2 - 1}, ");
            mins.Add(transformedSeeds.Min());
        }
        Console.WriteLine(mins.Min()); // part 2
    }

    void Count(IEnumerable<long> seeds, IList<long> collection, string prefix = "")
    {
        long idx = 0;
        long len = seeds.Count(); 
        seeds.AsParallel().ForAll(seed =>
        {
            idx++;
            if (idx % 1_000_000 == 0)
            {
                Console.WriteLine($"{prefix}idx {idx} of {len} ({(int)(((double)idx / 1000) / ((double)len / 1000) * 100)}%)");
            }

            foreach (var transformer in transformers)
            {
                seed = transformer.Transform(seed);
            }
            lock (this)
            {
                collection.Add(seed);
            }
        });
    }
}

class Transformer
{
    List<TrInfo> infos = new List<TrInfo>();

    public void Add(TrInfo info) => infos.Add(info);

    public long Transform(long seed)
    {
        var info = infos.FirstOrDefault(info => info.IsInRange(seed));
        return info != default ? info.Transform(seed) : seed;
    }
}

record struct TrInfo(long dest, long source, long len)
{
    public bool IsInRange(long seed) => seed >= source && seed < source + len;
    public long Transform(long seed) => seed + (dest - source);
}
