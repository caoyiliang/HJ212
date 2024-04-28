using HJ212.Response;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class SuccessfulReq(RspInfo rspInfo) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};ST=91;CN=9012;{rspInfo.PW};{rspInfo.MN};Flag=4;CP=&&ExeRtn=1&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
