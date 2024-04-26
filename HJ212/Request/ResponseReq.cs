using HJ212.Response;
using System.Text;
using TopPortLib.Interfaces;
using Utils;

namespace HJ212.Request
{
    internal class ResponseReq : IByteStream
    {
        private readonly RspInfo _rspInfo;

        public ResponseReq(RspInfo rspInfo)
        {
            _rspInfo = rspInfo;
        }

        public byte[] ToBytes()
        {
            var cmd = $"{_rspInfo.QN};{_rspInfo.ST};CN=9014;{_rspInfo.PW};{_rspInfo.MN};Flag=4;CP=&&QnRtn=1&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
