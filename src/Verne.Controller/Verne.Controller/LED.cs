namespace Verne.Controller
{
    public class LED
    {
        public LED(int index, string name, bool state = false, bool enabled = true)
        {
			Index = index;
			Name = name;
			State = state;
			Enabled = enabled;
		}

        public int Index { get; set; }

        public string Name { get; set; }

        public bool State { get; set; } = false;

        public bool Enabled { get; set; } = true;
    }
}
