namespace HJ212.Model;

/// <summary>
/// 设备运行时间日历史数据
/// </summary>
/// <param name="dataTime">时间</param>
/// <param name="data">设备运行时间数据包集</param>
public class RunningTimeHistory(DateTime dataTime, List<RunningTimeData> data)
{
    /// <summary>时间</summary>
    public DateTime DataTime { get; set; } = dataTime;
    /// <summary>设备运行时间数据包集</summary>
    public List<RunningTimeData> Data { get; set; } = data;
}
