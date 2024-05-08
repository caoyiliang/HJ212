using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadStateReq(string mn, string pw, ST st, int flag, DateTime dataTime, string polId, int maintenance, int warn) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN=3020;PW={pw};MN={mn};Flag={flag};CP=&&DataTime={dataTime:yyyyMMddHHmmss};PolId={polId};i12001-Info={maintenance};i12003-Info={warn}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
