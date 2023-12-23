class d22 : baseD
{
    public void Run()
    {
        var plain = new CopyableHashedHashSet<Vector3D>();

        var lines = File.ReadLines(@"..\..\..\inputs\22.txt").ToArray();
        foreach (var line in lines)
        {
            var xyzs = line.Split('~', ',');
            var start = new Point3D(ToInt(xyzs[0]), ToInt(xyzs[1]), ToInt(xyzs[2]));
            var end = new Point3D(ToInt(xyzs[3]), ToInt(xyzs[4]), ToInt(xyzs[5]));
            plain.Add(new Vector3D(start, end));
        }

        var abovesD = new Dictionary<Vector3D, HashSet<Vector3D>>();
        var belowsD = new Dictionary<Vector3D, HashSet<Vector3D>>();
        Fall(plain);

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

        long sum = 0;
        foreach (var s in singles)
        {
            var vanished = new HashSet<Vector3D>() { s };
            var q = new Queue<Vector3D>();
            q.Enqueue(s);
            while (q.TryDequeue(out var cur))
            {
                foreach (var above in abovesD[cur])
                {
                    if (belowsD[above].Except(vanished).Count() == 0)
                    {
                        vanished.Add(above);
                        q.Enqueue(above);
                    }
                }
            }
            sum += vanished.Count();
        }

        //var idx = 0;
        //foreach (var v in singles)
        //{
        //    var plain2 = plain.Copy();
        //    plain2.Remove(v);
        //    Fall(plain2);
        //    sum += plain2.Count - plain.Intersect(plain2).Count();
        //    Console.WriteLine($"{idx++}/{singles.Count}");
        //}
        Console.WriteLine(sum - singles.Count); // part 2

        void Fall(CopyableHashedHashSet<Vector3D> hs)
        {
            var zs = hs.Select(x => x.start.z).Distinct().OrderBy(x => x).ToArray();
            for (int z = 0; z < zs.Count(); z++)
            {
                var slice = hs.GroupBy(v => v.start.z).Where(x => x.Key == zs[z]).Single();
                foreach (var cur in slice)
                {
                    var belows = hs
                                    .Where(v => v.end.z < cur.start.z)
                                    .Where(v => v.IsIntersectingXY(cur))
                                    .GroupBy(v => v.end.z)
                                    .OrderByDescending(g => g.Key)
                                    .FirstOrDefault();

                    Vector3D newV = default;
                    hs.Remove(cur);
                    if (belows == default)
                    {
                        newV = cur.SetZ(1);
                    }
                    else if (cur.start.z - belows.First().end.z == 1)
                    {
                        newV = cur;
                    }
                    else
                    {
                        newV = cur.SetZ(belows.First().end.z + 1);
                    }
                    hs.Add(newV);
                    abovesD[newV] = new HashSet<Vector3D>();
                    belowsD[newV] = new HashSet<Vector3D>();
 
                    var bs = belows ?? Enumerable.Empty<Vector3D>();
                    foreach (var below in bs)
                    {
                        abovesD[below].Add(newV);
                        belowsD[newV].Add(below);
                    }
                }
            }
        }
    }
}

