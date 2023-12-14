class d14 : baseD
{
    public void Run()
    {
        long sum = 0;
        var lines = rotate(File.ReadLines(@"..\..\..\inputs\14.txt").ToList());
        foreach (var line in lines)
        {
            var counter = line.Length;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == 'O')
                    sum += counter--;
                else if (line[i] == '#')
                    counter = line.Length - i - 1;
            }
        }
        Console.WriteLine(sum); // part 1
    }
}
