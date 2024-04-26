using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class SendHourDataReq(string mn, bool flag, string pw, bool qn, ST st, DateTime dataTime, Dictionary<string, (string? avgValue, string? max, string? min, string? flag)> data) : IByteStream
    {
        public byte[] ToBytes()
        {
            var rs = $"{(qn ? $"QN={DateTime.Now:yyyyMMddHHmmssfff};" : "")}ST={(int)st};CN={(int)CN.小时数据};PW={pw};MN={mn};CP=&&DataTime={dataTime:yyyyMMddHH0000},{string.Join(";", data.Select(c => $"{c.Key}-Min={c.Value.min},{c.Key}-Avg={c.Value.avgValue},{c.Key}-Max={c.Value.max}{(flag ? $",{c.Key}-Flag={c.Value.flag}" : "")}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
