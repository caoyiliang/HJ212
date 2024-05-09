using System.Text;
using TopPortLib.Interfaces;

namespace HJ212.Request
{
    internal class UploadAcquisitionDeviceRestartTimeReq(string mn, string pw, ST st, DateTime dataTime, DateTime restartTime) : IAsyncRequest
    {
        private readonly string _QN = DateTime.Now.ToString("yyyyMMddHHmmssfff");

        public byte[]? Check()
        {
            return Encoding.ASCII.GetBytes(_QN);
        }

        public byte[] ToBytes()
        {
            var rs = $"QN={_QN};ST={(int)st};CN=2081;PW={pw};MN={mn};Flag={1 | (int)GB._version};CP=&&DataTime={dataTime:yyyyMMddHHmmss};RestartTime={restartTime:yyyyMMddHHmmss}&&";
            rs = GB.GetGbCmd(rs);
            return Encoding.ASCII.GetBytes(rs);
        }
    }
}
