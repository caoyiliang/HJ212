namespace HJ212.Model
{
    /// <summary>
    /// 历史数据
    /// </summary>
    /// <param name="dataTime">时间</param>
    /// <param name="data">统计数据</param>
    public class HistoryData(DateTime dataTime, List<StatisticsData> data)
    {
        /// <summary>时间</summary>
        public DateTime DataTime { get; set; } = dataTime;
        /// <summary>统计数据</summary>
        public List<StatisticsData> Data { get; set; } = data;
    }
}
