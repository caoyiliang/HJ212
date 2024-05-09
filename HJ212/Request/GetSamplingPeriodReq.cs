using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class GetSamplingPeriodReq(string polId, TimeOnly cstartTime, int ctime, RspInfo rspInfo) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};{rspInfo.ST};CN=3017;{rspInfo.PW};{rspInfo.MN};Flag={0 | (int)GB._version};CP=&&PolId={polId};CstartTime={cstartTime:HHmmss};CTime={ctime}&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
