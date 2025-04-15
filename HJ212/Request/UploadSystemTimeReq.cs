using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadSystemTimeReq(string? polId, DateTime? time, RspInfo rspInfo, Version version, Func<string, string> func) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};{rspInfo.ST};CN={(int)CN_Client.上传现场机时间};{rspInfo.PW};{rspInfo.MN};Flag={0 | (int)version};CP=&&{(polId is null ? "" : $"PolId={polId};")}SystemTime={time ?? DateTime.Now:yyyyMMddHHmmss}&&";
            cmd = func.Invoke(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
