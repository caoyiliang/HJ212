using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class SendStatisticsDataReq(CN_Client cn, string? mn, string pw, bool qn, ST st, DateTime dataTime, List<StatisticsData> data, int pnum, int pno, Version version, Func<string, string> func, DateTime? sendTime = null) : IByteStream
    {
        public byte[] ToBytes()
        {
            var rs = $"{(qn ? $"QN={(sendTime == null ? DateTime.Now.ToString("yyyyMMddHHmmssfff") : sendTime.Value.ToString("yyyyMMddHHmmssfff"))};" : "")}ST={(int)st};CN={(int)cn};PW={pw};MN={mn};Flag={(pnum > 1 ? $"{2 | (int)version};PNUM={pnum};PNO={pno}" : $"{0 | (int)version}")};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{string.Join(";", data.Select(c => $"{(c.Cou != null ? $"{c.Name}-Cou={c.Cou}," : "")}{(c.Min != null ? $"{c.Name}-Min={c.Min}," : "")}{c.Name}-Avg={c.Avg}{(c.Max != null ? $",{c.Name}-Max={c.Max}" : "")}{(c.Flag != null ? $",{c.Name}-Flag={c.Flag}" : "")}{(c.Other.Count > 0 ? $",{string.Join(",", c.Other.Select(i => $"{c.Name}-{i.Key}={i.Value}"))}" : "")}"))}&&";
            rs = func.Invoke(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
