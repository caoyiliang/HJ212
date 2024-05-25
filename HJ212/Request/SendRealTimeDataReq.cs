using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class SendRealTimeDataReq(string mn, string pw, bool qn, ST st, DateTime dataTime, List<RealTimeData> data) : IByteStream
    {
        public byte[] ToBytes()
        {
            var rs = $"{(qn ? $"QN={DateTime.Now:yyyyMMddHHmmssfff};" : "")}ST={(int)st};CN={(int)CN_Client.上传污染物实时数据};PW={pw};MN={mn};Flag={0 | (int)GB._version};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{string.Join(";", data.Select(c => $"{(c.SampleTime != null ? $"{c.Name}-SampleTime={c.SampleTime}," : "")}{c.Name}-Rtd={c.Rtd}{(c.Flag != null ? $",{c.Name}-Flag={c.Flag}" : "")}{(c.EFlag != null ? $",{c.Name}-EFlag={c.EFlag}" : "")}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
