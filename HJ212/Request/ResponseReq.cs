using HJ212.Response;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class ResponseReq(RspInfo rspInfo) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};{rspInfo.ST};CN=9011;{rspInfo.PW};{rspInfo.MN};Flag=4;CP=&&QnRtn=1&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
