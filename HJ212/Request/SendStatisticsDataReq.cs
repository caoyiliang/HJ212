using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class SendStatisticsDataReq(CN cn, string mn, string pw, bool qn, ST st, DateTime dataTime, List<StatisticsData> data, int pnum, int pno) : IByteStream
    {
        public byte[] ToBytes()
        {
            var rs = $"{(qn ? $"QN={DateTime.Now:yyyyMMddHHmmssfff};" : "")}ST={(int)st};CN={(int)cn};PW={pw};MN={mn};Flag={(pnum > 1 ? $"6;PNUM={pnum};PNO={pno}" : "4")};CP=&&DataTime={dataTime:yyyyMMddHHmmss};{string.Join(";", data.Select(c => $"{(c.Cou != null ? $"{c.Name}-Cou={c.Cou}," : "")}{c.Name}-Min={c.Min},{c.Name}-Avg={c.Avg},{c.Name}-Max={c.Max}{(c.Flag != null ? $",{c.Name}-Flag={c.Flag}" : "")}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
