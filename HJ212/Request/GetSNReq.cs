using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class GetSNReq(string polId, string sn, RspInfo rspInfo) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};{rspInfo.ST};CN=3019;{rspInfo.PW};{rspInfo.MN};Flag={0 | (int)GB._version};CP=&&PolId={polId};{polId}-SN={sn}&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
