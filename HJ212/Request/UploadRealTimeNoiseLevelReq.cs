using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadRealTimeNoiseLevelReq(string mn, string pw, ST st, DateTime dataTime, float noiseLevel) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN=2011;PW={pw};MN={mn};Flag={1 | (int)GB._version};CP=&&DataTime={dataTime:yyyyMMddHHmmss};LA-Rtd={noiseLevel}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
