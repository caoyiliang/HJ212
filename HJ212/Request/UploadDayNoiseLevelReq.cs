using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadDayNoiseLevelReq(string? mn, string pw, ST st, DateTime dataTime, List<NoiseLevelData_Day> data, Version version, Func<string, string> func) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN={(int)CN_Client.上传污染物日历史数据};PW={pw};MN={mn};Flag={1 | (int)version};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{string.Join(";", data.Select(c => $"{c.Name}-Data={c.Data}"))};{string.Join(";", data.Select(c => $"{c.Name}-DayData={c.DayData}"))};{string.Join(";", data.Select(c => $"{c.Name}-NightData={c.NightData}"))}&&";
            rs = func.Invoke(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
