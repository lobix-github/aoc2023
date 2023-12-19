class d19 : baseD
{
    public void Run()
    {
        var flows = new Dictionary<string, List<string>>();
        var parts = new List<Part>();
        var lines = File.ReadLines(@"..\..\..\inputs\19.txt");
        bool readParts = false;
        foreach (var line in lines)
        {
            if (line == "")
            {
                readParts = true;
                continue;
            }
            if (!readParts)
            {
                var name = line.Split('{')[0];
                var rules = line.Split('{')[1].TrimEnd('}').Split(',').ToList();
                flows[name] = rules;
            }
            else
            {
                var partDef = line.Trim('{', '}').Split(',').ToList();
                parts.Add(new Part(ToInt(partDef[0].Split('=')[1]), ToInt(partDef[1].Split('=')[1]), ToInt(partDef[2].Split('=')[1]), ToInt(partDef[3].Split('=')[1])));
            }
        }

        var sum = 0;
        var start = flows["in"];
        foreach (var part in parts)
        {
            var res = process(part, start);
            if (res)
            {
                sum += part.sum();
            }
        }
        Console.WriteLine(sum); // part 1

        bool process(Part part, List<string> rules)
        {
            foreach (var rule in rules)
            {
                if (rule == "A") return true;
                if (rule == "R") return false;
                if (rule.Contains(':'))
                {
                    var ruleDef = rule.Split(':');
                    if (part.eval(ruleDef[0]))
                    {
                        if (ruleDef[1] == "A") return true;
                        if (ruleDef[1] == "R") return false;
                        return process(part, flows[ruleDef[1]]);
                    }
                }
                else
                {
                    return process(part, flows[rule]);
                }
            }

            throw new Exception("expected not to reach here");
        }
    }
}

public record struct Part(int x, int m, int a, int s)
{
    public bool eval(string cond)
    {
        if (cond[0] == 'x')
        {
            return cond[1] == '>' ? x > int.Parse(cond[2..]) : x < int.Parse(cond[2..]);
        }
        else if (cond[0] == 'm')
        {
            return cond[1] == '>' ? m > int.Parse(cond[2..]) : m < int.Parse(cond[2..]);
        }
        else if (cond[0] == 'a')
        {
            return cond[1] == '>' ? a > int.Parse(cond[2..]) : a < int.Parse(cond[2..]);
        }
        else
        {
            return cond[1] == '>' ? s > int.Parse(cond[2..]) : s < int.Parse(cond[2..]);
        }
    }

    public int sum() => x + m + a + s;
}