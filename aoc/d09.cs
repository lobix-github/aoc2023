class d09 : baseD
{
    public void Run()
    {
        var lines = File.ReadLines(@"..\..\..\inputs\09.txt");
        var sum = 0;
        foreach (var line in lines)
        {
            var vals = line.Split(' ').Select(ToInt).ToList();
            List<int> lasts = new List<int>() { vals.Last() };
            while (!vals.All(x => x == 0))
            {
                vals = subArray(vals);
                lasts.Add(vals[vals.Count - 1]);
            }

            sum += lasts.Sum();
        }
        Console.WriteLine(sum); // part 1

        sum = 0;
        foreach (var line in lines)
        {
            var vals = line.Split(' ').Select(ToInt).ToList();
            List<int> firsts = new List<int>() { vals.First() };
            while (!vals.All(x => x == 0))
            {
                vals = subArray(vals);
                firsts.Add(vals[0]);
            }

            var val = 0;
            for (int i = firsts.Count - 2; i >= 0; i--)
            {
                val = firsts[i] - val;
            }
            sum += val;
        }
        Console.WriteLine(sum); // part 2

        List<int> subArray(List<int> vals)
        {
            List<int> list = new List<int>();
            for (int i = 1; i < vals.Count; i++)
            {
                list.Add(vals[i] - vals[i - 1]);
            }
            return list;
        }
    }
}
