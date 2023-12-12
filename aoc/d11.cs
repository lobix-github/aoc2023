class d11 : baseD
{
    public void Run()
    {
        var plain = File.ReadLines(@"..\..\..\inputs\11.txt").ToList();
        var rows = new List<int>();
        var cols = new List<int>();

        for (int y = 0; y < plain.Count; y++)
        {
            var row = plain[y];
            if (row.Select(x => x).All(x => x == '.'))
            {
                rows.Add(y);
            }
        }
        for (int x = 0; x < plain[0].Count(); x++)
        {
            var col = plain.Select(row => row[x]).ToList();
            if (col.All(x => x == '.'))
            {
                cols.Add(x);
            }
        }

        var galaxies = getGalaxies(2);
        Console.WriteLine(count(galaxies.ToArray())); // part 1
        galaxies = getGalaxies(1000000);
        Console.WriteLine(count(galaxies.ToArray())); // part 2

        IEnumerable<Galaxy> getGalaxies(int shift)
        {
            for (int y = 0; y < plain.Count; y++)
            {
                var row = plain[y];
                for (int x = 0; x < row.Length; x++)
                {
                    if (row[x] == '#')
                    {
                        var xs = cols.Where(c => c <= x);
                        var x1 = x + xs.Count() * (shift - 1);

                        var ys = rows.Where(r => r <= y);
                        var y1 = y + ys.Count() * (shift - 1);

                        yield return new Galaxy(x1, y1);
                    }
                }
            }
        }

        long count(Galaxy[] galaxies)
        {
            long sum = 0;
            for (int i = 0; i < galaxies.Count(); i++)
            {
                for (int j = i + 1; j < galaxies.Count(); j++)
                {
                    sum += Math.Abs(galaxies[i].x - galaxies[j].x) + Math.Abs(galaxies[i].y - galaxies[j].y);
                }
            }
            return sum;
        }
    }

    record struct Galaxy(int x, int y);
}
