using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class GetRealTimeDataIntervalReq(int rtdInterval, RspInfo rspInfo) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};{rspInfo.ST};CN={(int)CN_Client.上传实时数据间隔};{rspInfo.PW};{rspInfo.MN};Flag={0 | (int)GB._version};CP=&&RtdInterval={rtdInterval}&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
