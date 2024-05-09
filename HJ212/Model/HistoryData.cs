namespace HJ212.Model
{
    public class HistoryData(DateTime dataTime, List<StatisticsData> data)
    {
        public DateTime DataTime { get; set; } = dataTime;
        public List<StatisticsData> Data { get; set; } = data;
    }
}
