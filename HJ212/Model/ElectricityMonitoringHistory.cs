namespace HJ212.Model
{
    /// <summary>
    /// 用电监测历史数据
    /// </summary>
    /// <param name="dataTime">时间</param>
    /// <param name="data">用电统计数据</param>
    public class ElectricityMonitoringHistory(DateTime dataTime, List<ElectricityMonitoringData> data)
    {
        /// <summary>时间</summary>
        public DateTime DataTime { get; set; } = dataTime;
        /// <summary>用电统计数据</summary>
        public List<ElectricityMonitoringData> Data { get; set; } = data;
    }
}
