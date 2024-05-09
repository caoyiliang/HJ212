namespace HJ212.Model
{
    /// <summary>
    /// 设备运行时间数据包
    /// </summary>
    /// <param name="name">污染物名称</param>
    /// <param name="rt">运行时间</param>
    public class RunningTimeData(string name, string rt)
    {
        /// <summary>污染物名称</summary>
        public string Name { get; set; } = name;
        /// <summary>运行时间</summary>
        public string RT { get; set; } = rt;
    }
}
