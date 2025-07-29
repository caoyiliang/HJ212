namespace HJ212.Model
{
    /// <summary>
    /// 污染物的现场端信息
    /// </summary>
    /// <param name="infoId">参数名</param>
    /// <param name="info">参数值</param>
    public class SampleInfo(string infoId, string info)
    {
        /// <summary>参数名</summary>
        public string InfoId { get; set; } = infoId;
        /// <summary>参数值</summary>
        public string Info { get; set; } = info;
    }
}
