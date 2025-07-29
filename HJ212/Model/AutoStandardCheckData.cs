namespace HJ212.Model
{
    /// <summary>
    /// 自动标样核查（校准）数据
    /// </summary>
    public class AutoStandardCheckData(DateTime dataTime, string polId)
    {
        /// <summary>时间</summary>
        public DateTime DataTime { get; set; } = dataTime;
        /// <summary>污染物标识</summary>
        public string PolId { get; set; } = polId;
        /// <summary>标准溶液实际测量浓度示值</summary>
        public string? SampleRd { get; set; }
        /// <summary>自动标样核查结果或校准后验证结果类型，1表示通过，0表示未通过</summary>
        public string? ResultType { get; set; }
        /// <summary>自动标样核查（校准）标准溶液浓度标称值</summary>
        public string? StandardValue { get; set; }
    }
}
