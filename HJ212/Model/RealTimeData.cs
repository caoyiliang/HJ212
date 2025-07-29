namespace HJ212.Model
{
    /// <summary>
    /// 污染物实时数据包
    /// </summary>
    /// <param name="name">污染物名称</param>
    public class RealTimeData(string name)
    {
        /// <summary>污染物名称</summary>
        public string Name { get; set; } = name;
        /// <summary>污染物实时数据</summary>
        public string? Rtd { get; set; }
        /// <summary>污染物实时数据标记</summary>
        public string? Flag { get; set; }
        /// <summary>污染物实时数据采样时间</summary>
        public string? SampleTime { get; set; }
        /// <summary>污染物对应在线监控（监测）仪器仪表的设备标志</summary>
        public string? EFlag { get; set; }
        /// <summary>污染物标记拓展</summary>
        public Dictionary<string, string> Other { get; set; } = [];
    }
}
