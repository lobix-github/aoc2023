class d13 : baseD
{
    public void Run()
    {
        var lines = File.ReadLines(@"..\..\..\inputs\13.txt").ToList();
        lines.Add(string.Empty);
        var localPattern = new List<string>();
        var sum = 0;
        var groupIdx = 0;
        foreach (var line in lines)
        {
            if (line == "")
            {
                groupIdx++;
                var col = processPattern(localPattern);
                sum += col;
                if (col == 0)
                {
                    col = 100 * processPattern(rotate(localPattern));
                    sum += col;
                }
                localPattern.Clear();
                continue;
            }
            localPattern.Add(line);
        }
        Console.WriteLine(sum); // part 1

        int processPattern(List<string> pattern)
        {
            var col = 0;
            var match = false;
            var line0 = pattern[0];
            for (int idx = 1; idx < line0.Length; idx++)
            {
                var left = line0.Substring(0, idx);
                var right = line0.Substring(idx);
                match = compare(left, new string(right.Reverse().ToArray()));
                if (match)
                {
                    foreach (var line in pattern.Skip(1))
                    {
                        left = line.Substring(0, idx);
                        right = line.Substring(idx);
                        match &= compare(left, new string(right.Reverse().ToArray()));
                    }
                    if (match)
                    {
                        col = idx;
                        break;
                    }
                }
            }
            if (!match)
            {
                col = 0;
            }
            return col;
        }

        bool compare(string left, string right)
        {
            if (left.Length <= right.Length)
            {
                return right.EndsWith(left);
            }
            else
            {
                return left.EndsWith(right);
            }
        }
    }
}
