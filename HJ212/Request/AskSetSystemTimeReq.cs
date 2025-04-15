using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class AskSetSystemTimeReq(string mn, string pw, ST st, string polId, Version version, Func<string, string> func) : IAsyncRequest
    {
        public byte[]? Check()
        {
            return default;
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={DateTime.Now:yyyyMMddHHmmssfff};ST={(int)st};CN={(int)CN_Client.现场机时间校准请求};PW={pw};MN={mn};Flag={1 | (int)version};CP=&&PolId={polId}&&";
            rs = func.Invoke(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
