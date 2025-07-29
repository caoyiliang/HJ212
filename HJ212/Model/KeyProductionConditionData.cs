namespace HJ212.Model
{
    /// <summary>关键生产工况实时数据</summary>
    public class KeyProductionConditionData(string name)
    {
        /// <summary>污染物名称</summary>
        public string Name { get; set; } = name;
        /// <summary>大气污染源生产设施工况标记</summary>
        public string? Rtd { get; set; }
    }
}
