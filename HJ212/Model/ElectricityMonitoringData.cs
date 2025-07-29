namespace HJ212.Model
{
    /// <summary>
    /// 生产设施的用电监测实时数据
    /// </summary>
    /// <param name="name">生产设施</param>
    public class ElectricityMonitoringData(string name)
    {
        /// <summary>生产设施名称</summary>
        public string Name { get; set; } = name;
        /// <summary>生产设施用电信息的实时数据</summary>
        public string? Rtd { get; set; }
        /// <summary>生产设施的实时数据标识</summary>
        public string? Flag { get; set; }
    }
}
