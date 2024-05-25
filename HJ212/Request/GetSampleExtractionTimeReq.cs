using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class GetSampleExtractionTimeReq(string polId, int stime, RspInfo rspInfo) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};{rspInfo.ST};CN={(int)CN_Client.上传出样时间};{rspInfo.PW};{rspInfo.MN};Flag={0 | (int)GB._version};CP=&&PolId={polId};Stime={stime}&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
