class d04 : baseD
{
    public void Run()
    {
        var sum = 0;
        var lines = File.ReadLines(@"..\..\..\inputs\04.txt").Select(x => x.Split(':')[1]).ToArray();
        var cards = new List<int>(Enumerable.Repeat(1, lines.Count()));
        for (int i = 0; i < lines.Count(); i++)
        {
            var wins = new HashSet<int>(lines[i].Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ToInt));
            var nums = new HashSet<int>(lines[i].Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ToInt));
            var count = wins.Intersect(nums).Count();
            sum += (int)Math.Pow(2, count - 1);
            Enumerable.Range(1, count).ToList().ForEach(x => cards[i + x] += cards[i]);
        }
        Console.WriteLine(sum); // part 1
        Console.WriteLine(cards.Sum()); // part 2
    }
}
