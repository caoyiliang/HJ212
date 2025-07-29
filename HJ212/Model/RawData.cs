namespace HJ212.Model
{
    /// <summary>
    /// 原始数据
    /// </summary>
    /// <param name="name">污染物名称</param>
    public class RawData(string name)
    {
        /// <summary>污染物名称</summary>
        public string Name { get; set; } = name;
        /// <summary>污染物样品从混匀桶内开始采样的时间，表示一个时间点，时间精确到秒</summary>
        public string? SampleTime { get; set; }
        /// <summary>污染物的分析仪测量完成输出测量值的时间</summary>
        public string? CompleteTime { get; set; }
        /// <summary>污染物的采样模式</summary>
        public string? SampleType { get; set; }
        /// <summary>污染物的混合样品监测值</summary>
        public string? Avg { get; set; }
        /// <summary>污染物的现场端信息</summary>
        public List<SampleInfo>? Info { get; set; } = [];
        /// <summary>监测仪器数据标记</summary>
        public string? Flag { get; set; }
    }
}
