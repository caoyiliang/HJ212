using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class CN1011Req(string? polId, DateTime? time, RspInfo rspInfo) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};{rspInfo.ST};CN=1011;{rspInfo.PW};{rspInfo.MN};Flag={0 | (int)GB._version};CP=&&{(polId is null ? "" : $"PolId={polId};")}SystemTime={time ?? DateTime.Now:yyyyMMddHHmmss}&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
