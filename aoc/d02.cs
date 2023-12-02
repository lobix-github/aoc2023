using aoc;

 class d02 : baseD
{
    protected Dictionary<string, int> cubes = new Dictionary<string, int>() { { "blue", 14 }, { "red", 12 }, { "green", 13 }, };

    public void Run()
    {
        var lines = File.ReadLines(@"..\..\..\inputs\02.txt").ToList();
        var sum = 0;
        var powerSum = 0;
        foreach (var line in lines)
        {
            var words = line.Split(' ').Select(x => x.TrimEnd(':')).ToArray();
            int id = ToInt(words[1]);
            var sets = line.Substring(line.IndexOf(':') + 1).Split(';');
            var ok = true;
            var game = new Dictionary<string, int>() { { "blue", 0 }, { "red", 0 }, { "green", 0 }, };
            foreach (var set in sets)
            {
                var infos = set.Split(",").Select(x => x.Trim());
                foreach (var info in infos)
                {
                    var count = ToInt(info.Split(" ")[0]);
                    var color = info.Split(" ")[1];
                    if (count > cubes[color])
                    {
                        ok = false;
                    }
                    game[color] = Math.Max(game[color], count);
                }
            }
            if (ok) sum += id;
            var power = 1;
            game.Values.ToList().ForEach(v => power *= v);
            powerSum += power;
        }

        Console.WriteLine(sum);
        Console.WriteLine(powerSum);
    }
}
