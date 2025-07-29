using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadRealTimeDataReq(string? mn, string pw, ST st, DateTime dataTime, List<RealTimeData> data, Version version, Func<string, string> func) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN={(int)CN_Client.上传污染物实时数据};PW={pw};MN={mn};Flag={1 | (int)version};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{string.Join(";", data.Select(c => $"{(c.SampleTime != null ? $"{c.Name}-SampleTime={c.SampleTime}," : "")}{c.Name}-Rtd={c.Rtd}{(c.Flag != null ? $",{c.Name}-Flag={c.Flag}" : "")}{(c.EFlag != null ? $",{c.Name}-EFlag={c.EFlag}" : "")}{(c.Other.Count > 0 ? $",{string.Join(",", c.Other.Select(i => $"{c.Name}-{i.Key}={i.Value}"))}" : "")}"))}&&";
            rs = func.Invoke(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
