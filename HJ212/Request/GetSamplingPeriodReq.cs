using HJ212.Model;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class GetSamplingPeriodReq(string polId, TimeOnly cstartTime, int ctime, RspInfo rspInfo, Version version, Func<string, string> func) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};{rspInfo.ST};CN={(int)CN_Client.上传采样时间周期};{rspInfo.PW};{rspInfo.MN};Flag={0 | (int)version};CP=&&PolId={polId};CstartTime={cstartTime:HHmmss};CTime={ctime}&&";
            cmd = func.Invoke(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
