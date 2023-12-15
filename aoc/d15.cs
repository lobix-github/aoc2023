class d15 : baseD
{
    public void Run()
    {
        var lines = string.Join(Environment.NewLine, File.ReadLines(@"..\..\..\inputs\15.txt")).Split(',');

        var sum = lines.Select(getHash).Sum();
        Console.WriteLine(sum); // part 1

        int getHash(string val) => val.Aggregate(0, (hash, c) => hash = ((hash + c) * 17) % 256);
    }
}
