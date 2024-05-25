using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadSystemTimeReq(string? polId, DateTime? time, RspInfo rspInfo) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};{rspInfo.ST};CN={(int)CN_Client.上传现场机时间};{rspInfo.PW};{rspInfo.MN};Flag={0 | (int)GB._version};CP=&&{(polId is null ? "" : $"PolId={polId};")}SystemTime={time ?? DateTime.Now:yyyyMMddHHmmss}&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
