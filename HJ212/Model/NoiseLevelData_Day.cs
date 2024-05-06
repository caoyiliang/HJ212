namespace HJ212.Model
{
    public class NoiseLevelData_Day(string name, string data, string dayData, string nightData)
    {
        public string Name { get; set; } = name;
        public string Data { get; set; } = data;
        public string DayData { get; set; } = dayData;
        public string NightData { get; set; } = nightData;
    }
}
