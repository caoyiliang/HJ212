using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class SendRealTimeDataReq(string? mn, string pw, bool qn, ST st, DateTime dataTime, List<RealTimeData> data, Version version, Func<string, string> func, DateTime? sendTime = null) : IByteStream
    {
        public byte[] ToBytes()
        {
            var rs = $"{(qn ? $"QN={(sendTime == null ? DateTime.Now.ToString("yyyyMMddHHmmssfff") : sendTime.Value.ToString("yyyyMMddHHmmssfff"))};" : "")}ST={(int)st};CN={(int)CN_Client.上传污染物实时数据};PW={pw};MN={mn};Flag={0 | (int)version};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{string.Join(";", data.Select(c => $"{(c.SampleTime != null ? $"{c.Name}-SampleTime={c.SampleTime}," : "")}{c.Name}-Rtd={c.Rtd}{(c.Flag != null ? $",{c.Name}-Flag={c.Flag}" : "")}{(c.EFlag != null ? $",{c.Name}-EFlag={c.EFlag}" : "")}{(c.Other.Count > 0 ? $",{string.Join(",", c.Other.Select(i => $"{c.Name}-{i.Key}={i.Value}"))}" : "")}"))}&&";
            rs = func.Invoke(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
