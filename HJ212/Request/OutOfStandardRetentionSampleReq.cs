using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class OutOfStandardRetentionSampleReq(DateTime dataTime, int vaseNo, RspInfo rspInfo) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};{rspInfo.ST};CN={(int)CN_Client.上传超标留样信息};{rspInfo.PW};{rspInfo.MN};Flag={1 | (int)GB._version};CP=&&DataTime={dataTime:yyyyMMddHHmmss};VaseNo={vaseNo}&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
