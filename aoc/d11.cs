class d11 : baseD
{
    public void Run()
    {
        var plain = File.ReadLines(@"..\..\..\inputs\11.txt").ToList();
        var galaxies = new List<Galaxy>();

        for (int y = 0; y < plain.Count; y++)
        {
            var row = plain[y];
            if (row.Select(x => x).All(x => x == '.'))
            {
                plain.Insert(y, row);
                y++;
            }
        }
        for (int x = 0; x < plain[0].Count(); x++)
        {
            var col = plain.Select(row => row[x]).ToList();
            if (col.All(x => x == '.'))
            {
                for (int y = 0; y < plain.Count; y++)
                {
                    plain[y] = plain[y].Insert(x, ".");
                }
                x++;
            }
        }

        for (int y = 0; y < plain.Count; y++)
        {
            var row = plain[y];
            for (int x = 0; x < row.Length; x++)
            {
                if (row[x] == '#')
                {
                    galaxies.Add(new Galaxy(x, y));
                }
            }
        }

        var sum = 0;
        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = i + 1; j < galaxies.Count; j++)
            {
                sum += Math.Abs(galaxies[i].x - galaxies[j].x) + Math.Abs(galaxies[i].y - galaxies[j].y);
            }
        }
        Console.WriteLine(sum); // part 1
    }

    record struct Galaxy(int x, int y);
}
