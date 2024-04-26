using HJ212.Response;
using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class CN1063Req(int minInterval, RspInfo rspInfo) : IByteStream
    {
        public byte[] ToBytes()
        {
            var cmd = $"{rspInfo.QN};{rspInfo.ST};CN=1063;{rspInfo.PW};{rspInfo.MN};Flag=4;CP=&&MinInterval={minInterval}&&";
            cmd = GB.GetGbCmd(cmd);
            return Encoding.ASCII.GetBytes(cmd);
        }
    }
}
