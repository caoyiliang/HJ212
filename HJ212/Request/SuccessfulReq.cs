using HJ212.Response;
using System.Text;
using TopPortLib.Interfaces;
using Utils;

namespace HJ212.Request
{
    internal class SuccessfulReq : IByteStream
    {
        private readonly RspInfo _rspInfo;

        public SuccessfulReq(RspInfo rspInfo)
        {
            _rspInfo = rspInfo;
        }

        public byte[] ToBytes()
        {
            var cmd = $"{_rspInfo.QN};{_rspInfo.ST};CN=9012;{_rspInfo.PW};{_rspInfo.MN};Flag=4;CP=&&ExeRtn=1&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
