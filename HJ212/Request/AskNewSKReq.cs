using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class AskNewSKReq(string mn, string pw, ST st, string mSKCreateTime, Version version, Func<string, string> func) : IAsyncRequest
    {
        public byte[]? Check()
        {
            return default;
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={DateTime.Now:yyyyMMddHHmmssfff};ST={(int)st};CN={(int)CN_Client.现场机获取新密钥};PW={pw};MN={mn};Flag={1 | (int)version};CP=&&SKCreateTime={mSKCreateTime}&&";
            rs = func.Invoke(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
