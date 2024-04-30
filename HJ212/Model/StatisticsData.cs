namespace HJ212.Model
{
    public class StatisticsData(string name)
    {
        public string Name { get; set; } = name;
        public string? Cou { get; set; }
        public string? Min { get; set; }
        public string? Avg { get; set; }
        public string? Max { get; set; }
        public string? Flag { get; set; }
    }
}
