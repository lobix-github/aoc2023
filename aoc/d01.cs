abstract class d01
{
    protected abstract string inputFile { get; }
    protected List<string> lines = new List<string>();

    public void Run()
    {
        lines = File.ReadLines(inputFile).ToList();

        Count();
    }

    abstract protected void Count();
}

class d01_1 : d01
{
    protected override string inputFile => @"..\..\..\inputs\01_1.txt";
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
    protected override string inputFile => @"..\..\..\inputs\01_2.txt";
  
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
            var d1 = words[words.Keys.First(w => line.IndexOf(w) > -1)];
            var d2 = words[words.Keys.First(w => line.LastIndexOf(w) > -1)];
            sum += d1 * 10 + d2;
        }
        Console.WriteLine(sum);
    }
}