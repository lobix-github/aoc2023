class d04 : baseD
{
    public void Run()
    {
        var sum = 0;
        var lines = File.ReadLines(@"..\..\..\inputs\04.txt").Select(x => x.Split(':')[1]);
        var list = new List<List<(HashSet<int>, HashSet<int>)>>();
        foreach (var card in lines)
        {
            var wins = new HashSet<int>(card.Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ToInt));
            var nums = new HashSet<int>(card.Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ToInt));
            sum += (int)Math.Pow(2, wins.Intersect(nums).Count() - 1);

            list.Add(new List<(HashSet<int>, HashSet<int>)>() { (wins, nums) });
        }
        Console.WriteLine(sum); // part 1

        for (int i = 0; i < list.Count; i++)
        {
            var cards = list[i];
            foreach (var (wins, nums) in cards)
            {
                var count = wins.Intersect(nums).Count();
                for (int j = 1; j <= count; j++)
                {
                    list[i + j].Add(list[i + j][0]);
                }
            }
        }
        sum = list.Select(x => x.Count()).Sum();
        Console.WriteLine(sum); // part 2
    }
}
