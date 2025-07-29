using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class HeartbeatReq(string? mn, string pw, ST st, Version version, Func<string, string> func, bool returnValue = true) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN={(int)CN_Client.心跳包};PW={pw};MN={mn};Flag={(returnValue ? 1 : 0) | (int)version};CP=&&&&";
            rs = func.Invoke(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
