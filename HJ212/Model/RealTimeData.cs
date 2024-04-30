namespace HJ212.Model
{
    public class RealTimeData(string name)
    {
        public string Name { get; set; } = name;
        public string? Rtd { get; set; }
        public string? Flag { get; set; }
        public string? SampleTime { get; set; }
        public string? EFlag { get; set; }
    }
}
