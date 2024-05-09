namespace HJ212.Model
{
    /// <summary>
    /// 噪声数据
    /// </summary>
    /// <param name="name">污染物名称</param>
    /// <param name="data">数值</param>
    public class NoiseLevelData(string name, string data)
    {
        /// <summary>污染物名称</summary>
        public string Name { get; set; } = name;
        /// <summary>数值</summary>
        public string Data { get; set; } = data;
    }
}
