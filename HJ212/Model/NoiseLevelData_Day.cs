namespace HJ212.Model
{
    /// <summary>
    /// 噪声日数据
    /// </summary>
    /// <param name="name">污染物名称</param>
    /// <param name="data">昼夜等效升级</param>
    /// <param name="dayData">昼间等效升级</param>
    /// <param name="nightData">夜间等效升级</param>
    public class NoiseLevelData_Day(string name, string data, string dayData, string nightData)
    {
        /// <summary>污染物名称</summary>
        public string Name { get; set; } = name;
        /// <summary>昼夜等效升级</summary>
        public string Data { get; set; } = data;
        /// <summary>昼间等效升级</summary>
        public string DayData { get; set; } = dayData;
        /// <summary>夜间等效升级</summary>
        public string NightData { get; set; } = nightData;
    }
}
