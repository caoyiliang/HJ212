using HJ212.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class CN1011Req(string? polId, DateTime? time, RspInfo rspInfo) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};{rspInfo.ST};CN=1011;{rspInfo.PW};{rspInfo.MN};Flag=4;CP=&&{(polId is null ? "" : $"PolId={polId};")}SystemTime={time ?? DateTime.Now:yyyyMMddHHmmss}&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
