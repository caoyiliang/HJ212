using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadHardwareSNReq(string? mn, string pw, ST st, string dataLoggerId, string cpuId, string mac1, string mac2, Version version, Func<string, string> func) : IAsyncRequest
    {
        public byte[]? Check()
        {
            return default;
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={DateTime.Now:yyyyMMddHHmmssfff};ST={(int)st};CN={(int)CN_Client.上传数采仪硬件序号};PW={pw};MN={mn};Flag={1 | (int)version};CP=&&i23005-Info={dataLoggerId};i23006-Info={cpuId};i23007-Info={mac1};i23008-Info={mac2}&&";
            rs = func.Invoke(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
