using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class SendRealTimeDataReq(string mn, bool flag, string pw, bool qn, ST st, DateTime dataTime, Dictionary<string, (string? value, string? flag)> data) : IByteStream
    {
        public byte[] ToBytes()
        {
            var rs = $"{(qn ? $"QN={DateTime.Now:yyyyMMddHHmmssfff};" : "")}ST={(int)st};CN={(int)CN.实时数据};PW={pw};MN={mn};CP=&&DataTime={dataTime:yyyyMMddHHmmss},{string.Join(";", data.Select(c => $"{c.Key}-Rtd={c.Value.value}{(flag ? $",{c.Key}-Flag={c.Value.flag}" : "")}"))}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
