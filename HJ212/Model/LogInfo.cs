namespace HJ212.Model
{
    /// <summary>
    /// 日志信息
    /// </summary>
    /// <param name="info">日志内容</param>
    /// <param name="dataTime">时间</param>
    public class LogInfo(string info, DateTime? dataTime = default)
    {
        /// <summary>污染物ID</summary>
        public string? PolId { get; set; }
        /// <summary>时间</summary>
        public DateTime DataTime { get; set; } = dataTime ?? DateTime.Now;
        /// <summary>日志内容</summary>
        public string Info { get; set; } = info;
    }
}
