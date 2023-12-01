abstract class d01
{
    protected List<string> lines = new List<string>();

    public void Run()
    {
        lines = File.ReadLines(@"..\..\..\inputs\01.txt").ToList();

        Count();
    }

    abstract protected void Count();
}

class d01_1 : d01
{
    protected override void Count()
    {
        var sum = 0;
        foreach (var line in lines)
        {
            var d1 = line.First(char.IsDigit) - 0x30;
            var d2 = line.Reverse().First(char.IsDigit) - 0x30;
            sum += d1 * 10 + d2;
        }
        Console.WriteLine(sum);
    }
}

class d01_2 : d01
{
    private Dictionary<string, int> words = new Dictionary<string, int>()
    {
        { "zero", 0 },
        { "one", 1 },
        { "two", 2 },
        { "three", 3 },
        { "four", 4 },
        { "five", 5 },
        { "six", 6 },
        { "seven", 7 },
        { "eight", 8 },
        { "nine", 9 },
    };

    protected override void Count()
    {
        var sum = 0;

        foreach (var line in lines)
        {
            var d1 = -1;
            var d2 = -1;
            
            for (int i = 0; i < line.Length; i++)
            {
                var c = line[i];
                if (char.IsDigit(c))
                {
                    var d = c - 0x30;
                    if (d1 == -1)
                    {
                        d1 = d;
                    }
                    d2 = d;
                }

                if (words.Keys.Any(w => line.Substring(i).StartsWith(w)))
                {
                    var d = words[words.Keys.Single(w => line.Substring(i).StartsWith(w))];
                    if (d1 == -1)
                    {
                        d1 = d;
                    }
                    d2 = d;
                }
            }

            sum += d1 * 10 + d2;
        }
        Console.WriteLine(sum);
    }
}