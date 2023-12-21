class d20 : baseD
{
    public static Dictionary<string, Module> modules = new Dictionary<string, Module>();
    public static Queue<Action> actions = new Queue<Action>();
    public static Dictionary<bool, long> pulses = new Dictionary<bool, long>() { { false, 0 }, { true, 0 } };

    public void Run()
    {
        var allEndPoints = new HashSet<string>();
        var lines = File.ReadLines(@"..\..\..\inputs\20.txt");
        foreach (var line in lines)
        {
            var endPoints = line.Split(' ')[2..].Select(s => s.TrimEnd(',')).ToList();
            endPoints.ForEach(p => allEndPoints.Add(p));

            if (new[] { '%', '&' }.Contains(line[0]))
            {
                var name = line.Split(' ')[0][1..];
                Module module = line[0] == '%' ? new FlipFlop(name, endPoints) : new Conjunction(name, endPoints);
                modules.Add(name, module);
            }
            else
            {
                modules.Add("broadcaster", new Broadcaster("broadcaster", endPoints));
            }
        }
        foreach (var module in modules.Values.OfType<Conjunction>())
        {
            var dict = modules.Where(m => m.Value.EndPoints.Contains(module.Name)).ToDictionary(x => x.Key, x => false);
            module.LastReceived = dict;
        }
        foreach (var endPoint in allEndPoints.Except(modules.Keys))
        {
            modules.Add(endPoint, new Output(endPoint, null));
        }

        for (var i = 0; i < 1000; i++)
        {
            actions.Enqueue(() => modules["broadcaster"].Run(null, false));
            while (actions.Any())
            {
                var action = actions.Dequeue();
                action();
            }
        }

        Console.WriteLine(pulses[false] * pulses[true]); // part 1
    }

    public abstract class Module
    {
        public string Name { get; }
        public List<string> EndPoints { get; }

        public Module(string name, List<string> endPoints)
        {
            Name = name;
            EndPoints = endPoints;
        }

        protected void CountPulse(bool high) => pulses[high]++;

        public abstract void Run(string from, bool high);
    }

    public class Broadcaster : Module
    {
        public Broadcaster(string name, List<string> endPoints) : base(name, endPoints) { }

        public override void Run(string from, bool high)
        {
            CountPulse(high);
            actions.Enqueue(() => EndPoints.ForEach(ep => modules[ep].Run(Name, high)));
        }
    }

    public class FlipFlop : Module
    {
        public FlipFlop(string name, List<string> endPoints) : base(name, endPoints) { }

        public override void Run(string from, bool high)
        {
            CountPulse(high);
            actions.Enqueue(() =>
            {
                if (!high)
                {
                    state = !state;
                    EndPoints.ForEach(ep => modules[ep].Run(Name, state));
                }
            });
        }

        private bool state = false;
    }

    public class Conjunction : Module
    {
        public Dictionary<string, bool> LastReceived { get; set; }

        public Conjunction(string name, List<string> endPoints) : base(name, endPoints) { }

        public override void Run(string from, bool high)
        {
            CountPulse(high);
            actions.Enqueue(() =>
            {
                LastReceived[from] = high;
                EndPoints.ForEach(ep => modules[ep].Run(Name, !LastReceived.Values.All(x => x)));
            });
        }
    }

    public class Output : Module
    {
        public Output(string name, List<string> endPoints) : base(name, endPoints) { }

        public override void Run(string from, bool high) => CountPulse(high);
    }
}