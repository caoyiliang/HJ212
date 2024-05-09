using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class SuccessfulReq(RspInfo rspInfo) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};ST=91;CN=9012;{rspInfo.PW};{rspInfo.MN};Flag={0 | (int)GB._version};CP=&&ExeRtn=1&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
