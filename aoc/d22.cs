class d22 : baseD
{
    public void Run()
    {
        var plain = new HashSet<Vector3D>();

        var lines = File.ReadLines(@"..\..\..\inputs\22.txt").ToArray();
        foreach (var line in lines)
        {
            var xyzs = line.Split('~', ',');
            var start = new Point3D(ToInt(xyzs[0]), ToInt(xyzs[1]), ToInt(xyzs[2]));
            var end = new Point3D(ToInt(xyzs[3]), ToInt(xyzs[4]), ToInt(xyzs[5]));
            plain.Add(new Vector3D(start, end));
        }

        var zs = plain.Select(x => x.start.z).Distinct().OrderBy(x => x).ToArray();
        for (int z = 0; z < zs.Count(); z++)
        {
            var slice = plain.GroupBy(v => v.start.z).OrderBy(x => x.Key).Where(x => x.Key == zs[z]).Single();
            foreach (var cur in slice)
            {
                var belows = plain
                                .Where(v => v.end.z < cur.start.z)
                                .Where(v => v.IsIntersectingXY(cur))
                                .GroupBy(v => v.end.z)
                                .OrderByDescending(g => g.Key)
                                .FirstOrDefault();

                plain.Remove(cur);
                if (belows == default)
                {
                    plain.Add(cur.SetZ(1));
                }
                else if (cur.start.z - belows.First().end.z == 1)
                {
                    plain.Add(cur);
                }
                else
                {
                    plain.Add(cur.SetZ(belows.First().end.z + 1));
                }
            }
        }

        var singles = new HashSet<Vector3D>();
        foreach (var cur in plain)
        {
            var belows = plain.Where(v => cur.start.z - v.end.z == 1).Where(v => v.IsIntersectingXY(cur));

            if (belows?.Count() == 1)
            {
                singles.Add(belows.Single());
            }
        }
        
        Console.WriteLine(plain.Count - singles.Count); // part 1
    }
}

