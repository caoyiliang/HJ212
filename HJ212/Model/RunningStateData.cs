namespace HJ212.Model
{
    /// <summary>
    /// 运行状态数据包
    /// </summary>
    /// <param name="name">污染物名称</param>
    /// <param name="rs">运行状态</param>
    public class RunningStateData(string name, string rs)
    {
        /// <summary>污染物名称</summary>
        public string Name { get; set; } = name;
        /// <summary>运行状态</summary>
        public string RS { get; set; } = rs;
    }
}
