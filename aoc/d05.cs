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
        seeds.ForEach(seed => mins.Add(Count(seed, 1, false).First()));
        Console.WriteLine(mins.Min()); // part 1

        mins = new List<long>();
        for (int i = 0; i < seeds.Count / 2; i++)
        {
            var start = seeds[i * 2];
            var count = seeds[i * 2 + 1];
            var transformedSeeds = Count(start, count, true);
            mins.Add(transformedSeeds.Min());
        }
        Console.WriteLine(mins.Min()); // part 2
    }

    IEnumerable<long> Count(long start, long count, bool doAdvance)
    {
        for (long i = start; i < start + count; i++)
        {
            var seed = i;
            long advance = long.MaxValue;
            foreach (var transformer in transformers)
            {
                (seed, advance) = transformer.Transform(seed, advance);
            }
            yield return seed;

            if (doAdvance)
            {
                i += advance;
            }
        };
    }
}

class Transformer
{
    List<TrInfo> infos = new List<TrInfo>();

    public void Add(TrInfo info) => infos.Add(info);

    public (long, long) Transform(long seed, long advance)
    {
        var info = infos.FirstOrDefault(info => info.IsInRange(seed));
        if (info != default)
        {
            return info.Transform(seed, advance);
        }
        else 
        {
            var adv = infos.OrderBy(i => i.source).FirstOrDefault(i => i.source > seed, new TrInfo(-1, long.MaxValue, -1)).source;
            return (seed, Math.Min(advance, adv)); 
        }
    }
}

record struct TrInfo(long dest, long source, long len)
{
    public bool IsInRange(long seed) => seed >= source && seed < source + len;
    public (long, long) Transform(long seed, long advance) => (seed + (dest - source), Math.Min(source + len - seed, advance));
}
