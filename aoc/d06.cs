class d06 : baseD
{
    public void Run()
    {
        var lines = File.ReadLines(@"..\..\..\inputs\06.txt").Select(x => x.Split(':')[1]).ToArray();
        var times = new List<int>(lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ToInt));
        var dists = new List<int>(lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ToInt));

        long result = 1;
        long wins = 0;
        for (int i = 0; i < times.Count; i++)
        {
            wins = 0;
            for (int t = 0; t < times[i]; t++) 
            {
                var dist = t * (times[i] - t);
                if (dist > dists[i]) wins++;
            }
            result *= wins;
        }
        Console.WriteLine(result); // part 1

        wins = 0;
        var bigTime = ToBigInt(lines[0].Replace(" ", ""));
        var bigDist = ToBigInt(lines[1].Replace(" ", ""));

        for (int t = 0; t < bigTime; t++)
        {
            var dist2 = t * (bigTime - t);
            if (dist2 > bigDist) wins++;
        }
        Console.WriteLine(wins); // part 2

        wins = 0;
        for (var t = 0; t < bigTime; t++)
        {
            var dist2 = t * (bigTime - t);
            if (dist2 > bigDist) break;
            wins++;
        }
        for (var t = bigTime - 1; t >= 0; t--)
        {
            var dist2 = t * (bigTime - t);
            if (dist2 > bigDist) break;
            wins++;
        }
        Console.WriteLine(bigTime - wins); // part 2
    }
}
