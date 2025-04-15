using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class GetSNReq(string polId, string sn, RspInfo rspInfo, Version version, Func<string, string> func) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};{rspInfo.ST};CN={(int)CN_Client.上传设备唯一标识};{rspInfo.PW};{rspInfo.MN};Flag={0 | (int)version};CP=&&PolId={polId};{polId}-SN={sn}&&";
            cmd = func.Invoke(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
